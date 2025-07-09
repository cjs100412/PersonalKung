using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveButton : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerEquipment _playerEquipment;
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private MineralTile _mineralTile;
    [SerializeField] private RockTile _rockTile;
    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;
    [SerializeField] private GameManagerSO _gameManagerSO;
    [SerializeField] private HUD _ui;
    [SerializeField] private GameObject _SaveCheckPanel;

    public void OnSaveButtonClick()
    {
        _gameManagerSO.GameManager.SaveGame(
            new Vector3(-1, 0.1f, 0),
            _playerHealth.hp.Amount,
            _playerHealth.gold.gold,
            _ui.score,
            _tileManager.destroiedTiles,
            _mineralTile.destroiedMineralTiles,
            _rockTile.destroiedRockTiles,
            _inventoryServiceLocator.Service.Items,
            _shortCutServiceLocator.Service.Items,
            _playerEquipment.equippedHelmet?.itemId ?? 0,
            _playerEquipment.equippedBoots?.itemId ?? 0,
            _playerEquipment.equippedDrill?.itemId ?? 0
        );
        _SaveCheckPanel.gameObject.SetActive(true);
    }
}
