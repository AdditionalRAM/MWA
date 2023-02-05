using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class UIReferences : MonoBehaviour
{
    public static UIReferences instance;

    public Transform canvas;

    public GameOverManager gameOverManger;
    //General
    public MultiPurposeButton interactButton, dialogOption1, dialogOption2;

    public GameObject quickHealButton, quickHealImage;
    public Text quickHealItemCount, quickHealCountdown;
    public GameObject tutorialDialog;
    public TMP_Text tutorialDialogText;

    //Dialogue
    public GameObject dialogBox, dialogOptions;
    public Text dialogName, dialogText, opt1Text, opt2Text;
    public Text susDialog;

    public GameObject transitionBlocker;
    public AudioSource bgMusicSource;

    //Bossbar
    public Text bossTitle;
    public UIBar bossBar;

    //pause
    public Transform disableOnPause, pauseMenu, gameOverMenu;

    //lighting 
    public Light2D globalLight;

    //debug
    public GameObject debugMenuButton;
    public TMP_Text fps, fpsLimit;

    public MinimapController minimapController;

    public LayerMask obstacle, obstacleNoArrow;

    public bool stopLighting;


    private void Awake()
    {
        instance = this;
        stopLighting = PlayerPrefs.GetInt("enableLighting", 1) == 0;
        if (stopLighting)
        {
            RoomSwitcher[] allRooms = FindObjectsOfType<RoomSwitcher>();
            Light2D[] allLights = FindObjectsOfType<Light2D>();
            foreach(RoomSwitcher room in allRooms)
            {
                room.lightIntensity = 1;
                room.lighting = false;
            }
            foreach (Light2D light in allLights)
            {
                light.gameObject.SetActive(false);
            }
            globalLight.gameObject.SetActive(true);
            globalLight.intensity = 1;
        }
    }

    
}
