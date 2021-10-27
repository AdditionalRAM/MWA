using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoldToClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown;
    private float pointerDownTimer;

    public float requiredHoldTime;

    public UnityEvent onLongClick;

    public Image fillImage;

    public void OnPointerDown(PointerEventData data)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        Reset();
    }

    private void Update()
    {
        if (pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if(pointerDownTimer >= requiredHoldTime)
            {
                if (onLongClick != null)
                {
                    onLongClick.Invoke();                   
                }
                Reset();
            }if (fillImage != null) fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
        }
    }

    void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
        if (fillImage != null) fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
    }
}
