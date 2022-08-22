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
    public bool inited = false;


    public void DictionaryInit()
    {
        if (inited) return;
        GetId = new Dictionary<ItemObject, int>();
        GetItem = new Dictionary<int, ItemObject>();
        for (int i = 0; i < items.Length; i++)
        {
            if (!GetId.ContainsKey(items[i])) GetId.Add(items[i], i);
            if (!GetItem.ContainsKey(i)) GetItem.Add(i, items[i]);
        }
        inited = true;
    }

    public void OnAfterDeserialize()
    {
        //DictionaryInit();
    }

    public void OnBeforeSerialize()
    {
        //throw new System.NotImplementedException();
    }

    private void OnDisable()
    {
        inited = false;
    }
}
