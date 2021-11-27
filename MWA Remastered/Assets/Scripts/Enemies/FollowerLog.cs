using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FollowerLog : Enemy
{
    public Animator an;

    private void Awake()
    {
        //aipath = GetComponent<AIPath>();
    }

    public void Update()
    {
        /*if(aipath.desiredVelocity.x >= 0.01f)
        {
            sRenderer.flipX = false;
            an.SetBool("walking", true);
        } 
        else if (aipath.desiredVelocity.x <= -0.01f)
        {
            sRenderer.flipX = true;
            an.SetBool("walking", true);
        }
        else
        {
            an.SetBool("walking", false);
        }*/
    }
}
