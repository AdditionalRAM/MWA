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
            if (invenDisplay != null)
                invenDisplay.Rerender();
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
