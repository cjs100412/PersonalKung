using System;
using System.Collections.Generic;
using UnityEngine;

public class ShortCut
{
    List<UserShortCutItem> items = new List<UserShortCutItem>();

    public ShortCut()
    {
        items.Add(UserShortCutItem.Acquire(1003));
        items.Add(UserShortCutItem.Acquire(1004));
        items.Add(UserShortCutItem.Acquire(1005));
        items.Add(UserShortCutItem.Acquire(1001));
        items.Add(UserShortCutItem.Acquire(1002));
    }
    public List<UserShortCutItem> Items
    {
        get { return items; }
    }
    public void SetShortCut(List<UserShortCutItem> setShortCutItems)
    {
        items = setShortCutItems;
    }
    public void AddItem(UserShortCutItem item)
    {
         items.Add(item);
    }

    public void AddQuantity(int itemId)
    {
        UserShortCutItem item = items.Find(i => i.ItemId == itemId);
        if (item != null)
        {
            item.AddQuantity();
        }
        else
        {
            throw new KeyNotFoundException($"Item with ID {itemId} not found in shortcut.");
        }
    }

    public void RemoveQuantity(int itemId)
    {
        UserShortCutItem item = items.Find(i => i.ItemId == itemId);
        if (item != null)
        {
            if (item.Quantity != 0)
            {
                item.RemoveQuantity();
            }
        }
        else
        {
            throw new KeyNotFoundException($"Item with ID {itemId} not found in shortcut.");
        }
    }

    public bool CanUseShortCutItem(int itemId)
    {
        UserShortCutItem item = items.Find(i => i.ItemId == itemId);

        return !item.IsEmpty;
    }

    
}
