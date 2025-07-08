using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopText : MonoBehaviour
{
    [Header("메시지 박스에서 출력을 위한 UI 연결")]
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] TextMeshProUGUI _itemDiscription;
    [SerializeField] TextMeshProUGUI _itemPrice;

    [Header("각 패널 연결")]
    [SerializeField] GameObject _shopTextPanel;
    [SerializeField] GameObject _shopNotEnoughTextPanel;

    [SerializeField] PlayerHealth _playerHealth;
    [SerializeField] GameObject _billImage;

    private int _price;
    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    [SerializeField] private InventoryRepositoryLocatorSO _inventoryRepositoryLocator;
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;
    [SerializeField] private InventoryItemServiceLocatorSO inventoryItemServiceLocatorSO;

    private int id;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private ShortcutKey _shortcutKey;
    public Dictionary<int, int> mineralDict = new Dictionary<int, int>();

    [SerializeField] GameObject[] _billTexts;
    [SerializeField] TextMeshProUGUI _totalPrice;
    [SerializeField] private GameObject _mineralNotThingPanel;
    [SerializeField] private GameObject _mineralSellPanel;


    public void SetText(string name, int price, string dis)
    {
        _price = price;
        _itemName.text = name;
        _itemPrice.text = price.ToString();
        _itemDiscription.text = dis;
        _shopTextPanel.SetActive(true);
    }

    public void GetItemId(int itemId)
    {
        id = itemId;
    }

    public void OnClickYesButton()
    {
        if (_playerHealth.gold.IsEnough(_price))
        {
            _playerHealth.gold = _playerHealth.gold.RemoveGold(_price);
            if(id < 2000)
            {
                _shortCutServiceLocator.Service.AddShortCutItemQuantity(id);
            }
            else
            {
                _inventoryServiceLocator.Service.AcquireItem(id);
            }
            _shortcutKey.Refresh();
            _inventoryUI.Refresh();
        }
        else
        {
            SetNotEnoughText();

        }
    }

    public void SetNotEnoughText()
    {
        _shopNotEnoughTextPanel.SetActive(true);

    }

    public void OnClickMineralBillButton()
    {
        _shopTextPanel.SetActive(false);
        _shopNotEnoughTextPanel.SetActive(false);
        List<UserInventoryItemDto> items = _inventoryServiceLocator.Service.Items
            .Where(item => item.ItemId >= 3000)
            .ToList();
        if (items.Count <= 0)
        {
            _mineralNotThingPanel.SetActive(true);
            return;
        }


        mineralDict.Clear();
        foreach (GameObject go in _billTexts)
        {
            go.SetActive(false);
        }
        _billImage.SetActive(true);
        _mineralSellPanel.SetActive(true);




        items.Sort((a, b) => a.ItemId.CompareTo(b.ItemId));

        foreach (UserInventoryItemDto item in items)
        {
            if (mineralDict.ContainsKey(item.ItemId))
            {
                mineralDict[item.ItemId] += item.Quantity;
            }
            else
            {
                mineralDict.Add(item.ItemId, item.Quantity);
            }
        }

        int index = 0;
        int totalPrice = 0;
        foreach (var item in mineralDict)
        {
            _billTexts[index].GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(inventoryItemServiceLocatorSO.ItemService.GetIconPath(item.Key));
            TextMeshProUGUI[] texts = _billTexts[index].GetComponentsInChildren<TextMeshProUGUI>();
            int price = _inventoryRepositoryLocator.Repository.FindById(item.Key).Price;
            texts[0].text = price.ToString() + " ₩";
            texts[1].text = item.Value.ToString();
            texts[2].text = (item.Value * price).ToString() + " ₩";
            totalPrice += item.Value * price;
            _billTexts[index].SetActive(true);
            index++;
        }

        _totalPrice.text = totalPrice.ToString() + " ₩";
        _totalPrice.gameObject.SetActive(true);
    }

    public void OnClickMineralSellButton()
    {
        _billImage.SetActive(false);
        _totalPrice.gameObject.SetActive(false);
        _mineralSellPanel.SetActive(false);
        _playerHealth.gold = _playerHealth.gold.AddGold(_inventoryServiceLocator.Service.SellAllMineral());
        _inventoryUI.Refresh();
    }
}
