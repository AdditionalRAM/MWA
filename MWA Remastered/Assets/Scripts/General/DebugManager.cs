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

    int m_frameCounter = 0;
    float m_timeCounter = 0.0f;
    float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;

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
            case "setmaxfps":
                Application.targetFrameRate = 
                int.Parse(debugCommands[1]);
                break;
            case "toggleplayercollider":
                foreach (BoxCollider2D collider in FindObjectOfType<PlayerMovement>().GetComponents<BoxCollider2D>())
                {
                    if (!collider.isTrigger) collider.enabled = !collider.enabled;
                    outOut = "Toggled player collider";
                }
                break;
            case "resetplayerprefs":
                PlayerPrefs.DeleteAll();
                outOut = "Reset PlayerPrefs";
                break;
            default:
                outOut = "Invalid command " + debugCommands[0];
                break;
        }
        DebugOut(outOut);
    }

    private void Update()
    {
        UIReferences.instance.fpsLimit.text = "LIMIT : " + Application.targetFrameRate;
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }
        UIReferences.instance.fps.text = "FPS : " + (int)m_lastFramerate;
        //(int)(Time.frameCount / Time.time);
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
