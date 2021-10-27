using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Item", menuName = "Inventory/Items/Default")]
public class DefaultObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Default;
        defItem = this;
    }
}
