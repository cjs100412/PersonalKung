using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int _maxair = 100;
    private int _maxhp = 100;
    private bool _isAirDecrease;
    private bool _isHpDecrease;

    private const float _decreaseAirTime0 = 1.5f;
    private const float _decreaseAirTime1 = 1.0f;
    private const float _decreaseAirTime2 = 0.5f;
    private const float _decreaseAirTime3 = 0.3f;
    private const float _decreaseAirTime4 = 0.2f;
    private const float _decreaseAirTime5 = 0.1f;
    private const float _decreaseAirTime6 = 0.05f;

    private const float _hpDecreaseInterval = 0.2f;

    private bool _isInvincible;
    private float _invincibleTime = 2.5f;

    [HideInInspector] public bool isDamaged;

    [Header("Head랑 Body 애니메이터 연결")]
    [SerializeField] private Animator _headAnimator;
    [SerializeField] private Animator _bodyAnimator;

    [Header("Damage Object 연결")]
    [SerializeField] private Rigidbody2D _playerRigid;
    [SerializeField] private Collider2D _playerColider;
    [SerializeField] private Animator _damageAnimator;
    [SerializeField] private Animator _playerDieAnimator;
    [SerializeField] private GameObject _damageObject;
    [SerializeField] private GameObject _headObject;
    [SerializeField] private GameObject _bodyObject;
    [SerializeField] private GameObject _playerDieObject;
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

    [SerializeField] private GameObject boostLight;
    [SerializeField] private GameObject _playerDiePanel;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private HUD _HUD;

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
        hp = Health.New(_maxhp, _maxhp);
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

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    TakeDamage(5);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    air = air.AirDecrease(5);
        //    Debug.Log($"현재 산소 :{air.Amount}");
        //}
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
        boostLight.SetActive(false);
        int damage = amount - (int)_playerStats.defense;
        hp = hp.TakeDamage(damage);
        if(hp.IsDead)
        {
            Die();  
            return;
        }

        StartCoroutine(playerAttacked());


        StartCoroutine(Invincible());
        Debug.Log($"현재 체력 : {hp.Amount}");
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
        if (air.Amount <= 0) yield break;

        if (transform.position.y < -180)
            yield return new WaitForSeconds(_decreaseAirTime6);
        else if(transform.position.y < -150)
            yield return new WaitForSeconds(_decreaseAirTime5);
        else if(transform.position.y < -120)
            yield return new WaitForSeconds(_decreaseAirTime4);
        else if(transform.position.y < -90)
            yield return new WaitForSeconds(_decreaseAirTime3);
        else if(transform.position.y < -60)
            yield return new WaitForSeconds(_decreaseAirTime2);
        else if(transform.position.y < -30)
            yield return new WaitForSeconds(_decreaseAirTime1);
        else
            yield return new WaitForSeconds(_decreaseAirTime0);
        Debug.Log($"현재 산소 :{air.Amount}");
        _isAirDecrease = false;
    }

    public void Die()
    {
        SoundManager.Instance.PlaySFX(SFX.PlayerDead);
        StartCoroutine(playerDie());
        Debug.Log("사망");
    }

    IEnumerator playerDie()
    {
        SoundManager.Instance.PlaySFX(SFX.PlayerDead);
        _playerDiePanel.SetActive(true);
        _playerDieObject.SetActive(true);
        _playerDieAnimator.SetBool("isDie", true);
        _scoreText.text = "SCORE : " + _HUD.score.ToString();
        SetPlayerObjectsActive(false);
        gameObject.GetComponent<Player>().enabled = false;
        yield return new WaitForSeconds(0.8f);
    }

    IEnumerator playerAttacked()
    {
        isDamaged = true;
        _damageObject.SetActive(true);
        _damageAnimator.SetBool("isDamaged", true);
        SetPlayerObjectsActive(false);
        _playerRigid.linearVelocity = new Vector2(0, 2);
        SoundManager.Instance.PlaySFX(SFX.PlayerDamaged);
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
        transform.position = new Vector3(-1, 0.1f, 0);

        transform.rotation = Quaternion.identity;
        var ls = transform.localScale;
        ls.y = Mathf.Abs(ls.y);
        transform.localScale = ls;

        _playerColider.enabled = true;
        _playerRigid.simulated = true;
        _playerRigid.linearVelocity = Vector2.zero;

        var pm = GetComponent<Player>();
        if (pm != null) pm.enabled = true;

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
        if (equippedBootsId > 2000)
        {
            _bootsEquipment.sprite = Resources.Load<Sprite>(_itemServiceLocator.ItemService.GetIconPath(equippedBootsId));
            EquipmentData equipmentBootsData = equipmentDatas.First(equipment => equipment.itemId == equippedBootsId);
            _playerEquipment.EquipItem(equipmentBootsData);
        }
        else
        {
            _playerEquipment.equippedBoots = new EquipmentData();
        }
        if (equippedDrillId > 2000)
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

        _headAnimator.ResetTrigger("isDead");
        _bodyAnimator.ResetTrigger("isDead");
        _headAnimator.Play("Idle");
        _bodyAnimator.Play("Idle");

        Debug.Log($"Player Respawned HP={hp.Amount}");
    }
}
