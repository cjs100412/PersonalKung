using System;
using UnityEngine;

public struct Air
{
    public int Amount { get; }
    public int MaxAmount { get; }

    public int Current => Amount;
    private Air(int amount , int maxAmount)
    {
        Amount = amount;
        MaxAmount = maxAmount;
    }
    public static Air New(int amount,int maxamount) => new Air(amount, maxamount);

    public Air Heal(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException($"Invalid amount : {amount}");
        }
        if (Amount + amount > MaxAmount)
        {
            return Air.New(MaxAmount, MaxAmount);
        }
        else
        {
            return Air.New(Amount + amount, MaxAmount);
        }
    }

    public Air AirDecrease(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException($"Invalid amount : {amount}");
        }

        return Air.New(Amount - amount, MaxAmount);
    }

    public bool IsAirZero => Amount <= 0;
}