using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Item", menuName = "Inventory/Items/Consumable")]
public class ConsumableObject : ItemObject
{
    public float restoredHealth;

    private void Awake()
    {
        type = ItemType.Consumable;
        consumableItem = this;
    }
}
