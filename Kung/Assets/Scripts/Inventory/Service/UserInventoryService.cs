using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using System;

public interface IUserInventroyService
{
    public List<UserInventoryItemDto> Items { get; }
    public void SetItems(List<UserInventoryItemDto> setItems);

    void AcquireItem(int itemId);
    int SellAllMineral();
    bool CanAcquireItem(int itemId);

    void RemoveItem(int itemId);
}


public class UserInventoryService : IUserInventroyService
{
    private readonly Inventory _inventory;
    private readonly ItemService _itemService;
    private readonly IItemRepository _itemRepository;
    //public Dictionary<int, int> mineralDict = new Dictionary<int, int>();
    public UserInventoryService(IItemRepository itemRepository)
    {
        _inventory = new Inventory();
        _itemService = new ItemService();
        _itemRepository = itemRepository;
        //_itemRepository = new ItemRepository(new Parser<ShopItemList>());
    }

    public List<UserInventoryItemDto> Items
    {
        get
        {
            return _inventory.Items
                .Select(item => new UserInventoryItemDto(item))
                .ToList();
        }
    }

    public void SetItems(List<UserInventoryItemDto> setItems)
    {
        _inventory.SetItems(setItems
                .Select(item => new UserInventoryItem(item.SerialNumber,item.ItemId,item.Quantity,item.IsStackable))
                .ToList());
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

    // ���ؼ� ��ȯ�� int�� ���� ������;; �̷����ϸ� �ȵǴµ� ���߿� ���������� ������
    public int SellAllMineral()
    {
        int allprice = 0;

        foreach (UserInventoryItemDto item in Items)
        {
            if (item.IsStackable)
            {
                var repoItem = _itemRepository.FindById(item.ItemId);
                if (repoItem != null)
                {
                    //mineralDict[item.ItemId] += item.Quantity;
                    allprice += repoItem.Price * item.Quantity;
                    _inventory.RemoveItem(item.ItemId);
                }
                else
                {
                    Debug.LogWarning($"������ ID {item.ItemId}�� �ش��ϴ� �����Ͱ� ItemRepository�� �����ϴ�.");
                }
            }
        }
        Debug.Log("��� ������ �Ǹ��Ͽ� ���� �ݾ�: " + allprice);
        return allprice;
    }

    public void RemoveItem(int itemId)
    {
        _inventory.RemoveItem(itemId);
    }
}
