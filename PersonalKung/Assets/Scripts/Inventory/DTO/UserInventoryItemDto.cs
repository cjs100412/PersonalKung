using System;

[Serializable]
public sealed class UserInventoryItemDto
{

    public long SerialNumber;
    public int ItemId;
    public int Quantity;
    public bool IsStackable;
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