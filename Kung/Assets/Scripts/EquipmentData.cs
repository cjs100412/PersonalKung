using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentData", menuName = "Scriptable Objects/EquipmentData")]
public class EquipmentData : ScriptableObject
{
    public enum EquipmentType
    {
        None,
        Helmet,
        Boots,
        Drill
    }
    public EquipmentType equipmentType = EquipmentType.None;
    public int itemId;
    public float movementSpeed = 0;
    public float bosterSpeed = 0;
    public float defance = 0;
    public float drillDamage = 0;
    public float airCapacity = 0;

}
