using System;
using System.Collections.Generic;
using System.Linq;

public class Inventory
{
    List<UserInventoryItem> items = new List<UserInventoryItem>();
    public bool IsSlotCountFull => items.Count >= 25;

    public IReadOnlyList<UserInventoryItem> Items
    {
        get { return items.AsReadOnly(); }
    }

    public void AddItem(UserInventoryItem item)
    {
        UserInventoryItem? existingItem = items
            .FirstOrDefault(existing => existing.CanStack(item.ItemId));
        if (existingItem == null)
        {
            items.Add(item);
        }
        else
        {
            existingItem.Stack(item);
        }
    }

    public void RemoveItem(int id)
    {
        UserInventoryItem itemToRemove = items.First(item => item.ItemId == id);
        items.Remove(itemToRemove);
    }

    public bool CanAddItem(int itemId)
    {
        if (IsSlotCountFull == false)
        {
            return true;
        }

        return items.Any(item => item.CanStack(itemId));
    }
}
