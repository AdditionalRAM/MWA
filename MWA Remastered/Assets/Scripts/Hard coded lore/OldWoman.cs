using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OldWoman : NPCDialogue
{
    public Storyteller storyteller;

    public Vector3 chair1Pos, chair2Pos;
    public bool saidDialog1, takenDaisy, toldStory, endedStory;
    public ItemObject daisy;

    public GameObject fuatNPC, fuatFuat;

    public string[] yesDaisyDialog, noDaisyDialog, afterStoryDialog, endedStoryDialog;

    bool playerHasDaisy;

    public void OnAwake()
    {
        onInRangeInteract.AddListener(OnInRangeInteract);
        onOption1End.AddListener(OnOption1End);
        onDialogEnd.AddListener(OnDialogEnd);
    }

    public void OnInRangeInteract()
    {
        if (player.altScript.defInventory.HasItem(daisy, 3)) playerHasDaisy = true;
        else playerHasDaisy = false;
        if(playerHasDaisy && saidDialog1 && !takenDaisy)
        {
            player.altScript.defInventory.RemoveItem(daisy, 3);
            takenDaisy = true;
        }
        if(!toldStory && takenDaisy)
        {
            currentlySayingDialogs = yesDaisyDialog;
        }else if(!toldStory && !playerHasDaisy && saidDialog1)
        {
            currentlySayingDialogs = noDaisyDialog;
        }
        if (toldStory && !endedStory) currentlySayingDialogs = afterStoryDialog;
        else if (endedStory) currentlySayingDialogs = endedStoryDialog;
    }

    public void OnOption1End()
    {
        canOption = false; optionChoice = false;
        saidDialog1 = true;
    }

    public void OnDialogEnd()
    {
        if (currentlySayingDialogs == yesDaisyDialog)
        {
            toldStory = true;
            storyteller.gameObject.SetActive(true);
            storyteller.TellStory();
            fuatNPC.SetActive(false);
            fuatFuat.SetActive(true);
            transform.position = chair1Pos;
            player.transform.position = chair2Pos;
        }
        else if (currentlySayingDialogs == afterStoryDialog) endedStory = true;
    }



    /*UIReferences ui;
    PlayerItem currentPlayer;

    public Storyteller storyteller;
    public Fuat fuat;
    public KnifeGiver knifeGiver;

    public Vector3 chair1Pos, chair2Pos;

    public AudioSource dialogSound;

    public float textDelay;

    public bool makeSound, optionChoice, canInteract = false, inDialog = false, waitingForOption = false;

    public bool saidDialog1, takenDaisy, toldStory, endedStory;

    public string nameToDisplay, option1, option2;
    public string[] dialogs, dialogsOption1, dialogsOption2;
    public string[] currentlySayingDialogs;
    public string[] yesDaisyDialog, noDaisyDialog, afterStoryDialog, endedStoryDialog;

    bool tempFinish = false, saidMsg = false;

    public ItemObject daisy;

    int currentDialog = 0;

    public void Awake()
    {
        ui = FindObjectOfType<UIReferences>();
        currentlySayingDialogs = dialogs;
    }

    private void Update()
    {
        if (!waitingForOption)
        {
            if (ui.interactButton.pressed && canInteract && !inDialog)
            {
                if (currentPlayer.defInventory.HasItem(daisy, 3) && saidDialog1 && !takenDaisy)
                {
                    currentPlayer.defInventory.RemoveItem(daisy, 3);
                    currentlySayingDialogs = yesDaisyDialog;
                    takenDaisy = true;
                }else if (!currentPlayer.defInventory.HasItem(daisy, 3) && !takenDaisy && saidDialog1)
                {
                    currentlySayingDialogs = noDaisyDialog;
                }
                if (toldStory)
                {
                    currentlySayingDialogs = afterStoryDialog;
                }
                if (endedStory)
                {
                    currentlySayingDialogs = endedStoryDialog;
                }
                currentPlayer.altScript.freeze = true;
                ui.interactButton.pressed = false;
                ui.dialogOptions.SetActive(false);
                ui.dialogText.gameObject.SetActive(true);
                currentDialog = 0;
                inDialog = true;
                ui.dialogBox.SetActive(true);
                ui.dialogName.text = nameToDisplay;
                ui.dialogText.text = "";
                StartCoroutine(Dialog());
            }
            else if (ui.interactButton.pressed && canInteract && inDialog && currentDialog + 1 < currentlySayingDialogs.Length && !saidMsg)
            {
                ui.interactButton.pressed = false;
                currentDialog++;
                ui.dialogBox.SetActive(true);
                ui.dialogName.text = nameToDisplay;
                ui.dialogText.text = "";
                ui.interactButton.pressed = false;
                StartCoroutine(Dialog());
            }
            else if (ui.interactButton.pressed && canInteract && inDialog && currentDialog + 1 >= currentlySayingDialogs.Length && !saidMsg)
            {
                ui.interactButton.pressed = false;
                if (currentlySayingDialogs == dialogsOption1)
                {
                    saidDialog1 = true;
                    inDialog = false;
                    ui.dialogBox.SetActive(false);
                    currentDialog = 0;
                    StopAllCoroutines();
                    currentPlayer.altScript.freeze = false;
                    currentPlayer.altScript.iFrames = false;
                }
                else if(currentlySayingDialogs == yesDaisyDialog)
                {
                    storyteller.gameObject.SetActive(true);
                    storyteller.TellStory();
                    transform.position = chair1Pos;
                    currentPlayer.transform.position = chair2Pos;
                    toldStory = true;
                    inDialog = false;
                    ui.dialogBox.SetActive(false);
                    currentDialog = 0;
                    StopAllCoroutines();
                }
                else if (currentlySayingDialogs == afterStoryDialog)
                {
                    endedStory = true;
                    fuat.optionChoice = true;
                    fuat.storyTold = true;
                    knifeGiver.storyTold = true;
                    knifeGiver.optionChoice = true;
                    inDialog = false;
                    ui.dialogBox.SetActive(false);
                    currentDialog = 0;
                    StopAllCoroutines();
                    currentPlayer.altScript.freeze = false;
                    currentPlayer.altScript.iFrames = false;
                }
                else if(!optionChoice)
                {
                    inDialog = false;
                    ui.dialogBox.SetActive(false);
                    currentDialog = 0;
                    StopAllCoroutines();
                    currentPlayer.altScript.freeze = false;
                    currentPlayer.altScript.iFrames = false;

                }
                else
                {
                    ui.dialogText.gameObject.SetActive(false);
                    ui.dialogOptions.SetActive(true);
                    ui.opt1Text.text = option1;
                    ui.opt2Text.text = option2;
                    waitingForOption = true;
                    optionChoice = false;
                }

            }
        }
        else
        {
            if (ui.dialogOption1.pressed)
            {
                ui.dialogOption1.pressed = false;
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
                ui.dialogOption2.pressed = false;
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
        if (other.CompareTag("Player") && other.isTrigger && !inDialog)
        {
            ui.interactButton.pressed = false;
            canInteract = true;
            ui.interactButton.gameObject.SetActive(true);
            currentPlayer = other.GetComponent<PlayerItem>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            ui.interactButton.pressed = false;
            canInteract = false;
            ui.interactButton.gameObject.SetActive(false);
            if (inDialog)
            {
                ui.dialogBox.SetActive(false);
                inDialog = false;
                currentDialog = 0;
                StopAllCoroutines();
            }
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
    }*/
}
