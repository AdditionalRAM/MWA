using UnityEngine;
using UnityEngine.UI;

public class UIReferences : MonoBehaviour
{
    public GameOverManager gameOverManger;
    //General
    public MultiPurposeButton interactButton, dialogOption1, dialogOption2;

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
}
