using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Inventory")]
public class InventoryObject : ScriptableObject
{
    [SerializeField]
    public List<InventorySlot> container = new List<InventorySlot>();
    public int limit;
    public InventoryDisplay invDisplay;
    InventorySlot currentSlot;
    public void AddItem(ItemObject _item, int _amount)
    {        
        if (!HasItem(_item, _amount))
        {
            container.Add(new InventorySlot(_item, _amount));
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
}

[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
}
