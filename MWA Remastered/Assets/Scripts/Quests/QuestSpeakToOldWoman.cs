using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSpeakToOldWoman : MonoBehaviour
{
    public int progress = 0;
    public bool complete;
    public int questID;
    //progress = 0 means we didnt start yet
    public string titleID;
    public string[] bodyIDs;
    public Vector3[] objectivePositions;
    public List<GameObject> exclamations = new List<GameObject>();

    public NPCDialogue oldMan;
    public NPCDialogue oldWoman;

    public void DrawExclamation()
    {
        if (progress <= 0 || complete) return;
        RemoveOldExclamations();
        GameObject _exc = Instantiate(QuestHolder.instance.exclamationPrefab,
            objectivePositions[progress - 1], Quaternion.identity);
        exclamations.Add(_exc);
        Debug.Log("Drew exclamation at " + _exc.transform.position);
    }

    public void RemoveOldExclamations()
    {
        foreach (GameObject oldExc in exclamations)
        {
            Destroy(oldExc);
        }
        exclamations.Clear();
    }

    public void SpokenToOldMan()
    {
        oldMan.onOption1End.RemoveAllListeners();
        if (progress >= 1) return;
        progress++;
        QuestHolder.instance.CreateNewQuestWindow(titleID, 0);
        DrawExclamation();
    }

    public void OldWomanAskForDaisy()
    {
        if (progress >= 2) return;
        progress = 2;
        QuestHolder.instance.CreateNewQuestWindow(titleID, 1);
        DrawExclamation();
    }

    public void OldWomanCompleteQuest()
    {
        if (complete) return;
        QuestHolder.instance.CreateNewQuestWindow(titleID, 2);
        progress = 3;
        complete = true;
        RemoveOldExclamations();
        QuestHolder.instance.questSHim.StartQuest();
    }
}
