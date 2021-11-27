using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizeText : MonoBehaviour, ILocalization
{
    public string textID;
    public bool localize, localizeOnAwake = false;
    public Text myText;

    private void Awake()
    {
        if (GetComponent<Text>() != null) myText = GetComponent<Text>();
        if (localizeOnAwake) OnLocalize();
    }

    public void OnLocalize()
    {
        if (localize && myText != null)
        {
            myText.text = LocalManager.Localize(textID);
        }
        else if (myText == null) Debug.Log("myText is null!");
    }
}
