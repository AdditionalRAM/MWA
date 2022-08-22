using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    public GameObject[] enableOnEnter, disableOnEnter;
    public GameObject[] enableOnExit, disableOnExit;

    public GameObject[] dList;

    public bool roomLoaded = false, roomCleared = false;

    public Vector3 moveP;

    public void OnEnter()
    {
        if (roomLoaded) return;
        EnableEnters();
    }

    public void OnRoomClear()
    {
        roomCleared = true;
        EnableExits();
    }

    private void Update()
    {
        if (roomLoaded && !roomCleared)
        {
            bool activeFound = false;
            foreach (GameObject dTing in dList)
            {
                if (dTing != null && dTing.activeInHierarchy) { activeFound = true; break; }
            }
            if (!activeFound) OnRoomClear();
        }
    }

    void EnableEnters()
    {
        foreach (GameObject obj in enableOnEnter)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in disableOnEnter)
        {
            obj.SetActive(false);
        }
        roomLoaded = true;
        FindObjectOfType<PlayerMovement>().transform.position += moveP;
    }

    void EnableExits()
    {
        foreach (GameObject obj in disableOnExit)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in enableOnExit)
        {
            obj.SetActive(true);
        }
    }
}
