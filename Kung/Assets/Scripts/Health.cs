using System;
using UnityEngine;

public struct Health
{
    public int Amount { get; }
    public int MaxAmount { get; }

    private Health(int amount, int maxAmount)
    {
        Amount = amount;
        MaxAmount = maxAmount;
    }

    public static Health New(int amount, int maxAmount) => new Health(amount, maxAmount);

    public Health Heal(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException($"Invalid amount : {amount}");
        }
        if( Amount + amount > MaxAmount)
        {
            return Health.New(MaxAmount, MaxAmount);
        }
        else
        {
            return Health.New(Amount + amount, MaxAmount);
        }
    }

    public Health TakeDamage(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException($"Invalid amount : {amount}");
        }

        return Health.New(Amount - amount, MaxAmount);
    }

    public bool IsDead => Amount <= 0;
}