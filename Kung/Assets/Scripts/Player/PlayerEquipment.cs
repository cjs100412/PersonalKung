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

    [SerializeField] Animator headAnimator;
    [SerializeField] Animator bodyAnimator;

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
                equippedBoots = equipment;
                ApplyEquipment(equippedBoots);
                break;
            case EquipmentData.EquipmentType.Drill:
                equippedDrill = equipment;
                ApplyEquipment(equippedDrill);
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
                defenseText.text = playerStats.defense.ToString();
                airCapacityText.text = playerStats.airCapacity.ToString();

                var overrideHeadController = new AnimatorOverrideController(headAnimator.runtimeAnimatorController);

                if (data.headIdleAni != null)
                    overrideHeadController["HeadIdleAni"] = data.headIdleAni; 
                if (data.headMoveAni != null)
                    overrideHeadController["HeadMoveAni"] = data.headMoveAni;
                headAnimator.runtimeAnimatorController = overrideHeadController;

                break;
            case EquipmentData.EquipmentType.Boots:
                playerStats.boosterSpeed = data.bosterSpeed;
                playerStats.movementSpeed = data.movementSpeed;
                moveMentSpeedText.text = playerStats.boosterSpeed.ToString();

                var overrideShoesController = new AnimatorOverrideController(bodyAnimator.runtimeAnimatorController);

                if (data.bobyIdleAni != null)
                    overrideShoesController["BodyIdleAni"] = data.bobyIdleAni;
                if (data.bodyMoveAni != null)
                    overrideShoesController["BodyMoveAni"] = data.bodyMoveAni;

                bodyAnimator.runtimeAnimatorController = overrideShoesController;

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
