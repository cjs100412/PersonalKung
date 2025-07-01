using System;
using UnityEngine;


[Serializable]
public class Item
{
    public string _itemName;
    public Sprite _itemImage;

    public Item(string itemName, Sprite itemImage)
    {
        _itemImage = itemImage;
        _itemName = itemName;
    }
}
