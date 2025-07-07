using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopText : MonoBehaviour
{
    [Header("메시지 박스에서 출력을 위한 UI 연결")]
    [SerializeField] Text _itemName;
    [SerializeField] Text _itemDiscription;
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

    [SerializeField] GameObject _billLine;
    //[SerializeField] Image _billmineralImage;
    //[SerializeField] TextMeshProUGUI _billmineralCount;
    //[SerializeField] TextMeshProUGUI _billmineralTotal;
    [SerializeField] Transform _par;

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
        mineralDict.Clear();

        _billImage.SetActive(true);
        _shopTextPanel.SetActive(false);
        _shopNotEnoughTextPanel.SetActive(false);
        List<UserInventoryItemDto> items = _inventoryServiceLocator.Service.Items
            .Where(item => item.ItemId >= 3000)
            .ToList();

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

        float yPos = 0;
        foreach (var item in mineralDict)
        {
            GameObject go = Instantiate(_billLine,_par);
            go.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, yPos, 0);
            go.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(inventoryItemServiceLocatorSO.ItemService.GetIconPath(item.Key));
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();
            int price = _inventoryRepositoryLocator.Repository.FindById(item.Key).Price;
            texts[0].text = price.ToString();
            texts[1].text = item.Value.ToString();
            texts[2].text = (item.Value * price).ToString();
            yPos -= 120f;
        }
    }

    public void OnClickMineralSellButton()
    {
        _playerHealth.gold = _playerHealth.gold.AddGold(_inventoryServiceLocator.Service.SellAllMineral());
        _inventoryUI.Refresh();
    }
}
