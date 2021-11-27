using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour, IUseSaveGame
{
    public int bossID;
    public GameObject[] destroyIfNoBoss;
    public RoomSwitcher bosROM;
    public AudioClip sussyMusic;

    public void OnAfterGameLoad()
    {      
        if (SaveGame.bossesKilled[bossID])
        {
            if (bosROM != null) 
            { 
                bosROM.cutscenePlayed = true;
                bosROM.myMusic = sussyMusic;
            }
            foreach (GameObject obj in destroyIfNoBoss)
            {
                Destroy(obj);
            }
        }
    }
}
