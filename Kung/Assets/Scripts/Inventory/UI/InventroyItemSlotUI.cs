using NUnit.Framework.Constraints;
using System;
using System.IO;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSlotUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _quantityText;
    [SerializeField] private Sprite _sprite;
    public Sprite currentSprite;

    public bool isItem;
    public int _itemId;
    public int buttonIndex;
    private void Start()
    {
    }
    public void SetData(InventoryItemSlotUIData data)
    {
        Debug.Log(data.Quantity.ToString());
        _icon.sprite = data.IconSprite;
        currentSprite = data.IconSprite;
        _quantityText.text = data.Quantity.ToString();
        _itemId = data.ItemId;
        isItem = true;
    }

    public void RemoveData()
    {
        _icon.sprite = _sprite;
        _quantityText.text = "";
        isItem = false;
        _itemId = 0;
    }

    

    
}
