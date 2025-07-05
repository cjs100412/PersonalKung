using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveButton : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;
    public void OnSaveButtonClick()
    {
        // 2) ���� SaveGame ������ ȣ��
        GameManager.I.SaveGame(
            new Vector3(-1, 0, 0),
            _playerHealth.hp.Amount,
            _playerHealth.gold.gold,
            _inventoryServiceLocator.Service.Items,
            _shortCutServiceLocator.Service.Items
        );
    }
}
