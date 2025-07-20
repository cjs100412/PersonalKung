using System;
using System.Diagnostics;


public class Gold
{
    private int _gold;

    public int gold => _gold;
    private Gold(int gold)
    {
        _gold = gold;
    }

    public static Gold New(int gold) => new Gold(gold);

    public Gold AddGold(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Invalid amount : {amoun}");
        }
        return Gold.New(_gold + amount);
    }

    public Gold RemoveGold(int amount)
    {
        // »ý·«
        if (_gold < amount)
        {
            return Gold.New(_gold);
        }

        return Gold.New(_gold - amount);

    }
    public bool IsEnough(int amount) => _gold >= amount;
    public bool goldEnough;

}
