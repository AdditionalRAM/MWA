using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpostorBox : MonoBehaviour
{
    public bool boxIn;
    public GameObject myBox;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SussyBox"))
        {
            boxIn = true;
            myBox = other.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("SussyBox"))
        {
            boxIn = false;
        }
    }
}
