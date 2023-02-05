using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialMessages : MonoBehaviour, IOnPlayerGetItem, IOnPlayerEquipItem, ILocalization, IUseOnSave
{
    public ItemObject knife;
    public float disabledY, enabledY, tweenDur;
    public string[] tutorialMessages;
    public int tutorialProgress = 0;
    public int playerAttackCounter, playerAttackGoal;

    public void OnPlayerEquipItem(ItemObject item)
    {
        if (item == knife && tutorialProgress == 2)
        {
            tutorialProgress = 3;
            StartCoroutine(TutorialDisableEnableCo(tutorialMessages[2]));
        }
    }

    IEnumerator TutorialDisableEnableCo(string changeText)
    {
        TutorialDialogEnable(false);
        yield return new WaitForSeconds(tweenDur);
        WriteToTutorialDialog(changeText);
        TutorialDialogEnable(true);
    }

    public void OnPlayerGetItem(ItemObject item, int itemCount)
    {
        if(item == knife && tutorialProgress == 1)
        {
            WriteToTutorialDialog(tutorialMessages[1]);
            TutorialDialogEnable(true);
            tutorialProgress = 2;
        }
    }

    public void PlayerAttacked()
    {
        playerAttackCounter++;
        if (playerAttackCounter >= playerAttackGoal)
        {
            if (tutorialProgress == 3)
            {
                TutorialDialogEnable(false);
                tutorialProgress = 4;
            }
        }
    }

    private void Update()
    {
        if (transform.position.magnitude > 1 && tutorialProgress == 0)
        {
            TutorialDialogEnable(false);
            tutorialProgress = 1;
        }
    }

    public void WriteToTutorialDialog(string _text)
    {
        UIReferences.instance.tutorialDialogText.text = _text;
    }

    public void TutorialDialogEnable(bool enable)
    {
        RectTransform tutorialDialog = UIReferences.instance.tutorialDialog.GetComponent<RectTransform>();
        float finalY = disabledY;
        if (enable) finalY = enabledY;
        tutorialDialog.DOAnchorPosY(finalY, tweenDur);
    }

    public void OnLocalize()
    {
        tutorialMessages = LocalManager.LocalizeArray(tutorialMessages);
        tutorialProgress = PlayerPrefs.GetInt("tprogress", 0);
        if (transform.position.magnitude <= 1 && tutorialProgress == 0)
        {
            WriteToTutorialDialog(tutorialMessages[0]);
            TutorialDialogEnable(true);
        }
    }

    public void OnBeforeGameSave()
    {
        PlayerPrefs.SetInt("tprogress", tutorialProgress);
    }
}
