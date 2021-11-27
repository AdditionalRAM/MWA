using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableImpostorBox : MonoBehaviour
{
    public Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    public void ResetPos()
    {
        transform.position = startPos;
    }
}
