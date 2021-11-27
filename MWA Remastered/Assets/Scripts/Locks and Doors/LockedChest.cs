using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedChest : NPCDialogue
{
    Animator an;
    public int chestID;
    public ItemObject key;

    public GameObject itemPrefab;
    public Vector3 itemPosition;

    public string[] afterUnlock;
    public bool open;
    
    public void SussyLocalize()
    {
        if (!autoLocalize) return;
        afterUnlock = LocalManager.LocalizeArray(afterUnlock);
    }

    public void OnAwake()
    {
        an = GetComponent<Animator>();
        onInRangeInteract.AddListener(OnInRangeInteract);
        OnAfterGameLoad();
        onLocalize.AddListener(SussyLocalize);
    }

    public void OnInRangeInteract()
    {
        if (open) currentlySayingDialogs = afterUnlock;
        if (!open && player.altScript.keyInventory.HasItem(key, 1))
        {
            player.altScript.keyInventory.RemoveItem(key, 1);
            Instantiate(itemPrefab, transform.position + itemPosition, Quaternion.identity);
            open = true;
            an.SetBool("open", open);
            SaveGame.chestsOpened[chestID] = open;
            currentlySayingDialogs = afterUnlock;
        }
    }

    public void OnAfterGameLoad()
    {
        open = SaveGame.chestsOpened[chestID];
        an.SetBool("open", open);
        if(open) currentlySayingDialogs = afterUnlock;
    }
}
