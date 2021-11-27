using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour, IUseSaveGame
{
    public UIReferences ui;
    public bool gamePaused;

    public AudioListener cameraListener;
    public Animator pauseBck;
    public Transform pauseMenu;
    public Vector3 menuOn, menuOff;
    public GameObject audioOn, audioOff, musicOn, musicOff, lobbyPopup;
    public float tweenDur, waitForResume;

    private void Awake()
    {
        menuOn.y = Screen.height / 2;
    }
    public void Pause()
    {
        Time.timeScale = 0;
        ui.pauseMenu.gameObject.SetActive(true);
        pauseBck.SetBool("on", true);
        pauseMenu.DOMoveY(menuOn.y, tweenDur).SetUpdate(true);
        gamePaused = true;
    }

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
        ui.bgMusicSource.mute = !toggle;
        PlayerPrefs.SetInt("musicToggle", newVol);
        musicOn.SetActive(toggle);
        musicOff.SetActive(!toggle);
    }

    public void Resume()
    {
        StartCoroutine(ResumeCo());        
    }

    IEnumerator ResumeCo()
    {
        pauseMenu.DOMoveY(menuOff.y, tweenDur).SetUpdate(true);
        pauseBck.SetBool("on", false);
        yield return new WaitForSecondsRealtime(waitForResume);
        ui.pauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
        gamePaused = false;
    }

    public void OnAfterGameLoad()
    {
        int musicToggle = PlayerPrefs.GetInt("musicToggle", 1);
        int audioToggle = PlayerPrefs.GetInt("audioToggle", 1);
        if (musicToggle == 1) ToggleMusic(true);
        else ToggleMusic(false);
        if (audioToggle == 1) ToggleSound(true);
        else ToggleSound(false);
    }

    public void ReturnToMenu()
    {
        lobbyPopup.transform.DOScale(1.5f, tweenDur).SetUpdate(true);
    }

    public void PopupClose()
    {
        lobbyPopup.transform.DOScale(0, tweenDur).SetUpdate(true);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
