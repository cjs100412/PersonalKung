using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int _maxair = 100;
    private int _maxhp = 100;
    private bool _isAirDecrease;
    private bool _isHpDecrease;

    private const float _decreaseAirTime = 3.0f;
    private const float _hpDecreaseInterval = 0.2f;

    private bool _isInvincible;
    private float _invincibleTime = 1.0f;

    [HideInInspector] public bool isDamaged;

    [Header("Head랑 Body 애니메이터 연결")]
    [SerializeField] private Animator _headAnimator;
    [SerializeField] private Animator _bodyAnimator;

    [Header("Damage Object 연결")]
    [SerializeField] private Rigidbody2D _playerRigid;
    [SerializeField] private Collider2D _playerColider;
    [SerializeField] private Animator _damageAnimator;
    [SerializeField] private GameObject _damageObject;
    [SerializeField] private GameObject _headObject;
    [SerializeField] private GameObject _bodyObject;
    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;
    [SerializeField] private InventoryItemServiceLocatorSO _itemServiceLocator;

    public Health hp;
    public Air air;
    public Gold gold;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerEquipment _playerEquipment;
    [SerializeField] private List<EquipmentData> equipmentDatas;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private ShortcutKey _shortcutKey;
    [SerializeField] private Image _helmetEquipment;
    [SerializeField] private Image _bootsEquipment;
    [SerializeField] private Image _drillEquipment;

    public int MaxHp => _maxhp;
    public int MaxAir
    {
        get => _maxair;
        set => _maxair = value;
    }
    

    private void OnEnable()
    {
        _playerStats.OnAirCapacityChanged += HandleAirCapacityChanged;
    }

    private void OnDisable()
    {
        _playerStats.OnAirCapacityChanged -= HandleAirCapacityChanged;
    }

    private void Awake()
    {
        air = Air.New(MaxAir, MaxAir);
    }

    void Update()
    {
        if (hp.IsDead) return;

        if(transform.position.y >= -1 && air.Current < MaxAir)
        {
            air = air.Heal(MaxAir);
        }

        if(transform.position.y < -1 && !_isAirDecrease)
        {
            StartCoroutine(airDecrease());
        }

        if(air.IsAirZero && !_isHpDecrease)
        {
            StartCoroutine(hpDecrease());
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeDamage(5);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            air = air.AirDecrease(5);
            Debug.Log($"현재 산소 :{air.Amount}");
        }
    }

    IEnumerator hpDecrease()
    {
        _isHpDecrease = true;
        yield return new WaitForSeconds(_hpDecreaseInterval);
        hp = hp.TakeDamage(1);
        Debug.Log($"현재 체력 : {hp.Amount}");

        if (hp.IsDead)
        {
            Die();
        }
        _isHpDecrease = false;
    }

    public void TakeDamage(int amount)
    {
        if (hp.IsDead || _isInvincible) return;

        hp = hp.TakeDamage(amount);
        if(hp.IsDead)
        {
            Die();  
            return;
        }

        StartCoroutine(playerAttacked());
        // 코루틴이 시작되면 플레이어를 이동시킬 수 없고, 무적 상태가 됨


        StartCoroutine(Invincible());
        Debug.Log($"현재 체력 : {hp.Amount}");
    }

    public void Die()
    {
        _headAnimator.SetTrigger("isDead");
        _bodyAnimator.SetTrigger("isDead");
        Debug.Log("사망");
    }

    IEnumerator Invincible()
    {
        _isInvincible = true;
        Debug.Log("무적");
        yield return new WaitForSeconds(_invincibleTime);
        _isInvincible = false;
    }
    
    IEnumerator airDecrease()
    {
        _isAirDecrease = true;
        air = air.AirDecrease(1);
        Debug.Log($"현재 산소 :{air.Amount}");
        yield return new WaitForSeconds(_decreaseAirTime);
        _isAirDecrease = false;
    }


    IEnumerator playerAttacked()
    {
        isDamaged = true;
        _damageObject.SetActive(true);
        _damageAnimator.SetBool("isDamaged", true);
        SetPlayerObjectsActive(false);
        _playerRigid.linearVelocity = new Vector2(0, 2);
        yield return new WaitForSeconds(0.8f);
        _damageAnimator.SetBool("isDamaged", false);
        _damageObject.SetActive(false);
        SetPlayerObjectsActive(true);
        isDamaged = false;
    }

    private void SetPlayerObjectsActive(bool isActive)
    {
        _headObject.SetActive(isActive);
        _bodyObject.SetActive(isActive);
    }

    private void HandleAirCapacityChanged(float newCapacity)
    {
        MaxAir = Mathf.FloorToInt(newCapacity);
        if(air.Amount > MaxAir)
        {
            air = Air.New(MaxAir, MaxAir);
        }
        else
        {
            air = Air.New(air.Amount, MaxAir);
        }
    }

    public void Respawn(int savedhp,int savedcoin, List<UserInventoryItemDto> savedInventoryItems, List<UserShortCutItemDto> savedShortCutItems,
                        int equippedHelmetId, int equippedBootsId, int equippedDrillId)
    {
        // 위치 복원
        transform.position = new Vector3(-1, 0, 0);

        // 회전/스케일 초기화 (바닥에 똑바로 세우기)
        transform.rotation = Quaternion.identity;
        var ls = transform.localScale;
        ls.y = Mathf.Abs(ls.y);
        transform.localScale = ls;

        // 물리 충돌 복원
        _playerColider.enabled = true;
        _playerRigid.simulated = true;
        _playerRigid.linearVelocity = Vector2.zero;  // 이전 관성 제거

        // 이동 스크립트 재활성화
        var pm = GetComponent<PlayerMovement>();
        if (pm != null) pm.enabled = true;

        // 각종 상태 복원
        hp = Health.New(savedhp, _maxhp);
        gold = Gold.New(savedcoin);
        _inventoryServiceLocator.Service.SetItems(savedInventoryItems);
        _shortCutServiceLocator.Service.SetShortCut(savedShortCutItems);
        if(equippedHelmetId > 2000)
        {
            _helmetEquipment.sprite = Resources.Load<Sprite>(_itemServiceLocator.ItemService.GetIconPath(equippedHelmetId));
            EquipmentData equipmentHelmetData = equipmentDatas.First(equipment => equipment.itemId == equippedHelmetId);
            _playerEquipment.EquipItem(equipmentHelmetData);
        }
        else
        {
            _playerEquipment.equippedHelmet = new EquipmentData();
        }
        if (equippedHelmetId > 2000)
        {
            _bootsEquipment.sprite = Resources.Load<Sprite>(_itemServiceLocator.ItemService.GetIconPath(equippedBootsId));
            EquipmentData equipmentBootsData = equipmentDatas.First(equipment => equipment.itemId == equippedBootsId);
            _playerEquipment.EquipItem(equipmentBootsData);
        }
        else
        {
            _playerEquipment.equippedBoots = new EquipmentData();
        }
        if (equippedHelmetId > 2000)
        {
            _drillEquipment.sprite = Resources.Load<Sprite>(_itemServiceLocator.ItemService.GetIconPath(equippedDrillId));
            EquipmentData equipmentDrillData = equipmentDatas.First(equipment => equipment.itemId == equippedDrillId);
            _playerEquipment.EquipItem(equipmentDrillData);
        }
        else
        {
            _playerEquipment.equippedDrill = new EquipmentData();
        }
        _inventoryUI.Refresh();
        _shortcutKey.Refresh();

        // 애니메이터 상태 리셋
        _headAnimator.ResetTrigger("isDead");
        _bodyAnimator.ResetTrigger("isDead");
        _headAnimator.Play("Idle");
        _bodyAnimator.Play("Idle");

        Debug.Log($"Player Respawned HP={hp.Amount}");
    }
}
