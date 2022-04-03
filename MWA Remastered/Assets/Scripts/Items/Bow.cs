using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Item, IUsableItem, IOnRotateItem
{
    public bool requireItem, pulling;
    public InventoryObject inv;
    public ItemObject arrowItem;
    public GameObject displayArrow, bulletArrow;
    Animator an;

    private void Awake()
    {
        an = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (pulling && CanUse())
        {
            an.SetBool("pulling", true);
            displayArrow.SetActive(true);
        }
        else
        {
            an.SetBool("pulling", false);
            displayArrow.SetActive(false);
        }
        pulling = false;
    }
    public void OnRotate()
    {
        pulling = true;
    }

    public void OnUse()
    {
        if (!CanUse()) return;
        GameObject _arrowBullet = Instantiate(bulletArrow, displayArrow.transform.position, displayArrow.transform.rotation);
        _arrowBullet.GetComponent<Arrow>().owner = owner;
        useSound.Play();
        if (requireItem) inv.RemoveItem(arrowItem, 1);
    }

    public bool CanUse()
    {
        if (!requireItem) return true;
        if (inv.HasItem(arrowItem, 1)) return true;
        return false;
    }
}
