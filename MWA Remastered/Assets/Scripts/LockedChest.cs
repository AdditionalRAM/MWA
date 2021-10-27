using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedChest : NPCDialogue
{
    Animator an;
    public ItemObject key;

    public GameObject itemPrefab;
    public Vector3 itemPosition;

    public string[] afterUnlock;
    public bool open;

    public void OnAwake()
    {
        an = GetComponent<Animator>();
        onInRangeInteract.AddListener(OnInRangeInteract);
    }

    public void OnInRangeInteract()
    {
        if (!open && player.altScript.keyInventory.HasItem(key, 1))
        {
            player.altScript.keyInventory.RemoveItem(key, 1);
            an.SetBool("open", true);
            Instantiate(itemPrefab, transform.position + itemPosition, Quaternion.identity);
            open = true;
            currentlySayingDialogs = afterUnlock;
        }
    }

}
