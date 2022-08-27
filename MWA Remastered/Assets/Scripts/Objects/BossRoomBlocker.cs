using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomBlocker : MonoBehaviour
{
    public GameObject blocker;
    public GameObject cutsceneKnight, bossKnight;
    //enables boss on EnableBlocker() if the cutscene boss doesnt exist
    public bool enableBossOnCutscene;

    public void EnableBlocker()
    {
        blocker.SetActive(true);
        if (!cutsceneKnight.activeInHierarchy && enableBossOnCutscene)
        {
            Invoke("EnableBoss", 1f);
            enableBossOnCutscene = false;
        }
    }

    public void EnableBoss()
    {
        bossKnight.SetActive(true);
    }

}
