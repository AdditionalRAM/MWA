using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestHolder : MonoBehaviour
{
    public static QuestHolder instance;

    public GameObject exclamationPrefab, newQuestWindowPrefab;
    public string[] objectiveTexts;

    public QuestSpeakToOldWoman questSTOW;
    public QuestSaveHim questSHim;

    private void Awake()
    {
        instance = this;
    }

    public void CreateNewQuestWindow(string body, int style)
    {
        //style = 0 -> New Objective

        if (style <= objectiveTexts.Length)
        {
            GameObject _window = Instantiate(newQuestWindowPrefab, UIReferences.instance.canvas);
            string _title = LocalManager.Localize(objectiveTexts[style]);
            string _body = LocalManager.Localize(body);
            _window.GetComponent<NewQuestWindow>().SetTexts(_title, _body);
            _window.GetComponent<NewQuestWindow>().StayOpenFor(3.5f);
            GetComponent<AudioSource>().Play();
        }
        else return;

    }
}
