using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSaveHim : MonoBehaviour//, IOnPlayerGetItem
{
    public int progress = 0;
    public bool complete;
    public int questID;
    //progress = 0 means we didnt start yet
    public string titleID;
    public string[] bodyIDs;
    public Vector3[] objectivePositions;
    public List<GameObject> exclamations = new List<GameObject>();
    public bool playerHasKnife, playerHasKey;

    public Fuat fuat;
    public PlayerItem player;

    public ItemObject knifeItem, fuatKeyItem;

    private void Update()
    {
        CheckItems();
    }

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

    public void StartQuest()
    {
        if (progress >= 1) return;
        progress = 1;
        DrawExclamation();
        QuestHolder.instance.CreateNewQuestWindow(titleID, 0);
    }

    public void SpokeToFuat()
    {
        if (progress >= 2) return;
        progress = 2;
        DrawExclamation();
        QuestHolder.instance.CreateNewQuestWindow(titleID, 1);
    }

    public void GotWeapon()
    {
        if (progress >= 3) return;
        progress = 3;
        DrawExclamation();
        QuestHolder.instance.CreateNewQuestWindow(titleID, 1);
    }

    public void GotKey()
    {
        if (progress >= 4) return;
        progress = 4;
        DrawExclamation();
        QuestHolder.instance.CreateNewQuestWindow(titleID, 1);
    }

    public void ExploredBasement()
    {
        if (progress >= 5) return;
        progress = 5;
        DrawExclamation();
        QuestHolder.instance.CreateNewQuestWindow(titleID, 1);
    }

    public void FinishedCave()
    {
        if (progress >= 6) return;
        progress = 6;
        DrawExclamation();
        QuestHolder.instance.CreateNewQuestWindow(titleID, 1);
    }

    public void FinishQuest()
    {
        if (complete) return;
        progress = 7;
        complete = true;
        RemoveOldExclamations();
        QuestHolder.instance.CreateNewQuestWindow(titleID, 2);
    }

    public void CheckItems()
    {
        playerHasKnife = player.equipInventory.HasItem(knifeItem, 1);
        playerHasKey = player.keyInventory.HasItem(fuatKeyItem, 1);
        if (progress == 2 && playerHasKnife) GotWeapon();
        if (progress == 3 && playerHasKey) GotKey();
    }
}
