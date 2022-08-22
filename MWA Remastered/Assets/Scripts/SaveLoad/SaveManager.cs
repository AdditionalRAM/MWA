using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

public class SaveManager : MonoBehaviour
{
    public UIReferences ui;
    public InventoryTabManager invs;
    public string saveFolder, savePath;

    private void Start()
    {
        ui = GetComponent<UIReferences>();
        invs = FindObjectOfType<InventoryTabManager>();
        Load();
    }

    public void SetDataToSaveGame(SaveObject data)
    {
        SaveGame.playerPos = data.playerPosition;
        SaveGame.placeName = data.placeName;
        SaveGame.oldwomanDialog1 = data.oldwomanDialog1;
        SaveGame.oldwomanToldStory = data.oldwomanToldStory;
        SaveGame.fuatDialog1 = data.fuatDialog1;
        SaveGame.fuatKeyGiven = data.fuatKeyGiven;
        SaveGame.knifegiverGaveKnife = data.knifegiverGaveKnife;
        if (SaveGame.doorsUnlocked.Length > data.doorsUnlocked.Length)
        {
            Debug.Log("doorsUnlocked alert");
            for (int i = 0; i < data.doorsUnlocked.Length; i++)
            {
                SaveGame.doorsUnlocked[i] = data.doorsUnlocked[i];
            }
        }
        else SaveGame.doorsUnlocked = data.doorsUnlocked;
        if(SaveGame.chestsOpened.Length > data.chestsOpened.Length)
        {
            Debug.Log("chestsOpened alert");
            for (int i = 0; i < data.chestsOpened.Length; i++)
            {
                SaveGame.chestsOpened[i] = data.chestsOpened[i];
            }
        }
        else SaveGame.chestsOpened = data.chestsOpened;
        if (SaveGame.dungeonsComplete.Length > data.dungeonsComplete.Length)
        {
            Debug.Log("dungeonsComplete alert");
            for (int i = 0; i < data.dungeonsComplete.Length; i++)
            {
                SaveGame.dungeonsComplete[i] = data.dungeonsComplete[i];
            }
        }
        else SaveGame.dungeonsComplete = data.dungeonsComplete;
        if (SaveGame.bossesKilled.Length > data.bossesKilled.Length)
        {
            Debug.Log("bossesKilled alert");
            for (int i = 0; i < data.bossesKilled.Length; i++)
            {
                SaveGame.bossesKilled[i] = data.bossesKilled[i];
            }
        }
        else SaveGame.bossesKilled = data.bossesKilled;
    }

    public void Save()
    {
        var ss = FindObjectsOfType<MonoBehaviour>().OfType<IUseOnSave>();       
        foreach (IUseOnSave s in ss)
        {
            s.OnBeforeGameSave();
        }
        invs.SaveInventories();
        string nowPath = Application.persistentDataPath + saveFolder;
        if(!Directory.Exists(nowPath)) Directory.CreateDirectory(nowPath);    
        nowPath += savePath;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = File.Create(nowPath);
        SaveObject data = new SaveObject();
        string dataJSON = JsonUtility.ToJson(data, true);
        bf.Serialize(stream, dataJSON);
        Debug.Log("Saved");
        stream.Close();
    }

    public void Load()
    {
        SaveGame.ResetSaveGame();
        ui.transitionBlocker.SetActive(true);
        invs.LoadInventories();
        string nowPath = Application.persistentDataPath + saveFolder + savePath;
        if (File.Exists(nowPath))
        {
            SaveObject data = new SaveObject();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = File.Open(nowPath, FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(stream).ToString(), data);
            stream.Close();
            SetDataToSaveGame(data);
        }
        if(Directory.Exists(Application.persistentDataPath + "/savedata/amogus/sus/enabledebugmenuordietmr"))
        {
            ui.debugMenuButton.SetActive(true);
        }
        else
        {
            ui.debugMenuButton.SetActive(false);
        }
        var ss = FindObjectsOfType<MonoBehaviour>().OfType<IUseSaveGame>();
        foreach (IUseSaveGame s in ss)
        {
            s.OnAfterGameLoad();
        }
        Debug.Log("Loaded");
        Invoke("TransitionEnd", .5f);
    }

    public void Erase()
    {
        string nowPath = Application.persistentDataPath + saveFolder + savePath;
        invs.EraseInventories();
        if (File.Exists(nowPath))
        {
            File.Delete(nowPath);
        }
        Debug.Log("Erased");
    }

    void TransitionEnd()
    {
        ui.transitionBlocker.SetActive(false);
    }

}
