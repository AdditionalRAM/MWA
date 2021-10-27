using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Key Item", menuName = "Inventory/Items/Key")]
public class KeyObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Key;
        keyItem = this;
    }
}
