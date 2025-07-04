using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public interface IShortCutService
{
    IReadOnlyList<UserShortCutItemDto> Items { get; }
    void AddShortCutItem(int itemId);
    public void AddShortCutItemQuantity(int itemId);
    public void RemoveShortCutItemQuantity(int itemId);
    public bool CanUseShortCutItem(int itemId);
}
public class ShortCutService : IShortCutService
{
    private readonly ShortCut _shortCut;
    private readonly IItemRepository _itemRepository;

    public ShortCutService(IItemRepository itemRepository)
    {
        _shortCut = new ShortCut();
        _itemRepository = itemRepository;
    }
    public IReadOnlyList<UserShortCutItemDto> Items
    {
        get
        {
            return _shortCut.Items
                .Select(item => new UserShortCutItemDto(item))
                .ToList();
        }
    }
    public void AddShortCutItem(int itemId)
    {
        UserShortCutItem newItem = UserShortCutItem.Acquire(itemId);
        _shortCut.AddItem(newItem);
    }

    public void AddShortCutItemQuantity(int itemId)
    {
        _shortCut.AddQuantity(itemId);
    }

    public bool CanUseShortCutItem(int itemId)
    {
        return _shortCut.CanUseShortCutItem(itemId);
    }

    public void RemoveShortCutItemQuantity(int itemId)
    {
        _shortCut.RemoveQuantity(itemId);
    }
}
