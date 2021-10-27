using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    Default,
    Equipment,
    Consumable,
    Key
}
public abstract class ItemObject : ScriptableObject
{
    public DefaultObject defItem;
    public ConsumableObject consumableItem;
    public EquipmentObject equipItem;
    public KeyObject keyItem;

    public GameObject prefab;
    public ItemType type;
    [TextArea(10, 15)]
    public string description;
}
