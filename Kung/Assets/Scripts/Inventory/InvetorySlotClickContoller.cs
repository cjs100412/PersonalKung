using UnityEngine;
using UnityEngine.UI;

public class InvetorySlotClickContoller : MonoBehaviour
{

    [SerializeField] private Button[] slots;
    private InventoryItemSlotUI[] slotUIs;
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _headEq;
    [SerializeField] private Image _drillEq;
    [SerializeField] private Image _shoesEq;
    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    [SerializeField] private InventoryUI _inventoryUI;
    private int currentIndex;

    private void Start()
    {
        slotUIs = new InventoryItemSlotUI[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            int index = i; // Capture the current index
            slots[i].onClick.AddListener(() => OnClickItem(index));
            slotUIs[i] = slots[i].GetComponent<InventoryItemSlotUI>();
        }
    }


    public void OnClickItem(int index)
    {
        InventoryItemSlotUI slot = slots[index].GetComponent<InventoryItemSlotUI>();
        if (slot.isItem)
        {
            Debug.Log(slot._itemId);
           _panel.SetActive(true);
            currentIndex = index;
        }
    }

    public void OnClickEqiupmentButton()
    {
        _panel.SetActive(false);
        if (slotUIs[currentIndex]._itemId > 2000 && slotUIs[currentIndex]._itemId < 3000)
        {
            int eqId = slotUIs[currentIndex]._itemId / 100 % 10;
            switch (eqId)
            {
                case 1: // Çï¸ä
                    _headEq.sprite = slotUIs[currentIndex].currentSprite;
                    Debug.Log("Çï¸ä Âø¿ë");
                    break;
                case 2: // µå¸±
                    _drillEq.sprite = slotUIs[currentIndex].currentSprite;
                    Debug.Log("µå¸± Âø¿ë");
                    break;
                case 3: // ½Å¹ß
                    _shoesEq.sprite = slotUIs[currentIndex].currentSprite;
                    Debug.Log("½Å¹ß Âø¿ë");
                    break;
                default:
                    break;
            }
            _inventoryServiceLocator.Service.RemoveItem(slotUIs[currentIndex]._itemId);
            _inventoryUI.Refresh();
        }
        currentIndex = -1;

    }

    public void OnClickCloseButton()
    {
        _inventoryServiceLocator.Service.RemoveItem(slotUIs[currentIndex]._itemId);
        _inventoryUI.Refresh();
        _panel.SetActive(false);
        currentIndex = -1;

    }
}
