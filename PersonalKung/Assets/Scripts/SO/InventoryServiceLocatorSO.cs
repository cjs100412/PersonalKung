using UnityEngine;

[CreateAssetMenu(fileName = "InventoryServiceLocator", menuName = "Scriptable Objects/InventoryServiceLocator")]
public class InventoryServiceLocatorSO : ScriptableObject
{
    public IUserInventroyService Service { get; private set; }
    [SerializeField] InventoryRepositoryLocatorSO inventoryRepositoryLocator;
    public void Bootstrap()
    {
        Service = new UserInventoryService(inventoryRepositoryLocator.Repository);
    }
}
