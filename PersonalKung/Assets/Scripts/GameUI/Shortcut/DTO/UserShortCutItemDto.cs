using System;
using UnityEngine;

[Serializable]
public class UserShortCutItemDto
{
    public int ItemId;
    public int Quantity;

    public bool IsEmpty => Quantity == 0;

    public UserShortCutItemDto(UserShortCutItem item)
    {
        ItemId = item.ItemId;
        Quantity = item.Quantity;
    }
}
