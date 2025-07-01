using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopText : MonoBehaviour
{
    [SerializeField] Text _itemName;
    [SerializeField] TextMeshProUGUI _itemPrice;
    [SerializeField] Text _itemDiscription;
    [SerializeField] GameObject _shopTextPanel;
    [SerializeField] GameObject _shopNotEnoughTextPanel;
    [SerializeField] private Gold playerGold;
    private int _price;
    private void Start()
    {
        playerGold = Gold.New(10000);

    }
    public void SetText(string name, int price, string dis)
    {
        _price = price;
        _itemName.text = name;
        _itemPrice.text = price.ToString();
        _itemDiscription.text = dis;
        _shopTextPanel.SetActive(true);
    }

    public void OnClickYesButton()
    {
        if (playerGold.IsEnough(_price))
        {
            playerGold = playerGold.RemoveGold(_price);


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

    
}
