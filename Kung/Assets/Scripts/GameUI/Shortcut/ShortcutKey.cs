using UnityEngine;
using UnityEngine.Tilemaps;

public class ShortcutKey : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private GroundChecker _groundChecker;
    [SerializeField] private GameObject _bomb;
    [SerializeField] private GameObject _bigBomb;
    [SerializeField] private GameObject _dynamite;
    [SerializeField] private Tilemap _rockTile;

    private float bombCooldownTime = 3f;
    private float cooldownTime;


    private ShortCutSlotUI[] _slots;

    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;
    [SerializeField] private InventoryItemServiceLocatorSO _itemServiceLocator;

    private void Awake()
    {
        _slots = gameObject.GetComponentsInChildren<ShortCutSlotUI>(gameObject);
        Refresh();
    }

    public void Refresh()
    {
        var ShortCutItems = _shortCutServiceLocator.Service.Items;
        for (int i = 0; i < ShortCutItems.Count; i++)
        {
            var viewData = new ShortCutSlotUIData(ShortCutItems[i], _itemServiceLocator.ItemService);
            _slots[i].SetData(viewData);
        }
    }

    private void Update()
    {
        cooldownTime += Time.deltaTime;
    }

    public void OnBombButtonClick()
    {
        if(cooldownTime > bombCooldownTime)
        {
            if (_shortCutServiceLocator.Service.CanUseShortCutItem(1003) && _groundChecker.IsGrounded)
            {
                _shortCutServiceLocator.Service.RemoveShortCutItemQuantity(1003);
                Instantiate(_bomb, _playerTransform.position, Quaternion.identity);
                Refresh();
            }
            cooldownTime = 0f;
        }
    }
    public void OnBigBombButtonClick()
    {
        if (cooldownTime > bombCooldownTime)
        {
            if (_shortCutServiceLocator.Service.CanUseShortCutItem(1004) && _groundChecker.IsGrounded)
            {
                _shortCutServiceLocator.Service.RemoveShortCutItemQuantity(1004);
                Instantiate(_bigBomb, _playerTransform.position, Quaternion.identity);
                Refresh();
            }
            cooldownTime = 0f;
        }
    }
    public void OnDynamiteButtonClick()
    {
        if (cooldownTime > bombCooldownTime)
        {
            if (_groundChecker.IsGrounded == true)
            {
                // 현재 플레이어의 위치 밑의 미네랄 타일을 가져와야함
                Vector3Int DownCell = _rockTile.WorldToCell(_playerTransform.position + new Vector3(0, -0.1f, 0));
                TileBase tile = _rockTile.GetTile(DownCell);

                if (tile is CustomOreTile customOreTile)
                {
                    if (customOreTile.oreType == CustomOreTile.OreType.Rock)
                    {
                        if (_shortCutServiceLocator.Service.CanUseShortCutItem(1005))
                        {
                            _shortCutServiceLocator.Service.RemoveShortCutItemQuantity(1005);
                            Instantiate(_dynamite, _rockTile.CellToWorld(DownCell) + new Vector3(0.15f, 0.15f, 0), Quaternion.identity);
                            Refresh();
                        }
                    }
                }
            }
            cooldownTime = 0f;
        }
    }
    public void OnFirstAidKitButtonClick()
    {
        if (_shortCutServiceLocator.Service.CanUseShortCutItem(1001))
        {
            _shortCutServiceLocator.Service.RemoveShortCutItemQuantity(1001);
            _playerHealth.hp = _playerHealth.hp.Heal(_playerHealth.MaxHp);
            SoundManager.Instance.PlaySFX(SFX.Medicbox);
            Refresh();
        }
    }
    public void OnAirCapsuleButtonClick()
    {
        if (_shortCutServiceLocator.Service.CanUseShortCutItem(1002))
        {
            _shortCutServiceLocator.Service.RemoveShortCutItemQuantity(1002);
            _playerHealth.air = _playerHealth.air.Heal(_playerHealth.MaxAir);
            Refresh();
        }
    }
}
