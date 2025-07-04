using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public EquipmentData equippedHelmet;
    public EquipmentData equippedBoots;
    public EquipmentData equippedDrill;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

  
    public void EquipItem(EquipmentData equipment)
    {
        switch (equipment.equipmentType)
        {
            case EquipmentData.EquipmentType.Helmet:
                equippedHelmet = equipment;
                ApplyEquipment(equippedHelmet);
                break;
            case EquipmentData.EquipmentType.Boots:
                ApplyEquipment(equippedBoots);
                equippedBoots = equipment;
                break;
            case EquipmentData.EquipmentType.Drill:
                ApplyEquipment(equippedDrill);
                equippedDrill = equipment;
                break;
        }

    }

    private void ApplyEquipment(EquipmentData data)
    {
        if (data == null) return;


        switch (data.equipmentType)
        {
            case EquipmentData.EquipmentType.None:
                break;
            case EquipmentData.EquipmentType.Helmet:
                playerStats.defense = data.defance;
                playerStats.airCapacity = data.airCapacity;
                break;
            case EquipmentData.EquipmentType.Boots:
                playerStats.boosterSpeed = data.bosterSpeed;
                playerStats.movementSpeed = data.movementSpeed;
                break;
            case EquipmentData.EquipmentType.Drill:
                playerStats.drillDamage = data.drillDamage;
                break;
            default:
                break;
        }

    }
}
