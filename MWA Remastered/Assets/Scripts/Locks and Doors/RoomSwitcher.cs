using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;

public class RoomSwitcher : MonoBehaviour, ILocalization
{
    public bool localize;

    UIReferences ui;
    public GameObject roomCam;

    public UnityEvent onRoomLoad, onRoomExit;

    public string placeName;
    public AudioClip myMusic;
    public bool cutsceneOnEnter = false, oneTimeChange = false;
    public GameObject[] changeOnEnter;
    public bool cutscenePlayed;
    public float lightIntensity, lightTweenDuration;
    public bool lighting;

    private void Awake()
    {
        ui = FindObjectOfType<UIReferences>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (onRoomLoad != null) onRoomLoad.Invoke();
            if (lighting)
            {
                other.GetComponent<PlayerMovement>().SetLighting(lightIntensity, lightTweenDuration);
            }
            roomCam.SetActive(true);
            if (cutsceneOnEnter) Cutscene();
            if(other.GetComponent<PlayerMovement>().currentPlaceName != placeName)
            {
                StartCoroutine(PlaceName(other.GetComponent<PlayerMovement>()));
            }
            if (other.GetComponent<PlayerMovement>().currentBGMusic != myMusic && myMusic != null)
            {
                other.GetComponent<PlayerMovement>().currentBGMusic = myMusic;
                ui.bgMusicSource.Stop();
                ui.bgMusicSource.clip = myMusic;
                ui.bgMusicSource.Play();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (onRoomExit != null) onRoomExit.Invoke();
            roomCam.SetActive(false);
        }
    }

    IEnumerator PlaceName(PlayerMovement player)
    {
        player.currentPlaceName = placeName;
        player.placeText.text = placeName;
        player.placeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        player.placeText.gameObject.SetActive(false);
    }

    public void Cutscene()
    {
        if (oneTimeChange && cutscenePlayed) return;
        foreach (GameObject thing in changeOnEnter)
        {
            thing.SetActive(!thing.activeInHierarchy);
        }
        cutscenePlayed = true;
    }

    public void OnLocalize()
    {
        if (!localize) return;
        placeName = LocalManager.Localize(placeName);
    }
}
