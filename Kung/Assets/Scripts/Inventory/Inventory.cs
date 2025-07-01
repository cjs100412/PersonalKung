using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<UserInventoryItem> items;

    [SerializeField] private InventorySlotUI[] slots = new InventorySlotUI[25];
    [SerializeField] private MineralTile mineralTile;
    public bool isFull;
    private void Awake()
    {
        mineralTile.OnOreCollected += HandleOreCollected;
    }

    private void HandleOreCollected(CustomOreTile oreTile)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].isEmpty && slots[i].oreType == oreTile.oreType )
            {
                if (slots[i].count < 5)
                {
                    slots[i].count++;
                    slots[i].itemCountText.text = slots[i].count.ToString();
                    return;
                }
            }
        }

        for(int i = 0;i < slots.Length; i++)
        {
            if (slots[i].isEmpty)
            {
                slots[i].oreType = oreTile.oreType;
                slots[i].itemIcon.sprite = oreTile.sprite;
                slots[i].count++;
                slots[i].itemCountText.text = slots[i].count.ToString();
                slots[i].isEmpty = false;
                return;
            }
        }
        isFull = true;
    }
}
