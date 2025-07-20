using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    [SerializeField] private InventoryRepositoryLocatorSO _inventoryRepositoryLocator;
    [SerializeField] private InventoryItemServiceLocatorSO _inventoryItemServiceLocator;
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;
    [SerializeField] private GameManagerSO _gameManagerSO;

    void Awake()
    {
        _inventoryRepositoryLocator.Bootstrap();
        _inventoryServiceLocator.Bootstrap();
        _inventoryItemServiceLocator.Bootstrap();
        _shortCutServiceLocator.Bootstrap();
        _gameManagerSO.Bootstrap();
    }
}
