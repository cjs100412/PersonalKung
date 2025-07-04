using UnityEngine;

[CreateAssetMenu(fileName = "ShortCutServiceLocatorSO", menuName = "Scriptable Objects/ShortCutServiceSO")]
public class ShortCutServiceLocatorSO : ScriptableObject
{
    public IShortCutService Service;
    [SerializeField] InventoryRepositoryLocatorSO inventoryRepositoryLocator;
    public void Bootstrap()
    {
        Service = new ShortCutService(inventoryRepositoryLocator.Repository);
    }
}
