using System;

public sealed class UserInventoryItemDto
{
    public long SerialNumber { get; }
    public int ItemId { get; }
    public int Quantity { get; }

    public bool IsStackable { get; }
    public bool IsFull => !IsStackable || Quantity >= 5;

    public UserInventoryItemDto(UserInventoryItem item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item), "UserInventoryItem cannot be null");
        }

        SerialNumber = item.SerialNumber;
        ItemId = item.ItemId;
        Quantity = item.Quantity;
        IsStackable = item.IsStackable;
    }
}