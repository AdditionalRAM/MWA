using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour, ILocalization
{
    public bool localize;
    public ItemObject item;
    public int itemAmount;
    public string itemName;
    public bool obtained = false;

    public void OnLocalize()
    {
        if (!localize) return;
        itemName = LocalManager.Localize(itemName);
        localize = false;
    }

    private void Start()
    {
        if (localize && LocalManager.ReadyToLocalize) {
            itemName = LocalManager.Localize(itemName);
            localize = false;
        }   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerItem>() == null) return;
        if (!obtained)
        {
            other.GetComponent<PlayerItem>().GrabItem(item, itemAmount, itemName);
            obtained = true;
            Destroy(gameObject);
        }
    }
}
