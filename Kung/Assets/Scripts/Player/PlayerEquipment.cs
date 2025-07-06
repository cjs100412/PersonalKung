using TMPro;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public EquipmentData equippedHelmet;
    public EquipmentData equippedBoots;
    public EquipmentData equippedDrill;

    [SerializeField] TextMeshProUGUI drillDamageText;
    [SerializeField] TextMeshProUGUI moveMentSpeedText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI airCapacityText;

    private PlayerStats playerStats;

    private void Awake()
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
                equippedBoots = equipment;
                ApplyEquipment(equippedBoots);
                break;
            case EquipmentData.EquipmentType.Drill:
                equippedDrill = equipment;
                ApplyEquipment(equippedDrill);
                break;
        }

    }

    public void ApplyEquipment(EquipmentData data)
    {
        if (data == null) return;

        switch (data.equipmentType)
        {
            case EquipmentData.EquipmentType.None:
                break;
            case EquipmentData.EquipmentType.Helmet:
                playerStats.defense = data.defance;
                playerStats.airCapacity = data.airCapacity;
                defenseText.text = playerStats.defense.ToString();
                airCapacityText.text = playerStats.airCapacity.ToString(); 

                break;
            case EquipmentData.EquipmentType.Boots:
                playerStats.boosterSpeed = data.bosterSpeed;
                playerStats.movementSpeed = data.movementSpeed;
                moveMentSpeedText.text = playerStats.boosterSpeed.ToString();

                break;
            case EquipmentData.EquipmentType.Drill:
                playerStats.drillDamage = data.drillDamage;
                drillDamageText.text = playerStats.drillDamage.ToString(); 
                break;
            default:
                break;
        }

    }
}
