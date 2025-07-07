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

    private int _price;
    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;
    private int id;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private ShortcutKey _shortcutKey;
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

    public void OnClickMineralSellButton()
    {
        _playerHealth.gold = _playerHealth.gold.AddGold(_inventoryServiceLocator.Service.SellAllMineral());
        _inventoryUI.Refresh();
    }
}
