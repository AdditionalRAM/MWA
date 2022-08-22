using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTabManager : MonoBehaviour
{
    UIReferences ui;
    Animator an;
    public Animator buttAN;
    public GameObject currentInfoTab;
    public GameObject[] itemMenus;
    public Button[] menuButtons;
    public InventoryObject[] inventories;

    private void Awake()
    {
        foreach (InventoryObject hiyar in inventories)
        {
            hiyar.SussyDeserialize();
        }
    }

    private void Start()
    {
        ui = FindObjectOfType<UIReferences>();
        an = GetComponent<Animator>();
        SwitchTab(0);
    }

    public void AnimateOpening()
    {
        an.SetBool("visible", true);
        buttAN.SetBool("visible", true);
    }

    public void AnimateClosing()
    {
        an.SetBool("visible", false);
        buttAN.SetBool("visible", false);
        if (currentInfoTab != null) currentInfoTab.GetComponent<ItemInfoTab>().DisableTab();
    }

    public void SwitchTab(int i)
    {
        foreach(GameObject itemMenu in itemMenus) itemMenu.SetActive(false);
        foreach(Button menuButton in menuButtons) menuButton.interactable = true;
        menuButtons[i].interactable = false;
        itemMenus[i].SetActive(true);
        if (currentInfoTab != null) currentInfoTab.GetComponent<ItemInfoTab>().DisableTab();
    }

    public void SaveInventories()
    {
        foreach (GameObject invD in itemMenus)
        {
            invD.GetComponent<InventoryDisplay>().inventory.Save();
        }
    }

    public void LoadInventories()
    {
        foreach (GameObject invD in itemMenus)
        {
            InventoryDisplay inventar = invD.GetComponent<InventoryDisplay>();
            inventar.inventory.Load(inventar);
        }
    }

    public void EraseInventories()
    {
        foreach (GameObject invD in itemMenus)
        {
            invD.GetComponent<InventoryDisplay>().inventory.Erase();
        }
    }

}
