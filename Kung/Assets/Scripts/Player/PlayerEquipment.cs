using TMPro;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [HideInInspector] public EquipmentData equippedHelmet;
    [HideInInspector] public EquipmentData equippedBoots;
    public EquipmentData equippedDrill;

    [SerializeField] TextMeshProUGUI drillDamageText;
    [SerializeField] TextMeshProUGUI moveMentSpeedText;
    [SerializeField] TextMeshProUGUI defenseText;
    [SerializeField] TextMeshProUGUI airCapacityText;

    [SerializeField] Animator headAnimator;
    [SerializeField] Animator bodyAnimator;
    [SerializeField] Animator drillDownAnimator;
    [SerializeField] Animator drillMoveAnimator;
    [SerializeField] Animator damageAnimator;

    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        ApplyEquipment(equippedDrill);

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

                AnimatorOverrideController overrideHeadController = new AnimatorOverrideController(headAnimator.runtimeAnimatorController);

                overrideHeadController["HeadIdleAni"] = data.headIdleAni; 
                overrideHeadController["HeadMoveAni"] = data.headMoveAni;
                overrideHeadController["HeadSmileAni"] = data.headSmileAni;
                headAnimator.runtimeAnimatorController = overrideHeadController;

                AnimatorOverrideController overrideHeadDamageController = new AnimatorOverrideController(damageAnimator.runtimeAnimatorController);
                overrideHeadDamageController["DamageAni"] = data.headDamageAni;
                damageAnimator.runtimeAnimatorController = overrideHeadDamageController;

                break;
            case EquipmentData.EquipmentType.Boots:
                playerStats.boosterSpeed = data.bosterSpeed;
                playerStats.movementSpeed = data.movementSpeed;
                moveMentSpeedText.text = playerStats.boosterSpeed.ToString();

                AnimatorOverrideController overrideShoesController = new AnimatorOverrideController(bodyAnimator.runtimeAnimatorController);
                overrideShoesController["BodyIdleAni"] = data.bodyIdleAni;
                overrideShoesController["BodyMoveAni"] = data.bodyMoveAni;
                overrideShoesController["BodyStopAni"] = data.bodyStopAni;
                bodyAnimator.runtimeAnimatorController = overrideShoesController;

                break;
            case EquipmentData.EquipmentType.Drill:
                playerStats.drillDamage = data.drillDamage;
                drillDamageText.text = playerStats.drillDamage.ToString();

                AnimatorOverrideController overrideDrillDownController = new AnimatorOverrideController(drillDownAnimator.runtimeAnimatorController);
                AnimatorOverrideController overrideDrillMoveController = new AnimatorOverrideController(drillMoveAnimator.runtimeAnimatorController);
                overrideDrillDownController["DrillDownAni"] = data.drillDownAni;
                overrideDrillMoveController["DrillMoveAni"] = data.drillMoveAni;
                drillDownAnimator.runtimeAnimatorController = overrideDrillDownController;
                drillMoveAnimator.runtimeAnimatorController = overrideDrillMoveController;

                break;
            default:
                break;
        }

    }
}
