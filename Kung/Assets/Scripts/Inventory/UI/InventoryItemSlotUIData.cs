using System;
using UnityEngine;

public sealed class InventoryItemSlotUIData
{
    public int Quantity { get; }
    public Sprite IconSprite { get; }

    public int ItemId { get; }
    public InventoryItemSlotUIData(UserInventoryItemDto dto, IItemService itemService)
    {
        if (dto == null)
        {
            throw new ArgumentNullException(nameof(dto), "UserInventoryItemDto cannot be null");
        }

        Quantity = dto.Quantity;
        IconSprite = Resources.Load<Sprite>(itemService.GetIconPath(dto.ItemId));
        ItemId = dto.ItemId;

        if (IconSprite == null)
        {
            throw new InvalidOperationException($"Icon sprite for Item ID {dto.ItemId} not found.");
        }
    }
}