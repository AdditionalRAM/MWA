using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KologrentBossCheck : MonoBehaviour
{
    public GameObject[] activeYesBoss, activeNoBoss;

    private void Update()
    {
        foreach (GameObject obj in activeYesBoss)
        {
            obj.SetActive(SaveGame.bossesKilled[0]);
        }
        foreach (GameObject obj in activeNoBoss)
        {
            obj.SetActive(!SaveGame.bossesKilled[0]);
        }
    }
}
