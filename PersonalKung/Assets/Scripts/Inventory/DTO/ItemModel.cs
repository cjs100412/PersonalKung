using System;

[Serializable]
public struct ShopItem
{
    public int id;
    public string dev_name;
    public int price;
    public string discription;
    public string category;
    public string equipmentType;
}

[Serializable]
public struct ShopItemList
{
    public ShopItem[] items;
}