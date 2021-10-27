using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public Slider slider;
    public Text valText;

    public void SetMaxValue(float maxVal, bool wholeNumbers)
    {
        slider.wholeNumbers = wholeNumbers;
        slider.maxValue = maxVal;
        slider.value = maxVal;
        valText.text = maxVal.ToString();
    }

    public void SetValue(float val)
    {
        slider.value = val;
        valText.text = val.ToString();
    }
}
