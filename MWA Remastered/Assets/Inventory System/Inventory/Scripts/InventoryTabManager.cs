using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTabManager : MonoBehaviour
{
    Animator an;
    public GameObject[] itemMenus;
    public Button[] menuButtons;

    private void Awake()
    {
        an = GetComponent<Animator>();
        SwitchTab(0);
    }

    public void AnimateOpening()
    {
        an.SetBool("visible", true);
    }

    public void AnimateClosing()
    {
        an.SetBool("visible", false);
    }

    public void SwitchTab(int i)
    {
        foreach(GameObject itemMenu in itemMenus) itemMenu.SetActive(false);
        foreach(Button menuButton in menuButtons) menuButton.interactable = true;
        menuButtons[i].interactable = false;
        itemMenus[i].SetActive(true);
    }

}
