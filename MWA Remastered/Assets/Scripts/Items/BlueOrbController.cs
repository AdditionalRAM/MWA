using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlueOrbController : MonoBehaviour
{
    public bool aiming = true;
    public Transform target, ball, currentTarget;
    public Transform[] keyFrames;
    public int midKeyFrame, currentKeyFrame = 0;
    public float keyDistanceX, keyDistanceY, ballSpeed;
    public Vector3 targetDistance;
    private Vector3 velocity;

    public UnityEvent onFinishAttack;

    public bool AboveTarget()
    {
        return transform.position.y > target.position.y;
    }


    public void Aiming()
    {
        currentKeyFrame = 0;
        if(keyFrames.Length >= 3)
        {
            keyFrames[midKeyFrame].position = target.position;
            keyFrames[midKeyFrame - 1].transform.position =
                new Vector3(target.position.x - keyDistanceX,
                target.position.y - keyDistanceY, 0);
            keyFrames[midKeyFrame + 1].transform.position =
                new Vector3(target.position.x + keyDistanceX,
                target.position.y - keyDistanceY, 0);
        }
        else
        {
            Debug.LogError("Blue Orb must have 3 key frames or more!");
        }
     
    }


    private void Update()
    {
        targetDistance = new Vector3(target.position.x - transform.position.x, target.position.y -
            transform.position.y, 0);
        keyDistanceX = targetDistance.x / 2;
        keyDistanceY = targetDistance.y / 2;
        if (aiming) Aiming();
        else Shooting();
    }

    public void Shooting()
    {
        float step = ballSpeed * Time.deltaTime;
        if (currentKeyFrame <= keyFrames.Length - 1)
        {
            currentTarget = keyFrames[currentKeyFrame];
        }
        else if (currentKeyFrame == keyFrames.Length)
            currentTarget = transform;
        else
        {
            currentKeyFrame = 0;
            if (onFinishAttack != null) onFinishAttack.Invoke();
            aiming = true;
        }
        ball.position = Vector3.MoveTowards(ball.position, currentTarget.position, step);
        if (Vector3.Distance(ball.position, currentTarget.position) < 0.1f)
        {
            currentKeyFrame++;
            
        }    
    }

    public void Shoot()
    {
        aiming = false;
    }

}
