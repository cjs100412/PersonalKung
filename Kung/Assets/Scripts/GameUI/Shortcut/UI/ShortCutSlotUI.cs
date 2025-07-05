using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShortCutSlotUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _quantityText;

    public int _itemId;

    public void SetData(ShortCutSlotUIData data)
    {
        _icon.sprite = data.IconSprite;
        _quantityText.text = data.Quantity.ToString();
        _itemId = data.ItemId;
    }
}
