using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemInfoTab : MonoBehaviour
{
    public Text objectName, stat1, stat2, stat3, useMSG;
    public Button useButton;
    public float endScale, tweenTime;

    private void OnEnable()
    {
        transform.DOScaleY(endScale, tweenTime);
    }

    public void DisableTab()
    {
        Destroy(gameObject, tweenTime);
        transform.DOScaleY(0, tweenTime);
    }
}
