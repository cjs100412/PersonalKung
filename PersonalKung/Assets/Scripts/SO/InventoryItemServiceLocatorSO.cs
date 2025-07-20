using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemServiceLocatorSO", menuName = "Scriptable Objects/InventoryItemServiceLocatorSO")]
public class InventoryItemServiceLocatorSO : ScriptableObject
{
    public IItemService ItemService { get; private set; }

    public void Bootstrap()
    {
        ItemService = new ItemService();
    }
}
