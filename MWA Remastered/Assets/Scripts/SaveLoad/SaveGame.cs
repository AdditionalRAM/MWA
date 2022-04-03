using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveGame
{
    public static float[] playerPos = new float[2];
    public static string placeName;

    public static bool[] chestsOpened = new bool[10];
    public static bool[] doorsUnlocked = new bool[3];
    public static bool[] dungeonsComplete = new bool[2];
    public static bool[] bossesKilled = new bool[1];
    public static bool oldwomanDialog1, oldwomanToldStory;
    public static bool fuatDialog1, fuatKeyGiven;
    public static bool knifegiverGaveKnife;

    public static void ResetSaveGame()
    {
        playerPos = new float[2];
        placeName = "";
        chestsOpened = new bool[10];
        doorsUnlocked = new bool[3];
        dungeonsComplete = new bool[2];
        oldwomanDialog1 = false;
        oldwomanToldStory = false; ;
        fuatDialog1 = false;
        fuatKeyGiven = false;
        knifegiverGaveKnife = false;
}
}
