using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject audioOn, audioOff, musicOn, musicOff;
    public AudioSource bgMusicSource;
    public GameObject mainMenu, creditsMenu;
    public float tweenDur;
    public Vector3 creditsOpen, creditsClosed;
    public Text versionText;

    public void ToggleSound(bool toggle)
    {
        int newVol = 1;
        if (!toggle) newVol = 0;
        AudioListener.volume = newVol;
        PlayerPrefs.SetInt("audioToggle", newVol);
        audioOn.SetActive(toggle);
        audioOff.SetActive(!toggle);
    }

    public void ToggleMusic(bool toggle)
    {
        int newVol = 1;
        if (!toggle) newVol = 0;
        bgMusicSource.mute = !toggle;
        PlayerPrefs.SetInt("musicToggle", newVol);
        musicOn.SetActive(toggle);
        musicOff.SetActive(!toggle);
    }

    public void Awake()
    {
        int musicToggle = PlayerPrefs.GetInt("musicToggle", 1);
        int audioToggle = PlayerPrefs.GetInt("audioToggle", 1);
        if (musicToggle == 1) ToggleMusic(true);
        else ToggleMusic(false);
        if (audioToggle == 1) ToggleSound(true);
        else ToggleSound(false);
        creditsOpen = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        creditsClosed = new Vector3(Screen.width * 2, Screen.height / 2, 0);
        versionText.text = Application.version;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Overworld");
    }

    public void OpenCredits()
    {
        StartCoroutine(OpenCreditsCo());
    }

    public void CloseCredits()
    {
        StartCoroutine(CloseCreditsCo());
    }

    IEnumerator OpenCreditsCo()
    {
        creditsMenu.transform.DOMove(creditsOpen, tweenDur);
        yield return new WaitForSecondsRealtime(tweenDur/2);
        mainMenu.SetActive(false);
    }

    IEnumerator CloseCreditsCo()
    {
        creditsMenu.transform.DOMove(creditsClosed, tweenDur);
        yield return new WaitForSecondsRealtime(tweenDur/5);
        mainMenu.SetActive(true);
    }
}
