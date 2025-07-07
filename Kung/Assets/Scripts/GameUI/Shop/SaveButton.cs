using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveButton : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PlayerEquipment _playerEquipment;
    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;
    //[SerializeField] private GameManagerSO _gameManagerSO;
    public void OnSaveButtonClick()
    {
        // 기존 SaveGame 로직도 호출
        //_gameManagerSO.GameManager.SaveGame(
        GameManager.I.SaveGame(
            new Vector3(-1, 0, 0),
            _playerHealth.hp.Amount,
            _playerHealth.gold.gold,
            _inventoryServiceLocator.Service.Items,
            _shortCutServiceLocator.Service.Items,
            _playerEquipment.equippedHelmet?.itemId ?? 0,
            _playerEquipment.equippedBoots?.itemId ?? 0,
            _playerEquipment.equippedDrill?.itemId ?? 0
        );
    }
}
