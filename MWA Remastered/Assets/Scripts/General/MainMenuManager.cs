using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour, ILocalization
{
    public GameObject audioOn, audioOff, musicOn, musicOff, lightsOn, lightsOff, blek;
    public AudioSource bgMusicSource;
    public GameObject mainMenu, creditsMenu, lightingInfoTab;
    public float tweenDur;
    public Vector3 creditsOpen, creditsClosed;
    public Text versionText;
    public GameObject[] difLangCredits;

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

    public void ToggleLighting(bool toggle)
    {
        int newVal = 1;
        if (!toggle) newVal = 0;
        PlayerPrefs.SetInt("enableLighting", newVal);
        lightsOn.SetActive(toggle);
        lightsOff.SetActive(!toggle);
    }

    public void Awake()
    {
        int musicToggle = PlayerPrefs.GetInt("musicToggle", 1);
        int audioToggle = PlayerPrefs.GetInt("audioToggle", 1);
        int lightToggle = PlayerPrefs.GetInt("enableLighting", 1);
        ToggleMusic(musicToggle == 1);
        ToggleSound(audioToggle == 1);
        ToggleLighting(lightToggle == 1);
        creditsOpen = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        creditsClosed = new Vector3(Screen.width * 2, Screen.height / 2, 0);
        versionText.text = Application.version;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCo());
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
        yield return new WaitForSecondsRealtime(tweenDur/8);
        mainMenu.SetActive(true);
    }

    IEnumerator StartGameCo()
    {
        blek.SetActive(true);
        //blek.GetComponent<Image>().DOFade(255, tweenDur/2);
        bgMusicSource.mute = true;
        yield return null;
        SceneManager.LoadScene("Overworld");
    }

    public void OnLocalize()
    {
        foreach (GameObject credits in difLangCredits)
        {
            credits.SetActive(false);
        }
        difLangCredits[GetComponent<Localizer>().currentLang - 1].SetActive(true);
    }

    public void OpenLightingInfo()
    {
        lightingInfoTab.transform.DOScale(Vector3.one, tweenDur / 2);
    }

    public void CloseLightingInfo()
    {
        lightingInfoTab.transform.DOScale(Vector3.zero, tweenDur / 2);
    }

}
