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
    [SerializeField] private Tilemap _mineralTile;

    // 폭탄이랑 빅폭탄은 누르면 현재위치에 설치 (isGround)체크해야함
    // 다이나마이트는 누르면 현재위치 바로 아래에 설치 (isGround)체크해야함
    // 아래가 돌인지 감지하고 그 돌위치에 소환

    // 구급상자는 플레이어 Health.hp 이런데 접근해서 100 힐
    // 산소는 플레이어 Air.air 이런데 접근해서 100% 힐

    // 모든 버튼들은 누른 뒤에 지금 해당 아이템을 가지고있는지. 수량 확인 해야함
    public void OnBombButtonClick()
    {
        Instantiate(_bomb, _playerTransform.position, Quaternion.identity);
    }
    public void OnBigBombButtonClick()
    {
        Instantiate(_bigBomb, _playerTransform.position, Quaternion.identity);
    }
    public void OnDynamiteButtonClick()
    {
        // 현재 플레이어의 위치 밑의 미네랄 타일을 가져와야함
        Vector3Int DownCell = _mineralTile.WorldToCell(_playerTransform.position + new Vector3(0,-0.1f,0));
        TileBase tile = _mineralTile.GetTile(DownCell);

        if (tile is CustomOreTile customOreTile)
        {
            if(customOreTile.oreType == CustomOreTile.OreType.Rock)
            {
                Instantiate(_dynamite, _mineralTile.CellToWorld(DownCell) + new Vector3(0.15f, 0.15f, 0),Quaternion.identity);
            }
        }
    }
    public void OnFirstAidKitButtonClick()
    {

    }
    public void OnAirCapsuleButtonClick()
    {

    }
}
