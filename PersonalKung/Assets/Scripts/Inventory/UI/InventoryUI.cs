using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;


public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryItems;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private TextMeshProUGUI _goldText;

    private InventoryItemSlotUI[] _slots;

    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    [SerializeField] private InventoryRepositoryLocatorSO _inventoryRepositoryLocator;
    [SerializeField] private InventoryItemServiceLocatorSO _itemServiceLocator;
    // Temp

    private void Awake()
    {
        _slots = _inventoryItems.GetComponentsInChildren<InventoryItemSlotUI>(_inventoryItems);
    }

    public void Refresh()
    {
        var inventoryItems = _inventoryServiceLocator.Service.Items;
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            var viewData = new InventoryItemSlotUIData(inventoryItems[i], _itemServiceLocator.ItemService);
            _slots[i].SetData(viewData);
        }
        for (int i = inventoryItems.Count; i < _slots.Length; i++)
        {
            _slots[i].RemoveData();
        }
        _goldText.text = _playerHealth.gold.gold.ToString();
    }

    [SerializeField] private float _UIspeed = 0.5f;
    [SerializeField] private Transform _quitTransform;
    public void OnClickCloseButton()
    {
        SoundManager.Instance.PlaySFX(SFX.InventoryClose);
        transform.DOLocalMove(_quitTransform.localPosition, _UIspeed);
    }
}
