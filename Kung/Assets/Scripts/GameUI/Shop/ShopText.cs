using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopText : MonoBehaviour
{
    [Header("�޽��� �ڽ����� ����� ���� UI ����")]
    [SerializeField] Text _itemName;
    [SerializeField] Text _itemDiscription;
    [SerializeField] TextMeshProUGUI _itemPrice;

    [Header("�� �г� ����")]
    [SerializeField] GameObject _shopTextPanel;
    [SerializeField] GameObject _shopNotEnoughTextPanel;

    private Gold playerGold;
    private int _price;
    [SerializeField] private InventoryServiceLocatorSO _inventoryServiceLocator;
    [SerializeField] private ShortCutServiceLocatorSO _shortCutServiceLocator;
    private int id;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private ShortcutKey _shortcutKey;
    private void Start()
    {
        playerGold = Gold.New(1000000);

    }
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
        if (playerGold.IsEnough(_price))
        {
            playerGold = playerGold.RemoveGold(_price);
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
        _inventoryServiceLocator.Service.SellAllMineral();
    }
}
