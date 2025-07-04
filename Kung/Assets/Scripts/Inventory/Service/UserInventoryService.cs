using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using System;

public interface IUserInventroyService
{
    IReadOnlyList<UserInventoryItemDto> Items { get; }
    void AcquireItem(int itemId);
    void SellAllMineral();
    bool CanAcquireItem(int itemId);

    void RemoveItem(int itemId);
}


public class UserInventoryService : IUserInventroyService
{
    private readonly Inventory _inventory;
    private readonly ItemService _itemService;
    private readonly IItemRepository _itemRepository;

    public UserInventoryService(IItemRepository itemRepository)
    {
        _inventory = new Inventory();
        _itemService = new ItemService();
        _itemRepository = itemRepository;
        //_itemRepository = new ItemRepository(new Parser<ShopItemList>());
    }

    public IReadOnlyList<UserInventoryItemDto> Items
    {
        get
        {
            return _inventory.Items
                .Select(item => new UserInventoryItemDto(item))
                .ToList();
        }
    }

    public void AcquireItem(int itemId)
    {
        bool isMineral = _itemService.CheckIsMineral(itemId);
        UserInventoryItem newItem = UserInventoryItem.Acquire(itemId, isMineral);
        _inventory.AddItem(newItem);
    }

    public bool CanAcquireItem(int itemId)
    {
        return _inventory.CanAddItem(itemId);
    }

    public void SellAllMineral()
    {
        int allprice = 0;

        foreach (UserInventoryItemDto item in Items)
        {
            if (item.IsStackable)
            {
                var repoItem = _itemRepository.FindById(item.ItemId);
                if (repoItem != null)
                {
                    allprice += repoItem.Price * item.Quantity;
                    _inventory.RemoveItem(item.ItemId);
                }
                else
                {
                    Debug.LogWarning($"아이템 ID {item.ItemId}에 해당하는 데이터가 ItemRepository에 없습니다.");
                }
            }
        }

        Debug.Log("모든 광물을 판매하여 얻은 금액: " + allprice);
    }

    public void RemoveItem(int itemId)
    {
        _inventory.RemoveItem(itemId);
    }
}
