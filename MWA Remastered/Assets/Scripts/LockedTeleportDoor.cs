using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedTeleportDoor : NPCDialogue
{
    Animator an;
    public ItemObject key;
    public ItemObject noRemoveKey;
    public float sceneTransitionDuration;
    public bool locked = true, keyNoRemove = false;
    public bool enterOnChoice, animate, requireKey, replaceDialog;
    public Vector3 teleportPosition;
    public string[] lockedDialog, goThroughDialog, forgorDialog;

    /*private void Update()
    {
        if (playerInRange && ui.interactButton.pressed)
        {
            ui.interactButton.pressed = false;
            if (locked)
            {
                if(player.keyInventory.HasItem(key, 1))
                {
                    player.keyInventory.RemoveItem(key, 1);
                    locked = false;
                    an.SetBool("open", !locked);
                    dialog.currentlySayingDialogs = goThroughDialog;
                    dialog.DialogInteract();
                }
                else
                {
                    dialog.currentlySayingDialogs = lockedDialog;
                    dialog.DialogInteract();
                }
            }
            else
            {
                StartCoroutine(FakeSceneTransition());
                player.transform.position = teleportPosition;
            }
        }
    }*/

    public void OnAwake()
    {
        if(animate)an = GetComponent<Animator>();
        onInRangeInteract.AddListener(OnInRangeInteract);
        if(enterOnChoice) onOption1End.AddListener(OnOption2End);
    }

    public void OnInRangeInteract()
    {
        if (!enterOnChoice) AttemptUnlock();
    }

    public void OnOption2End()
    {
        if (enterOnChoice)
        {
            enterOnChoice = false;
            AttemptUnlock();
        }
    }

    void AttemptUnlock()
    {
        if (locked)
        {
            if (requireKey && player.altScript.keyInventory.HasItem(key, 1))
            {
                player.altScript.keyInventory.RemoveItem(key, 1);
                locked = false;
                if (animate) an.SetBool("open", !locked);
                currentlySayingDialogs = goThroughDialog;
            }else if (!requireKey)
            {
                locked = false;
                if (animate) an.SetBool("open", !locked);
                currentlySayingDialogs = goThroughDialog;
            }
            else
            {
                currentlySayingDialogs = lockedDialog;
            }
        }
        else
        {
            if (keyNoRemove && !player.altScript.keyInventory.HasItem(noRemoveKey, 1))
            {
                currentlySayingDialogs = forgorDialog;
            }
            else if(!keyNoRemove)
            {
                StartCoroutine(FakeSceneTransition());
            }
            else if(keyNoRemove && player.altScript.keyInventory.HasItem(noRemoveKey, 1))
            {
                StartCoroutine(FakeSceneTransition());
            }
        }
    }

    IEnumerator FakeSceneTransition()
    {
        ui.transitionBlocker.SetActive(true);
        player.transform.position = teleportPosition;
        //Debug.Log(sceneTransitionDuration);
        Invoke("DisableTransitionBlocker", sceneTransitionDuration);
        yield return new WaitForSeconds(sceneTransitionDuration);
        //yield return null;
        Debug.Log("after yield");
        ui.transitionBlocker.SetActive(false);
    }

    void DisableTransitionBlocker()
    {
        ui.transitionBlocker.SetActive(false);
    }
}

