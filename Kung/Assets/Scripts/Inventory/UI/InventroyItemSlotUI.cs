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
    [SerializeField] private Sprite sprite;

    private void Start()
    {
    }
    public void SetData(InventoryItemSlotUIData data)
    {
        Debug.Log(data.Quantity.ToString());
        _icon.sprite = data.IconSprite;
        _quantityText.text = data.Quantity.ToString();
    }

    public void RemoveData()
    {
        _icon.sprite = sprite;
        _quantityText.text = "";
    }
}
