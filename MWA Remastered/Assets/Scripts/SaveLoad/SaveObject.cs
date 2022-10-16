using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveObject
{
    public float[] playerPosition;
    public string placeName;
    public bool[] dungeonsComplete, bossesKilled;
    public bool[] chestsOpened, doorsUnlocked;
    public int[] questProgresses;
    public int currentQuest;
    public bool oldwomanDialog1, oldwomanToldStory;
    public bool fuatDialog1, fuatKeyGiven;
    public bool knifegiverGaveKnife;

    public SaveObject()
    {
        playerPosition = SaveGame.playerPos;
        placeName = SaveGame.placeName;
        oldwomanDialog1 = SaveGame.oldwomanDialog1;
        oldwomanToldStory = SaveGame.oldwomanToldStory;
        fuatDialog1 = SaveGame.fuatDialog1;
        fuatKeyGiven = SaveGame.fuatKeyGiven;
        knifegiverGaveKnife = SaveGame.knifegiverGaveKnife;
        chestsOpened = SaveGame.chestsOpened;
        doorsUnlocked = SaveGame.doorsUnlocked;
        dungeonsComplete = SaveGame.dungeonsComplete;
        bossesKilled = SaveGame.bossesKilled;
        questProgresses = SaveGame.questProgresses;
        currentQuest = SaveGame.currentQuest;
    }
}
