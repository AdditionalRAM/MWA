using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LocalManager
{
    public static bool ReadyToLocalize;
    public static List<string> currentValues = new List<string>();
    public static List<string> keys = new List<string>();
    public static Dictionary<string, string> KeyToValue = new Dictionary<string, string>();

    public static void LoadKeys(string keysFile)
    {
        keys = new List<string>();
        char[] delims = new[] { '\r', '\n' };
        string[] _keys = keysFile.Split(delims, StringSplitOptions.RemoveEmptyEntries);
        foreach (string KEY in _keys)
        {
            keys.Add(KEY);
        }
    }

    public static void LoadLanguage(string langFile)
    {
        currentValues = new List<string>();
        char[] delims = new[] { '\r', '\n' };
        string[] _langs = langFile.Split(delims, StringSplitOptions.RemoveEmptyEntries);
        foreach (string LANG in _langs)
        {
            currentValues.Add(LANG);
        }
    }

    public static void WriteToDic()
    {
        KeyToValue = new Dictionary<string, string>();
        for (int i = 0; i < keys.Count; i++)
        {
            KeyToValue.Add(keys[i], currentValues[i]);
        }
        ReadyToLocalize = true;
    }

    public static string Localize(string textID)
    {
        string ToOut = "";
        if(KeyToValue.TryGetValue(textID, out ToOut))
        {
            return ToOut;
        }
        else
        {
            //Debug.Log(textID + " couldn't be found in the dictionary");
            return "";
        }
    }

    public static string[] LocalizeArray(string[] texts)
    {
        string[] stringsToReturn = new string[texts.Length];
        for (int i = 0; i < texts.Length; i++)
        {
            stringsToReturn[i] = Localize(texts[i]);
        }
        return stringsToReturn;
    }
}
