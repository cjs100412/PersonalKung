using System;


public class Gold
{
    private int _gold;

    private Gold(int gold)
    {
        _gold = gold;
    }

    public static Gold New(int hp) => new Gold(hp);

    public Gold AddGold(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Invalid amount : {amoun}");
        }
        return new Gold(_gold + amount);
    }

    public Gold RemoveGold(int amount)
    {
        // »ý·«
        if (_gold < amount)
        {
            goldEnough = false;
            return Gold.New(_gold);
        }




        goldEnough = true;

        return Gold.New(_gold - amount);

    }
    public bool goldEnough;
    private bool GoldLess(int amount) => _gold < amount;

}
