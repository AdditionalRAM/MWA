using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    UIReferences ui;
    SaveUI saveUI;
    PlayerMovement currentPlayer;
    public bool playerInRange, playerFrozen;

    private void Awake()
    {
        ui = FindObjectOfType<UIReferences>();
        saveUI = FindObjectOfType<SaveUI>();
    }
    private void Update()
    {
        if (ui.interactButton.pressed && playerInRange)
        {
            ui.interactButton.pressed = false;
            StartCoroutine(Interact());
        }
        else if(saveUI.transform.localScale == Vector3.zero && playerInRange && !playerFrozen)
        {
            currentPlayer.freeze = false;
            currentPlayer.iFrames = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayer = other.GetComponent<PlayerMovement>();
            playerInRange = true;
            ui.interactButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ui.interactButton.gameObject.SetActive(false);
            saveUI.DisableSaveMenu();
            currentPlayer.freeze = false;
            currentPlayer.iFrames = false;
        }
    }

    IEnumerator Interact()
    {
        currentPlayer.freeze = true;
        playerFrozen = true;
        saveUI.EnableSaveMenu();
        if (currentPlayer.altScript.Healable())
        {
            currentPlayer.altScript.Heal(10);
            currentPlayer.altScript.healSound.Play();
        }
        yield return new WaitForSeconds(.25f);
        playerFrozen = false;
    }
}
