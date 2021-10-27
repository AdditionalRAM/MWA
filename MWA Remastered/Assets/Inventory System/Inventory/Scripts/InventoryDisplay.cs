using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public InventoryObject inventory;
    public PlayerItem player;

    public float startX, startY;
    public float xSpaceBetweenItems, ySpaceBetweenItems;
    public int columnCount;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Awake()
    {
        CreateDisplay();
        inventory.invDisplay = this;
    }
    private void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.container.Count; i++)
        {
            InventorySlot slot = inventory.container[i];
            if (itemsDisplayed.ContainsKey(slot))
            {
                itemsDisplayed[slot].GetComponentInChildren<Text>().text = slot.amount.ToString("n0");
            }
            else
            {
                GameObject obj = Instantiate(slot.item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                obj.GetComponentInChildren<Text>().text = slot.amount.ToString("n0");
                obj.GetComponent<InventoryButtons>().invDisplay = this;
                obj.GetComponent<InventoryButtons>().myItem = slot.item;
                obj.GetComponent<InventoryButtons>().val = i;
                itemsDisplayed.Add(slot, obj);
            }
        }
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.container.Count; i++)
        {
            InventorySlot slot = inventory.container[i];
            GameObject obj = Instantiate(slot.item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<Text>().text = slot.amount.ToString("n0");
            obj.GetComponent<InventoryButtons>().invDisplay = this;
            obj.GetComponent<InventoryButtons>().myItem = slot.item;
            obj.GetComponent<InventoryButtons>().val = i;
            obj.GetComponent<InventoryButtons>().Innit(this, slot.item, i);
            itemsDisplayed.Add(slot, obj);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(startX + xSpaceBetweenItems * (i % columnCount), startY + ySpaceBetweenItems * (i / columnCount));
    }

    public void Consume(int val)
    {
        if(inventory.container[val].item.type == ItemType.Consumable)
        {
            InventorySlot slot = inventory.container[val];
            if(slot.item.consumableItem != null)
            {
                if (player.Healable())
                {
                    player.Heal(slot.item.consumableItem.restoredHealth);

                    inventory.RemoveItem(slot.item, 1);
                }
            }
        }
    }

    public void Equip(EquipmentObject itemToEquip)
    {
        player.EquipItem(itemToEquip.equipPrefab);
    }

    public void Unequip()
    {
        foreach (GameObject butt in itemsDisplayed.Values)
        {
            if(butt.GetComponent<Button>() != null)
            butt.GetComponent<Button>().interactable = true;
        }
        player.UnequipItem();
    }

    public void Rerender()
    {
        foreach (GameObject destroyObject in itemsDisplayed.Values)
        {
            Destroy(destroyObject);
        }
        itemsDisplayed.Clear();
        CreateDisplay();
    }
}
