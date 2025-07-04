using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float baseMovementSpeed = 1f;
    public float baseBoosterSpeed = 7f;
    public float baseDefense = 0;
    public float baseDrillDamage = 1f;
    public float baseAirCapacity = 100f;

    public float movementSpeed;
    public float boosterSpeed;
    public float defense;
    public float drillDamage;
    public float airCapacity;

    private void Start()
    {
        ResetStats();
    }

    public void ResetStats()
    {
        movementSpeed = baseMovementSpeed;
        boosterSpeed = baseBoosterSpeed;
        defense = baseDefense;
        drillDamage = baseDrillDamage;
        airCapacity = baseAirCapacity;
    }
}
