using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvetorySlotClickController : MonoBehaviour
{

    [SerializeField] private Button[] slots;
    private InventoryItemSlotUI[] slotUIs;
    [SerializeField] private GameObject _SlotPanel;
    [SerializeField] private GameObject _MineralPanel;

    [SerializeField] private Image _headEq;
    [SerializeField] private Image _drillEq;
    [SerializeField] private Image _shoesEq;
    
    [SerializeField] private List<EquipmentData> equipmentDatas;
    [SerializeField] private PlayerEquipment _playerEquipment;
    [SerializeField] private InventoryUI _inventoryUI;
    
    [SerializeField] private InventoryRepositoryLocatorSO _itemRepositoryLocator;
    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;

    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemDescriptionText;
    [SerializeField] private TextMeshProUGUI _itemPriceText;

    [SerializeField] private TextMeshProUGUI _MineralNameText;
    [SerializeField] private TextMeshProUGUI _MineralPriceText;

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
        if (slot._itemId >= 3000)
        {
            _MineralPanel.transform.position = slots[index].transform.position + new Vector3(0, -250, 0);
            _MineralPanel.SetActive(true);
            currentIndex = index;

            Item item = _itemRepositoryLocator.Repository.FindById(slot._itemId);
            _MineralNameText.text = item.DevName;
            _MineralPriceText.text = item.Price.ToString();
            return;
        }

        if (slot.isItem)
        {
            Debug.Log(slot._itemId);
            _SlotPanel.transform.position = slots[index].transform.position + new Vector3(0, -250, 0);
            _SlotPanel.SetActive(true);
            currentIndex = index;

            Item item = _itemRepositoryLocator.Repository.FindById(slot._itemId);

            _itemNameText.text = item.DevName;
            _itemPriceText.text = item.Price.ToString();
            _itemDescriptionText.text = item.Discription;
        }
    }

    public void OnClickEqiupmentButton()
    {
        _SlotPanel.SetActive(false);
        if (slotUIs[currentIndex]._itemId > 2000 && slotUIs[currentIndex]._itemId < 3000)
        {
            EquipmentData data = null;
            int eqId = slotUIs[currentIndex]._itemId / 100 % 10;
            switch (eqId)
            {
                case 1: // Çï¸ä
                    _headEq.sprite = slotUIs[currentIndex].currentSprite;
                    data = equipmentDatas.First(item => item.itemId == slotUIs[currentIndex]._itemId);
                    Debug.Log("Çï¸ä Âø¿ë");
                    break;
                case 2: // µå¸±
                    _drillEq.sprite = slotUIs[currentIndex].currentSprite;
                    data = equipmentDatas.First(item => item.itemId == slotUIs[currentIndex]._itemId);
                    Debug.Log("µå¸± Âø¿ë");
                    break;
                case 3: // ½Å¹ß
                    _shoesEq.sprite = slotUIs[currentIndex].currentSprite;
                    data = equipmentDatas.First(item => item.itemId == slotUIs[currentIndex]._itemId);
                    Debug.Log("½Å¹ß Âø¿ë");
                    break;
                default:
                    break;
            }
            if (data != null)
            {
                _playerEquipment.EquipItem(data);
            }
            _inventoryServiceLocator.Service.RemoveItem(slotUIs[currentIndex]._itemId);
            _inventoryUI.Refresh();

        }
        currentIndex = -1;
    }

    public void OnClickSellButton()
    {
        _MineralPanel.SetActive(false);
        if (slotUIs[currentIndex]._itemId > 3000)
        {
            _inventoryServiceLocator.Service.RemoveItem(slotUIs[currentIndex]._itemId);
            _inventoryUI.Refresh();
        }
        currentIndex = -1;
    }

    public void OnClickCloseMineralPanelButton()
    {
        _MineralPanel.SetActive(false);
        currentIndex = -1;
    }

    public void OnClickCloseButton()
    {
        _inventoryServiceLocator.Service.RemoveItem(slotUIs[currentIndex]._itemId);
        _inventoryUI.Refresh();
        _SlotPanel.SetActive(false);
        currentIndex = -1;

    }
}
