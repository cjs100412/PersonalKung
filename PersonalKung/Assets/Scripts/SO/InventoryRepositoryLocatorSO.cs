using UnityEngine;

[CreateAssetMenu(fileName = "InventoryRepositoryLocatorSO", menuName = "Scriptable Objects/InventoryRepositoryLocatorSO")]
public class InventoryRepositoryLocatorSO : ScriptableObject
{
    public IItemRepository Repository { get; private set; }

    public void Bootstrap()
    {
        Repository = new ItemRepository(new Parser<ShopItemList>());
    }
}
