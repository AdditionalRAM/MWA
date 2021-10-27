using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Item", menuName = "Inventory/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    public float damage, range, kbThrust, kbTime, rotateOffset;
    public GameObject equipPrefab;
    public bool equipped;
    private void Awake()
    {
        type = ItemType.Equipment;
        equipItem = this;
    }
}
