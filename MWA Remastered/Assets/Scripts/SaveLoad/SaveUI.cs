using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SaveUI : MonoBehaviour, IUseSaveGame
{
    SaveManager manager;
    public Text placeText;
    public string noSaveText;
    public float saveCloseDelay, tweenDur, tweenEnd;

    private void Awake()
    {
        manager = FindObjectOfType<SaveManager>();
    }

    public void OnAfterGameLoad()
    {
        UpdatePlaceText();
    }

    public void UpdatePlaceText()
    {
        if (SaveGame.placeName != null) 
        { 
            placeText.text = SaveGame.placeName;
        }
        else
            placeText.text = noSaveText;
    }

    public void EnableSaveMenu()
    {
        UpdatePlaceText();
        transform.DOScale(tweenEnd, tweenDur);
    }

    public void DisableSaveMenu()
    {
        transform.DOScale(0, tweenDur);
    }

    public void EraseFile()
    {
        manager.Erase();
        placeText.text = noSaveText;
    }

    public void SaveFile()
    {
        manager.Save();
        UpdatePlaceText();
    }

    public void LoadFile()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
