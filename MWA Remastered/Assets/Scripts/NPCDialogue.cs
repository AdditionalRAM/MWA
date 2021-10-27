using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NPCDialogue : MonoBehaviour
{
    [HideInInspector]
    public UIReferences ui;

    [HideInInspector]
    public AudioSource dialogSound;
    [HideInInspector]
    public PlayerMovement player;

    public float textDelay;

    public bool makeSound, optionChoice, canInteract = false, inDialog = false, waitingForOption = false, canOption;

    public bool controlledFromOutside = false;

    public string nameToDisplay, option1, option2;
    public string[] dialogs, dialogsOption1, dialogsOption2;
    public string[] currentlySayingDialogs;

    bool tempFinish = false, saidMsg = false;
    int currentDialog = 0;

    public UnityEvent onDialogStart, onDialogEnd, 
        onOption1Select, onOption2Select, onOption1End, onOption2End, onPlayerEnterRange, onPlayerExitRange, onInRangeInteract, onAwake;

    private UnityEvent onUpdate;

    public void Awake()
    {
        if (controlledFromOutside && onAwake != null) onAwake.Invoke();
        dialogSound = GetComponent<AudioSource>();
        ui = FindObjectOfType<UIReferences>();
        currentlySayingDialogs = dialogs;
        canOption = optionChoice;
    }

    public void DialogInteract()
    {
        if (!inDialog)
        {
            if (controlledFromOutside && onDialogStart != null) onDialogStart.Invoke();
            canOption = optionChoice;
            if (optionChoice) currentlySayingDialogs = dialogs;
            player.freeze = true;
            ui.dialogOptions.SetActive(false);
            ui.dialogText.gameObject.SetActive(true);
            currentDialog = 0;
            inDialog = true;
            ui.dialogBox.SetActive(true);
            ui.dialogName.text = nameToDisplay;
            ui.dialogText.text = "";
            StartCoroutine(Dialog());
        }
        else if (inDialog && currentDialog + 1 < currentlySayingDialogs.Length && saidMsg)
        {
            currentDialog++;
            ui.dialogBox.SetActive(true);
            ui.dialogName.text = nameToDisplay;
            ui.dialogText.text = "";
            StartCoroutine(Dialog());
        }
        else if (inDialog && currentDialog + 1 >= currentlySayingDialogs.Length && saidMsg)
        {
            if (controlledFromOutside && onDialogEnd != null) onDialogEnd.Invoke();
            if (currentlySayingDialogs == dialogsOption1) { if (controlledFromOutside && onOption1End != null) onOption1End.Invoke(); }
            if (currentlySayingDialogs == dialogsOption2) { if (controlledFromOutside && onOption2End != null) onOption2End.Invoke(); }
            if (!canOption)
            {
                inDialog = false;
                ui.dialogBox.SetActive(false);
                currentDialog = 0;
                StopAllCoroutines();
                player.freeze = false;
                player.iFrames = false;
                player.takenKB = false;
            }
            else
            {
                ui.dialogText.gameObject.SetActive(false);
                ui.dialogOptions.SetActive(true);
                ui.opt1Text.text = option1;
                ui.opt2Text.text = option2;
                canOption = false;
                waitingForOption = true;
                ui.interactButton.gameObject.SetActive(false);
            }
        }
        else if(inDialog && canInteract && !saidMsg)
        {
            tempFinish = true;
        }
        
    }        
            

    private void Update()
    {
        if (controlledFromOutside && onUpdate != null) onUpdate.Invoke();
        if (!waitingForOption)
        {
            if (ui.interactButton.pressed && canInteract)
            {
                if (controlledFromOutside && onInRangeInteract != null) onInRangeInteract.Invoke();
                ui.interactButton.pressed = false;
                DialogInteract();
            }
        }
        else
        {
            if (ui.dialogOption1.pressed)
            {
                if (controlledFromOutside && onOption1Select != null) onOption1Select.Invoke();
                ui.dialogOption1.pressed = false;
                ui.interactButton.gameObject.SetActive(true);
                currentDialog = 0;
                currentlySayingDialogs = dialogsOption1;
                ui.dialogText.text = "";
                ui.dialogText.gameObject.SetActive(true);
                ui.dialogOptions.SetActive(false);
                waitingForOption = false;
                StartCoroutine(Dialog());
            }
            else if (ui.dialogOption2.pressed)
            {
                if (controlledFromOutside && onOption2Select != null) onOption2Select.Invoke();
                ui.dialogOption2.pressed = false;
                ui.interactButton.gameObject.SetActive(true);
                currentDialog = 0;
                currentlySayingDialogs = dialogsOption2;
                ui.dialogText.text = "";
                ui.dialogText.gameObject.SetActive(true);
                ui.dialogOptions.SetActive(false);
                waitingForOption = false;
                StartCoroutine(Dialog());
            }
        }    
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.isTrigger && !inDialog)
        {
            if (controlledFromOutside && onPlayerEnterRange != null) onPlayerEnterRange.Invoke();
            canInteract = true;
            ui.interactButton.gameObject.SetActive(true);
            player = other.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            SusOut();
        }
    }

    IEnumerator Dialog()
    {
        saidMsg = false;
        foreach (char character in currentlySayingDialogs[currentDialog])
        {
            if (tempFinish) break;
            if (makeSound && character != ' ')
            {
                dialogSound.Play();
            }
            yield return new WaitForSeconds(textDelay);
            ui.dialogText.text += character;
        }
        tempFinish = false;
        ui.dialogText.text = currentlySayingDialogs[currentDialog];
        saidMsg = true;
    }

    public void SusOut()
    {
        if (controlledFromOutside && onPlayerExitRange != null) onPlayerExitRange.Invoke();
        canInteract = false;
        ui.interactButton.gameObject.SetActive(false);
        if (inDialog)
        {
            player.freeze = false;
            player.iFrames = false;
            ui.dialogBox.SetActive(false);
            inDialog = false;
            currentDialog = 0;
            StopAllCoroutines();
        }
    }
}
