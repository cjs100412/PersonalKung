using System;

public class Item
{
    public Item(int id, string devName, int price, string description, string category, string equipmentType)
    {
        Id = id;
        DevName = devName;
        Price = price;
        Discription = description;
        Category = category;
        EquipmentType = equipmentType;
    }

    public int Id { get; }
    public string DevName { get; }
    public int Price { get; }
    public string Discription { get; }
    public string Category { get; }
    public string EquipmentType { get; }
}
