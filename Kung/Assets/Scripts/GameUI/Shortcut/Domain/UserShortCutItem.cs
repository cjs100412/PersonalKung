using System;
using UnityEngine;

public class UserShortCutItem
{
    public int ItemId { get; }
    public int Quantity { get; private set; }
    public bool IsEmpty => Quantity == 0;

    public static UserShortCutItem Acquire(int itemId)
    {
        return new UserShortCutItem(
                itemId: itemId,
                quantity: 0
            );
    }

    private UserShortCutItem(int itemId, int quantity)
    {
        ItemId = itemId;
        Quantity = quantity;
    }

    public void AddQuantity()
    {
        Quantity += 1;
    }

    public void RemoveQuantity()
    {
        Quantity -= 1;
    }
}
