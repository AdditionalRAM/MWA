using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewQuestWindow : MonoBehaviour
{
    [SerializeField] private Text title, body;

    public void SetTexts(string _title, string _body)
    {
        title.text = _title;
        body.text = _body;
    }

    public void StayOpenFor(float time)
    {
        StartCoroutine(StayOpenForCo(time));
    }


    IEnumerator StayOpenForCo(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
