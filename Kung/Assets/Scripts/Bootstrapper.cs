using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    [SerializeField] private InventoryRepositoryLocatorSO _inventoryRepositoryLocator;
    [SerializeField] private InventoryItemServiceLocatorSO _inventoryItemServiceLocator;

    void Awake()
    {
        _inventoryRepositoryLocator.Bootstrap();
        _inventoryServiceLocator.Bootstrap();
        _inventoryItemServiceLocator.Bootstrap();
    }
}
