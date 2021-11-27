using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSceneSwitcher : MonoBehaviour
{
    public Vector3 posToSwitch;
    public PlayerMovement player;
    public float sceneTransitionDuration;
    UIReferences ui;

    private void Awake()
    {
        ui = FindObjectOfType<UIReferences>();
    }

    private void Update()
    {
        if(player != null && ui.interactButton.pressed)
        {
            ui.interactButton.pressed = false;
            StartCoroutine(FakeSceneTransition());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ui.interactButton.pressed = false;
            player = other.GetComponent<PlayerMovement>();
            ui.interactButton.gameObject.SetActive(true);
        }  
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ui.interactButton.pressed = false;
            player = null;
            ui.interactButton.gameObject.SetActive(false);
        }
    }

    IEnumerator FakeSceneTransition()
    {
        ui.transitionBlocker.SetActive(true);
        player.transform.position = posToSwitch;
        yield return new WaitForSeconds(sceneTransitionDuration);
        ui.transitionBlocker.SetActive(false);
    }
}
