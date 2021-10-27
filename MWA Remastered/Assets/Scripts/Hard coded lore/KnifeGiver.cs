using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeGiver : NPCDialogue
{
    public string[] afterKnife;
    public GameObject knifePrefab;
    public Vector3 knifePos;

    public void OnOption2End()
    {
        canOption = false; optionChoice = false;
        currentlySayingDialogs = afterKnife;
        Instantiate(knifePrefab, knifePos, Quaternion.identity);
    }

    /*UIReferences ui;

    public GameObject knifePrefab;
    public Vector3 knifePos;

    AudioSource dialogSound;
    PlayerMovement player;

    public float textDelay;

    public bool makeSound, optionChoice, canInteract = false, inDialog = false, waitingForOption = false, controlledFromOutside = false, canOption;
    public bool storyTold, knifeGiven;

    public string nameToDisplay, option1, option2;
    public string[] defDialogs, dialogs, dialogsOption1, dialogsOption2, afterKnife;
    public string[] currentlySayingDialogs;

    int currentDialog = 0;

    public void Awake()
    {
        dialogSound = GetComponent<AudioSource>();
        ui = FindObjectOfType<UIReferences>();
        currentlySayingDialogs = defDialogs;
        canOption = optionChoice;
    }

    public void DialogInteract()
    {
        if (!inDialog)
        {
            canOption = optionChoice;
            if(storyTold && !knifeGiven)
            {
                currentlySayingDialogs = dialogs;
            }else if(storyTold && knifeGiven)
            {
                currentlySayingDialogs = afterKnife;
            }
            player.freeze = true;
            ui.dialogOptions.SetActive(false);
            ui.dialogText.gameObject.SetActive(true);
            currentDialog = 0;
            inDialog = true;
            ui.dialogBox.SetActive(true);
            ui.dialogName.text = nameToDisplay;
            ui.dialogText.text = "";
            ui.interactButton.gameObject.SetActive(false);
            StartCoroutine(Dialog());
        }
        else if (inDialog && currentDialog + 1 < currentlySayingDialogs.Length)
        {
            currentDialog++;
            ui.dialogBox.SetActive(true);
            ui.dialogName.text = nameToDisplay;
            ui.dialogText.text = "";
            ui.interactButton.gameObject.SetActive(false);
            StartCoroutine(Dialog());
        }
        else if (inDialog && currentDialog + 1 >= currentlySayingDialogs.Length)
        {
            if(currentlySayingDialogs == dialogsOption2)
            {
                Instantiate(knifePrefab, knifePos, Quaternion.identity);
                knifeGiven = true;
                inDialog = false;
                ui.dialogBox.SetActive(false);
                currentDialog = 0;
                StopAllCoroutines();
                player.freeze = false;
                player.iFrames = false;
                canOption = false;
                optionChoice = false;
            }
            if (!canOption)
            {
                inDialog = false;
                ui.dialogBox.SetActive(false);
                currentDialog = 0;
                StopAllCoroutines();
                player.freeze = false;
                player.iFrames = false;
            }
            else
            {
                ui.interactButton.gameObject.SetActive(false);
                ui.dialogText.gameObject.SetActive(false);
                ui.dialogOptions.SetActive(true);
                ui.opt1Text.text = option1;
                ui.opt2Text.text = option2;
                canOption = false;
                waitingForOption = true;
            }
        }

    }


    private void Update()
    {
        if (!waitingForOption)
        {
            if (!controlledFromOutside && ui.interactButton.pressed && canInteract)
            {
                ui.interactButton.pressed = false;
                DialogInteract();
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
            canInteract = true;
            ui.interactButton.gameObject.SetActive(true);
            player = other.GetComponent<PlayerMovement>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
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
        foreach (char character in currentlySayingDialogs[currentDialog])
        {
            if (makeSound && character != ' ')
            {
                dialogSound.Play();
            }
            yield return new WaitForSeconds(textDelay);
            ui.dialogText.text += character;
        }
        ui.interactButton.gameObject.SetActive(true);
    }*/
}
