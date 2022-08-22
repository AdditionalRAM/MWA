using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugManager : MonoBehaviour
{
    public static DebugManager instance;

    public GameObject debugMenu;
    public TMP_InputField debugInput;
    public TMP_Text debugConsole;
    public string[] debugCommands;
    public Vector3 oldPos;

    private void Awake()
    {
        instance = this;
    }

    public void ToggleDebugMenu(bool open)
    {
        debugMenu.SetActive(open);
    }

    public void Debug()
    {
        debugCommands = debugInput.text.Split(" ", System.StringSplitOptions.None);
        string outOut = "";
        debugInput.text = "";
        switch (debugCommands[0])
        {
            case "oldpos":
                playerM().transform.position = oldPos;
                outOut = "Teleported player to the old pos of " + oldPos.ToString();
                break;
            case "debugroom":
                oldPos = playerM().transform.position;
                playerM().transform.position = new Vector3(13, 0, 0);
                outOut = "Teleported player to debug room. Old position is " + oldPos.ToString();
                break;
            case "loadsave":
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
                break;
            case "savesave":
                GetComponent<SaveManager>().Save();
                outOut = "Saved save.";
                break;
            case "erasesave":
                GetComponent<SaveManager>().Erase();
                outOut = "Erased save.";
                break;
            case "setplayerpos":
                oldPos = playerM().transform.position;
                Vector3 newPos = ParseStrings(debugCommands[1], debugCommands[2], "0");
                FindObjectOfType<PlayerMovement>().transform.position = newPos;
                outOut = "Teleported player to " + newPos.ToString();
                break;
            default:
                outOut = "Invalid command " + debugCommands[0];
                break;
        }
        DebugOut(outOut);
    }

    public void ClearConsole()
    {
        debugConsole.text = "Debug menu, if you aint a dev you arent supposed to be here";
    }

    public void DebugOut(string toOut)
    {
        debugConsole.text += "\n" + toOut;
    }

    public static Vector3 ParseStrings(string str1, string str2, string str3)
    {
        float a = float.Parse(str1);
        float b = float.Parse(str2);
        float c = float.Parse(str3);
        return new Vector3(a, b, c);
    }

    public PlayerMovement playerM()
    {
        return FindObjectOfType<PlayerMovement>();
    }

    public PlayerItem playeri()
    {
        return FindObjectOfType<PlayerItem>();
    }

}
