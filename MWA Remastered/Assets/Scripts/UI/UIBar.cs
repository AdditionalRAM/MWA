using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public Slider slider;
    public Text valText;
    public Animator an;

    private void Awake()
    {
        if(GetComponent<Animator>() != null)
        an = GetComponent<Animator>();
    }
    public void SetMaxValue(float maxVal, bool wholeNumbers)
    {
        slider.wholeNumbers = wholeNumbers;
        slider.maxValue = maxVal;
        slider.value = maxVal;
        valText.text = maxVal.ToString();
    }

    public void SetValue(float val)
    {
        if (val < 0) val = 0;
        slider.value = val;
        valText.text = val.ToString();
    }
}
