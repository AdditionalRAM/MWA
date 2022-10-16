using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public int questID, progress = 0;
    //progress = 0 means we didnt start yet
    public string titleID;
    public string[] bodyIDs;
    public Vector3[] objectivePositions;
}
