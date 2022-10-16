using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockedTeleportDoor : NPCDialogue, IUseSaveGame
{
    Animator an;
    bool localized;
    public int doorID;
    public ItemObject key;
    public ItemObject[] noRemoveKey;
    public float sceneTransitionDuration;
    public bool locked = true, keyNoRemove = false;
    public bool enterOnChoice, animate, requireKey, replaceDialog;
    public Vector3 teleportPosition;
    public string[] lockedDialog, goThroughDialog, forgorDialog;

    public UnityEvent onUnlock, onEnter;

    public void SussyLocalize()
    {
        if (localized) return;
        lockedDialog = LocalManager.LocalizeArray(lockedDialog);
        goThroughDialog = LocalManager.LocalizeArray(goThroughDialog);
        forgorDialog = LocalManager.LocalizeArray(forgorDialog);
        localized = true;
    }

    public void OnAfterGameLoad()
    {
        locked = !SaveGame.doorsUnlocked[doorID];
        CheckLock();
    }

    void CheckLock()
    {
        //OnAwake();
        if (!locked) currentlySayingDialogs = goThroughDialog;
        else currentlySayingDialogs = lockedDialog;
        if (animate && an != null) an.SetBool("open", !locked);
    }

    void SetVarsToSaveGame()
    {
        SaveGame.doorsUnlocked[doorID] = !locked;
    }

    public void OnAwake()
    {
        if(animate)an = GetComponent<Animator>();
        onInRangeInteract.AddListener(OnInRangeInteract);
        onLocalize.AddListener(SussyLocalize);
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
        CheckLock();
        if (locked)
        {
            if (requireKey && player.altScript.keyInventory.HasItem(key, 1))
            {
                player.altScript.keyInventory.RemoveItem(key, 1);
                locked = false;   
                SetVarsToSaveGame();
                CheckLock();
                if (onUnlock != null) onUnlock.Invoke();
            }else if (!requireKey)
            {
                locked = false;
                SetVarsToSaveGame();
                CheckLock();
                if (onUnlock != null) onUnlock.Invoke();
            }
            else
            {
                currentlySayingDialogs = lockedDialog;
            }
        }
        else
        {
            bool playerHasKeys = true;
            foreach (ItemObject iKey in noRemoveKey)
            {
                if(!player.altScript.keyInventory.HasItem(iKey, 1))
                {
                    playerHasKeys = false;
                    break;
                }
            }
            if (keyNoRemove && !playerHasKeys)
            {
                currentlySayingDialogs = forgorDialog;
            }
            else if(!keyNoRemove)
            {
                StartCoroutine(FakeSceneTransition());
            }
            else if(keyNoRemove && playerHasKeys)
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
        if (onEnter != null) onEnter.Invoke();
    }
}

