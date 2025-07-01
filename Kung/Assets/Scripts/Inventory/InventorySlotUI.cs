using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI itemCountText;

    private Item _items;
    public CustomOreTile.OreType oreType;
    public int count = 0;
    public bool isEmpty = true;

    private void Awake()
    {
        itemCountText.text = "";
    }
    public Item item
    {
        get { return _items; }
        set
        {
            _items = value;
            itemIcon.sprite = item._itemImage;
        }
    }


    //public string IconPath { get; }

    //private void Start()
    //{
    //    const string k_TexturePath = "Textures";

    //    itemIcon.sprite = Resources.Load<Sprite>(Path.Combine(k_TexturePath, IconPath));
    //}
}
