using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimStick : MonoBehaviour
{
    public Joystick stick;
    Vector3 input;

    public PlayerItem player;

    private void OnDisable()
    {
        input.x = stick.Horizontal;
        input.y = stick.Vertical;
        if (input.magnitude > 0.2f)
        {
            player.UseItem();
        }
    }
}
