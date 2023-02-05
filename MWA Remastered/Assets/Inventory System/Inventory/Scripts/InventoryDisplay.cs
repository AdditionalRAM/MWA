using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public InventoryTabManager myTabs;
    public InventoryObject inventory;
    public PlayerItem player;
    public AudioSource useAudio;

    public float startX, startY;
    public float xSpaceBetweenItems, ySpaceBetweenItems;
    public int columnCount;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    private void Start()
    {
        myTabs = transform.parent.GetComponent<InventoryTabManager>();
        Rerender();
        inventory.invDisplay = this;
    }

    private void OnEnable()
    {
        Rerender();
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
                obj.GetComponent<InventoryButtons>().Innit(this, slot.item, i);
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
            obj.GetComponent<InventoryButtons>().Innit(this, slot.item, i);
            itemsDisplayed.Add(slot, obj);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(startX + xSpaceBetweenItems * (i % columnCount), startY + ySpaceBetweenItems * (i / columnCount));
    }

    public void Consume(int val, AudioClip useSound)
    {
        if(inventory.container[val].item.type == ItemType.Consumable)
        {
            InventorySlot slot = inventory.container[val];
            if(slot.item.consumableItem != null)
            {
                if (player.Healable())
                {
                    if(useSound != null && useAudio != null)
                    {
                        useAudio.clip = useSound;
                        useAudio.Play();
                    }
                    player.Heal(slot.item.consumableItem.restoredHealth);
                    inventory.RemoveItem(slot.item, 1);
                }
            }
        }
    }

    public void Equip(EquipmentObject itemToEquip)
    {
        if (player == null) Debug.Log("Need backup coroutunio");
        inventory.iEquippedItem(itemToEquip);
        player.EquipItem(itemToEquip.equipPrefab);
        useAudio.Play();
    }

    public void Unequip()
    {
        foreach (GameObject butt in itemsDisplayed.Values)
        {
            if (butt.GetComponent<Button>() != null)
                butt.GetComponent<InventoryButtons>().Revert();
        }
        player.UnequipItem();
    }

    public void EquipArmor(ArmorObject armorToEquip)
    {
        player.EquipArmor(armorToEquip);
        inventory.iEquippedItem(armorToEquip);
        useAudio.Play();
    }

    public void UnequipArmor()
    {
        foreach (GameObject butt in itemsDisplayed.Values)
        {
            if (butt.GetComponent<Button>() != null)
                butt.GetComponent<InventoryButtons>().Revert();
        }
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
