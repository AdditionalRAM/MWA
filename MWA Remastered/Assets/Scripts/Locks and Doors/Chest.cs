using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Animator an;
    UIReferences ui;

    public int chestID;

    public GameObject itemPrefab;
    public Vector3 itemPosition;

    public bool locked, playerInRange, open;

    private void Awake()
    {
        an = GetComponent<Animator>();
        ui = FindObjectOfType<UIReferences>();
        OnAfterGameLoad();
    }

    private void Update()
    {
        if(playerInRange && !open)
        {
            ui.interactButton.gameObject.SetActive(true);
        }
        if(!open && playerInRange && ui.interactButton.pressed)
        {
            ui.interactButton.pressed = false;
            ui.interactButton.gameObject.SetActive(false);
            Instantiate(itemPrefab, transform.position + itemPosition, Quaternion.identity);
            open = true;
            an.SetBool("open", open);
            SaveGame.chestsOpened[chestID] = open;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.isTrigger)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            playerInRange = false;
            ui.interactButton.gameObject.SetActive(false);
        }
    }

    public void OnAfterGameLoad()
    {
        open = SaveGame.chestsOpened[chestID];
        an.SetBool("open", open);
    }
}
