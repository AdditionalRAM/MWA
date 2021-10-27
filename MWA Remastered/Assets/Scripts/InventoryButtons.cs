using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtons : MonoBehaviour
{
    public InventoryDisplay invDisplay;
    public ItemObject myItem;
    public Button myButton;
    public int val;

    private void Awake()
    {
        myButton = GetComponent<Button>();
    }

    public void Innit(InventoryDisplay display, ItemObject theItem, int theVal)
    {
        invDisplay = display;
        myItem = theItem;
        val = theVal;
        if (myItem.type == ItemType.Equipment)
        {
            if (invDisplay.player.selectedItem != null && invDisplay.player.selectedItem.myItem == myItem)
                myButton.interactable = false;
        }
    }

    public void OnInteract()
    {
        if(myItem.type == ItemType.Consumable)
        {
            invDisplay.Consume(val);
        }else if (myItem.type == ItemType.Equipment && !myItem.equipItem.equipped)
        {
            if(invDisplay.player.selectedItem == null)
            {
                invDisplay.Equip(myItem.equipItem);
                myButton.interactable = false;
            }
            else
            {
                invDisplay.Unequip();
                invDisplay.Equip(myItem.equipItem);
                myButton.interactable = false;
            }

        }
    }
}
