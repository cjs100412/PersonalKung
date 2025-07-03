using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IItemRepository
{
    Item FindById(int id);
    IReadOnlyList<Item> FindAll();
}
public class ItemRepository : IItemRepository
{
    private readonly string Path = "ShopItems";
    private List<Item> _items;

    public ItemRepository(IParser<ShopItemList> parser)
    {
        _items = parser.LoadFrom(Path).items.Select(model => ToItem(model)).ToList();
    }

    private Item ToItem(ShopItem model)
    {
        Item item = new Item(model.id, model.dev_name, model.price, model.discription, model.category, model.equipmentType);
        return item;
    }

    public IReadOnlyList<Item> FindAll()
    {
        return _items;
    }

    public Item FindById(int id)
    {
        return _items.Find(item => item.Id == id);
    }

//    public IReadOnlyList<UserInventoryItemDto> LoadItems()
//{
//    // Resources 폴더에서 JSON 불러오기

//    if (jsonFile == null)
//    {
//        throw new Exception("json없음");
//    }

//    _Items = .FromJson<ShopItem>(jsonFile.text);
//}

//    //public IReadOnlyList<UserInventoryItemDto> LoadItems()
//    //{
//    //    if (!File.Exists(FilePath))
//    //    {
//    //        return new List<UserInventoryItemDto>();
//    //    }
//    //    string json = File.ReadAllText(FilePath);
//    //    UserInventoryItemListDto itemList = JsonUtility.FromJson<UserInventoryItemListDto>(json);
//    //    return itemList.Items;
//    //}
}