using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Database", menuName = "Inventory/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] items;
    public EquipmentObject emptyEquipment;
    public ArmorObject emptyArmor;
    public Dictionary<ItemObject, int> GetId = new Dictionary<ItemObject, int>();
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    public void OnAfterDeserialize()
    {
        //GetId = new Dictionary<ItemObject, int>();
        GetId.Clear();
        //GetItem = new Dictionary<int, ItemObject>();
        GetItem.Clear();
        for (int i = 0; i < items.Length; i++)
        {
            GetId.Add(items[i], i);
            GetItem.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        //throw new System.NotImplementedException();
    }
}
