class PlayerAbility
{
    public float movementSpeed = 0;
    public float bosterSpeed = 0;
    public float defance = 0;
    public float drillDamage = 0;
    public float airCapacity = 0;

    public PlayerAbility(float movementSpeed, float bosterSpeed, float defance, float drillDamage, float airCapacity)
    {
        this.movementSpeed = movementSpeed;
        this.bosterSpeed = bosterSpeed;
        this.defance = defance;
        this.drillDamage = drillDamage;
        this.airCapacity = airCapacity;
    }


    //public void GetAbility(PlayerMovement movement, Drilling drilling)
    //{
    //    movement.movementSpeed += ability.movementSpeed;
    //    this.bosterSpeed += ability.bosterSpeed;
    //    this.defance += ability.defance;
    //    this.drillDamage += ability.drillDamage;
    //    this.airCapacity += ability.airCapacity;
    //}

}
