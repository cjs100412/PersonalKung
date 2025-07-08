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
    // 폭탄이랑 빅폭탄은 누르면 현재위치에 설치 (isGround)체크해야함
    // 다이나마이트는 누르면 현재위치 바로 아래에 설치 (isGround)체크해야함
    // 아래가 돌인지 감지하고 그 돌위치에 소환

    // 구급상자는 플레이어 Health.hp 이런데 접근해서 100 힐
    // 산소는 플레이어 Air.air 이런데 접근해서 100% 힐

    // 모든 버튼들은 누른 뒤에 지금 해당 아이템을 가지고있는지. 수량 확인 해야함
    public void OnBombButtonClick()
    {
        if (_shortCutServiceLocator.Service.CanUseShortCutItem(1003) && _groundChecker.IsGrounded)
        {
            _shortCutServiceLocator.Service.RemoveShortCutItemQuantity(1003);
            Instantiate(_bomb, _playerTransform.position, Quaternion.identity);
            Refresh();
        }
    }
    public void OnBigBombButtonClick()
    {
        if (_shortCutServiceLocator.Service.CanUseShortCutItem(1004) && _groundChecker.IsGrounded)
        {
            _shortCutServiceLocator.Service.RemoveShortCutItemQuantity(1004);
            Instantiate(_bigBomb, _playerTransform.position, Quaternion.identity);
            Refresh();
        }
    }
    public void OnDynamiteButtonClick()
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
    }
    public void OnFirstAidKitButtonClick()
    {
        if (_shortCutServiceLocator.Service.CanUseShortCutItem(1001))
        {
            _shortCutServiceLocator.Service.RemoveShortCutItemQuantity(1001);
            _playerHealth.hp = _playerHealth.hp.Heal(_playerHealth.MaxHp);
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
