using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InDungeon : MonoBehaviour
{
    public bool inDungeon, disabledThings;
    public int dungeonID, inDungeonCount;
    public GameObject[] activateInDungeon, activateIfComplete, deactivateIfComplete;

    private void Update()
    {
        inDungeon = inDungeonCount > 0;
        if (inDungeon) EnableObjects();
        else if (!inDungeon && !disabledThings)
        {
            Invoke("DisableObjects", .5f);
            disabledThings = true;
        }
    }

    public void CompleteDungeon()
    {
        SaveGame.dungeonsComplete[dungeonID] = true;
    }

    public void OnEnterDungeon()
    {
        inDungeonCount++;
    }

    public void EnableObjects()
    {
        disabledThings = false;
        foreach (GameObject obj in activateInDungeon)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in activateIfComplete)
        {
            obj.SetActive(SaveGame.dungeonsComplete[dungeonID]);
        }
        foreach (GameObject obj in deactivateIfComplete)
        {
            obj.SetActive(!SaveGame.dungeonsComplete[dungeonID]);
        }
    }

    public void DisableObjects()
    {
        foreach (GameObject obj in activateInDungeon)
        {
            obj.SetActive(false);
        }
    }

    public void OnExitDungeon()
    {
        inDungeonCount--;
    }
}
