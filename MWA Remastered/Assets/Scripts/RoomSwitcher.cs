using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSwitcher : MonoBehaviour
{
    UIReferences ui;
    public GameObject roomCam;

    public string placeName;
    public AudioClip myMusic;

    private void Awake()
    {
        ui = FindObjectOfType<UIReferences>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            roomCam.SetActive(true);
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
            roomCam.SetActive(false);
        }
    }

    void BGMusicTimer()
    {

    }

    IEnumerator PlaceName(PlayerMovement player)
    {
        player.currentPlaceName = placeName;
        player.placeText.text = placeName;
        player.placeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        player.placeText.gameObject.SetActive(false);
    }
}
