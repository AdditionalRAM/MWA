using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    UIReferences ui;
    public GameObject gameOverScreen, deathEffect, otherUI, popup;
    public Animator gameoverBck;
    public Transform gameoverMenu;
    public AudioClip gameoverTheme;
    public Vector3 menuOn, menuOff;
    public float tweenDur, waitForPause;

    private void Awake()
    {
        ui = GetComponent<UIReferences>();
        menuOn.y = Screen.height / 2;
    }
    public void GameOver(Transform player)
    {
        StartCoroutine(GameOverCo(player));
    }

    public void StartPopup()
    {
        popup.transform.DOScale(1.5f, tweenDur).SetUpdate(true);
    }

    public void ClosePopup()
    {
        popup.transform.DOScale(0, tweenDur).SetUpdate(true);
    }

    IEnumerator GameOverCo(Transform player)
    {
        GameObject _ = Instantiate(deathEffect, player.transform.position, Quaternion.identity);
        Destroy(_, 1f);
        player.localScale = Vector3.zero;
        gameOverScreen.SetActive(true);
        otherUI.SetActive(false);
        ui.bgMusicSource.Stop();
        yield return new WaitForSecondsRealtime(waitForPause);
        ui.bgMusicSource.clip = gameoverTheme;
        ui.bgMusicSource.Play();
        gameoverBck.SetBool("on", true);
        gameoverMenu.DOMoveY(menuOn.y, tweenDur).SetUpdate(true);
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
