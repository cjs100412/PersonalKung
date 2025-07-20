using System;

public class UserInventoryItem
{
    public long SerialNumber { get; }
    public int ItemId { get; }
    public int Quantity { get; private set; }
    public bool IsStackable { get; }
    public bool IsFull
    {
        get
        {
            if (IsStackable == false)
            {
                return true;
            }

            if (IsStackable && Quantity >= 5)
            {
                return true;
            }

            return false;
        }
    }

    public static UserInventoryItem Acquire(int itemId, bool isStackable)
    {
        return new UserInventoryItem(
                serialNumber: DateTime.UtcNow.Ticks,
                itemId: itemId,
                quantity: 1,
                isStackable: isStackable
            );    
    }

    public UserInventoryItem(long serialNumber, int itemId, int quantity, bool isStackable)
    {
        SerialNumber = serialNumber;
        ItemId = itemId;
        Quantity = quantity;
        IsStackable = isStackable;
    }

    public void Stack(UserInventoryItem other)
    {
        if (IsStackable == false || other.IsStackable == false)
        {
            throw new InvalidOperationException("Cannot stack non-stackable items.");
        }

        if (ItemId != other.ItemId)
        {
            throw new InvalidOperationException("Cannot stack items with different IDs.");
        }

        Quantity += other.Quantity;
    }

    public bool CanStack(int itemId) => itemId == ItemId && !IsFull;
}
