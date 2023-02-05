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
    public int minimapRoomID = -1;
    public bool playerInside = false;
    public float pushAmount = .5f;

    private void Awake()
    {
        ui = FindObjectOfType<UIReferences>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player.currentRoom != null)
            {
                Vector3 dir = (player.GetComponent<Rigidbody2D>().velocity).normalized;
                player.GetComponent<Rigidbody2D>().MovePosition(player.transform.position + (DirectionTo4Way(dir) * pushAmount));
            }
            playerInside = true;
            if (onRoomLoad != null) onRoomLoad.Invoke();
            if (lighting)
            {
                player.SetLighting(lightIntensity, lightTweenDuration);
            }
            roomCam.SetActive(true);
            if (cutsceneOnEnter) Cutscene();
            player.currentRoomID = minimapRoomID;
            player.currentRoom = this;
            if (player.currentPlaceName != placeName)
            {
                StartCoroutine(PlaceName(player));
            }
            if (player.currentBGMusic != myMusic && myMusic != null)
            {
                player.currentBGMusic = myMusic;
                UpdateMusic();
            }
        }
    }

    public void UpdateMusic()
    {
        ui.bgMusicSource.Stop();
        ui.bgMusicSource.clip = myMusic;
        ui.bgMusicSource.Play();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            playerInside = false;
            if (onRoomExit != null) onRoomExit.Invoke();
            roomCam.SetActive(false);
        }
    }

    IEnumerator PlaceName(PlayerMovement player)
    {
        player.currentPlaceName = placeName;
        player.placeText.text = placeName;
        yield return null;
        /*player.placeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        player.placeText.gameObject.SetActive(false);*/
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

    public Vector3 DirectionTo4Way(Vector3 dir)
    {
        float absoluteX = Mathf.Abs(dir.x);
        float absoluteY = Mathf.Abs(dir.y);
        float newX = 0;
        float newY = 0;
        if (absoluteX > absoluteY)
        {
            //X
            if (dir.x > 0) newX = 1;
            else if (dir.x < 0) newX = -1;
        }
        else if (absoluteY > absoluteX)
        {
            if (dir.y > 0) newY = 1;
            else if (dir.y < 0) newY = -1;
        }
        else return dir;
        return new Vector3(newX, newY);
    }
}
