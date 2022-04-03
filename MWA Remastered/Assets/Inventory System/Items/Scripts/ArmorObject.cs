using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Item", menuName = "Inventory/Items/Armor")]
public class ArmorObject : ItemObject
{
    public float extraHealth;
    public int armorID;
    public bool equipped;
    private void Awake()
    {
        type = ItemType.Armor;
        armorItem = this;
    }
}