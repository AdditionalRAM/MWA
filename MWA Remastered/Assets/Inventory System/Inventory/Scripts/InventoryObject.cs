using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemType inventoryType;
    public int equippedID;

    public string savePath, fileName;
    public List<InventorySlot> container = new List<InventorySlot>();
    public int limit;
    [NonSerialized] public InventoryDisplay invDisplay;
    private ItemDatabaseObject database;
    InventorySlot currentSlot;

    private void OnEnable()
    {
        #if UNITY_EDITOR
            database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
#else
            database = Resources.Load<ItemDatabaseObject>("Database");
#endif
    }

    public void AddItem(ItemObject _item, int _amount)
    {        
        if (!HasItem(_item, 1))
        {
            container.Add(new InventorySlot(database.GetId[_item], _item, _amount));
        }
        else
        {
            currentSlot.AddAmount(_amount);
        }
    }

    public void RemoveItem(ItemObject _item, int _amount)
    {
        if(HasItem(_item, _amount))
        {
            currentSlot.amount-= _amount;
            if (currentSlot.amount <= 0)
            {
                container.Remove(currentSlot);
                if (invDisplay == null)
                {
                    Debug.Log("invDisplay is null!  -" + name); return;
                }
                invDisplay.Rerender();
            }
        }
    }

    public bool HasItem(ItemObject __item, int __amount)
    {
        foreach (InventorySlot slot in container.ToArray())
        {
            if (slot.item == __item && slot.amount >= __amount)
            {
                currentSlot = slot; 
                return true;
            }
        }
        return false;
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        
    }

    public void SussyDeserialize()
    {
        database.DictionaryInit();
        for (int i = 0; i < container.Count; i++)
        {
            ItemObject sussy;
            if (!database.GetItem.TryGetValue(i, out sussy))
            {
                Debug.LogError("Error from " + fileName + ": database.GetItem doesn't contain value " + i);
            }
            else
            {
                container[i].item = database.GetItem[container[i].ID];
            }
        }
    }


    public void Save()
    {
        if(!Directory.Exists(Application.persistentDataPath + savePath))
        {
            Directory.CreateDirectory(Application.persistentDataPath + savePath);
        }
        string nowSavePath = Application.persistentDataPath + savePath + fileName;
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = File.Create(nowSavePath);
        bf.Serialize(stream, saveData);
        stream.Close();
    }

    public void Load(InventoryDisplay invenDisplay)
    {
        string nowSavePath = Application.persistentDataPath + savePath + fileName;
        if (File.Exists(nowSavePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(nowSavePath, FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(stream).ToString(), this);
            stream.Close();
            if (inventoryType == ItemType.Armor && !HasItem(database.emptyArmor, 1))
            {
                AddItem(database.emptyArmor, 1);
            }
            else if (inventoryType == ItemType.Equipment && !HasItem(database.emptyEquipment, 1))
            {
                AddItem(database.emptyEquipment, 1);
            }
            if (invenDisplay != null) {
                invenDisplay.Rerender(); 
                if(inventoryType == ItemType.Equipment)
                {
                    if(equippedID != database.GetId[database.emptyEquipment] && database.GetItem[equippedID].type == ItemType.Equipment)
                    {
                        invenDisplay.Equip(database.GetItem[equippedID].equipItem);
                    }
                    else
                    {
                        invenDisplay.Equip(database.emptyEquipment.equipItem);
                    }
                }
                if (inventoryType == ItemType.Armor)
                {
                    if (equippedID != database.GetId[database.emptyArmor] && database.GetItem[equippedID].type == ItemType.Armor)
                    {
                        invenDisplay.EquipArmor(database.GetItem[equippedID].armorItem);
                    }
                    else
                    {
                        invenDisplay.EquipArmor(database.emptyArmor.armorItem);
                    }
                }
            }
        }
        else
        {
            if (inventoryType == ItemType.Armor && !HasItem(database.emptyArmor, 1))
            {
                AddItem(database.emptyArmor, 1);
                invenDisplay.EquipArmor(database.emptyArmor);
            }
            else if (inventoryType == ItemType.Equipment && !HasItem(database.emptyEquipment, 1))
            {
                AddItem(database.emptyEquipment, 1);
                invenDisplay.Equip(database.emptyEquipment);
            }
        }
    }

    public void Erase()
    {
        string nowSavePath = Application.persistentDataPath + savePath + fileName;
        if (File.Exists(nowSavePath))
        {
            File.Delete(nowSavePath);
        }
    }

    public void iEquippedItem(ItemObject item)
    {
        equippedID = database.GetId[item];
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemObject item;
    public int amount;
    public InventorySlot(int id, ItemObject _item, int _amount)
    {
        ID = id;
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}
