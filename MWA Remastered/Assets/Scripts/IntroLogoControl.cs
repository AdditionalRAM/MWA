using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IntroLogoControl : MonoBehaviour
{
    public Transform logo, blek, canvas, langSelect;
    public RectTransform logoRect;
    public AudioSource bgMusic, vineBoom, zap1, zap2;
    public Vector3 endPos, endScale, finalEndPos;
    public float tweenDur, amountToWait;
    public bool debug;

    private void Awake()
    {
        logoRect = logo.GetComponent<RectTransform>();
        endPos.y = Screen.height / 1.21348314607f;
        /*//this is the ui element

        //first you need the RectTransform component of your canvas
        RectTransform CanvasRect = canvas.GetComponent<RectTransform>();

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Camera.main.ViewportToWorldPoint(endPos);
        */
    }

    public void StartAnimation()
    {
        StartCoroutine(AnimCo());
    }

    public void DisableMe()
    {
        gameObject.SetActive(false);
        langSelect.gameObject.SetActive(true);
        bgMusic.Play();
    }

    public void DisableBlek()
    {
        blek.gameObject.SetActive(false);
        transform.localScale = Vector3.one*5;
    }

    public void PlayZap(int zapNo)
    {
        if (zapNo == 1) zap1.Play();
        else if (zapNo == 2) zap2.Play();
    }

    public IEnumerator AnimCo()
    {
        logo.gameObject.SetActive(true);
        vineBoom.Play();
        yield return new WaitForSecondsRealtime(amountToWait);
        //Vector3 worldSpace = Camera.main.ViewportToWorldPoint(endPos);
        //logo.DOMoveY(worldSpace.y, tweenDur);
        logoRect.DOMoveY(endPos.y, tweenDur);
        logo.DOScale(endScale, tweenDur);
    }

    private void Update()
    {
        Vector3 worldSpace = Camera.main.ViewportToWorldPoint(endPos);
        if (debug) logoRect.position = worldSpace;
    }
}
