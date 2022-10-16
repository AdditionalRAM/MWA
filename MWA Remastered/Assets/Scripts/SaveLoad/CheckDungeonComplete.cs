using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDungeonComplete : MonoBehaviour, IUseSaveGame
{
    public int myDungeonID;
    public GameObject entrance, noEnter;

    public void OnAfterGameLoad()
    {
        entrance.SetActive(!SaveGame.dungeonsComplete[myDungeonID]);
        noEnter.SetActive(SaveGame.dungeonsComplete[myDungeonID]);
    }

}
