using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    public int currentRoom;
    public RectTransform mmTexture, mmUnknown, playerHead, extendedMMPoint;
    public RectTransform[] mmPoints;
    public Vector2 minimapOffset;
    public float tweenDur;

    public Mask mapMask;
    public bool mapExtended;

    private void Awake()
    {
        mmPoints = new RectTransform[mmTexture.GetChild(0).childCount];
        for (int i = 0; i < mmTexture.GetChild(0).childCount; i++)
        {
            mmPoints[i] = mmTexture.GetChild(0).GetChild(i).GetComponent<RectTransform>();
        }
        minimapOffset.x = (-mmTexture.anchoredPosition.x)- mmPoints[0].anchoredPosition.x;
        minimapOffset.y = (-mmTexture.anchoredPosition.y) - mmPoints[0].anchoredPosition.y;
    }

    private void Update()
    {
        if (currentRoom >= 0 && !mapExtended)
        {
            mmUnknown.gameObject.SetActive(false);
            TweenMap(mmPoints[currentRoom], false);
        }
        else if (currentRoom < 0) { UnextendMap(); mmUnknown.gameObject.SetActive(true); }
        if(currentRoom >=0 ) TweenMapExtras(mmPoints[currentRoom]);
        if (Input.GetKeyDown(KeyCode.M)) ToggleMap();
    }

    public void TweenMap(RectTransform tweenTrans, bool scaleMap)
    {
        Vector2 calculatedPosition = -(tweenTrans.anchoredPosition + minimapOffset);
        if (mmTexture.anchoredPosition != calculatedPosition)
        {
            if (scaleMap) {
                mmTexture.localScale = new Vector3(0, 0, 0);
                mmTexture.DOScale(1, tweenDur);
                mmTexture.anchoredPosition = new Vector2(0, 0);
            }
            mmTexture.DOAnchorPos(calculatedPosition, tweenDur);
        }
    }

    public void TweenMapExtras(RectTransform tweenTrans)
    {
        playerHead.DOAnchorPos(tweenTrans.anchoredPosition, tweenDur);
    }

    public void ToggleMap()
    {
        if (!mapExtended && currentRoom >= 0) ExtendMap();
        else if(mapExtended) UnextendMap();
    }

    public void ExtendMap()
    {
        mapExtended = true;
        mapMask.enabled = false;
        TweenMap(extendedMMPoint, true);
    }

    public void UnextendMap()
    {
        mapExtended = false;
        mapMask.enabled = true;
    }
}
