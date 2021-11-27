using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButtons : MonoBehaviour
{
    public bool localize;
    public AudioClip useSound;
    Image myImg;
    public InventoryDisplay invDisplay;
    public ItemObject myItem;
    public Button myButton;
    public int val;
    public bool usable, canUse;
    public GameObject infotabPrefab;
    public string itemName, useMSG, itemStat1, itemStat2, itemStat3;
    public float statVal1, statVal2, statVal3;
    public Color regularColor, imColor;

    private void Awake()
    {
        myButton = GetComponent<Button>();
        canUse = usable;
        myImg = GetComponent<Image>();
        if (localize) Localize();
    }

    public void Innit(InventoryDisplay display, ItemObject theItem, int theVal)
    {
        invDisplay = display;
        myItem = theItem;
        val = theVal;
        if (myItem.type == ItemType.Equipment)
        {
            if (invDisplay.player.selectedItem != null && invDisplay.player.selectedItem.myItem == myItem)
            {
                myImg.color = imColor;
                canUse = false;
            }
        }
    }

    public void OnPress()
    {
        if (invDisplay == null)
        {
            Debug.Log(name + ": invDisplay is null.");
            return;
        }
        if (invDisplay.myTabs.currentInfoTab != null) invDisplay.myTabs.currentInfoTab.GetComponent<ItemInfoTab>().DisableTab();
        invDisplay.myTabs.currentInfoTab = Instantiate(infotabPrefab, invDisplay.transform.parent);
        ItemInfoTab infoTab = invDisplay.myTabs.currentInfoTab.GetComponent<ItemInfoTab>();
        Vector3 windowOffset = Vector3.zero;
        if (Mathf.Sign(transform.position.y) == 1) windowOffset.y = Screen.height / -6.75f;
        else windowOffset.y = Screen.height / 6.75f;
        windowOffset.x = (Screen.height / 9) * 16 / -8.53333333333f;
        infoTab.transform.position = transform.position + windowOffset;
        infoTab.objectName.text = itemName;
        infoTab.stat1.text = itemStat1;
        infoTab.stat2.text = itemStat2;
        infoTab.stat3.text = itemStat3;
        infoTab.useMSG.text = useMSG;
        if (canUse)
        {
            infoTab.useButton.onClick.AddListener(OnInteract);
        }
        else Destroy(infoTab.useButton.transform.parent.gameObject);
    }

    public void OnInteract()
    {
        if (invDisplay.myTabs.currentInfoTab != null) invDisplay.myTabs.currentInfoTab.GetComponent<ItemInfoTab>().DisableTab();
        if (myItem.type == ItemType.Consumable)
        {
            if (useSound != null) invDisplay.Consume(val, useSound);
            else invDisplay.Consume(val, null);
        }
        else if (myItem.type == ItemType.Equipment && !myItem.equipItem.equipped)
        {
            if(invDisplay.player.selectedItem == null)
            {
                invDisplay.Equip(myItem.equipItem);
                myImg.color = imColor;
                canUse = false;
            }
            else
            {
                invDisplay.Unequip();
                invDisplay.Equip(myItem.equipItem);
                myImg.color = imColor;
                canUse = false;
            }

        }
    }

    public void Revert()
    {
        canUse = true;
        myImg.color = regularColor;
        if (invDisplay.myTabs.currentInfoTab != null)
        {
            ItemInfoTab infoTab = invDisplay.myTabs.currentInfoTab.GetComponent<ItemInfoTab>();
            Destroy(infoTab.useButton.transform.parent.gameObject);
        }
    }

    void Localize()
    {
        itemName = LocalManager.Localize(itemName);
        useMSG = LocalManager.Localize(useMSG);
        itemStat1 = LocalManager.Localize(itemStat1);
        itemStat2 = LocalManager.Localize(itemStat2);
        itemStat3 = LocalManager.Localize(itemStat3);
        if (itemStat1 != "") itemStat1 += statVal1;
        if (itemStat2 != "") itemStat2 += statVal2;
        if (itemStat3 != "") itemStat3 += statVal3;
    }
}
