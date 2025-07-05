using System;
using UnityEngine;

public class UserShortCutItemDto
{
    public int ItemId { get; }
    public int Quantity { get; }

    public bool IsEmpty => Quantity == 0;

    public UserShortCutItemDto(UserShortCutItem item)
    {
        ItemId = item.ItemId;
        Quantity = item.Quantity;
    }
}
