using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GiveDamageOnTrigger : MonoBehaviour
{
    public int damage;
    public float kbTime, kbThrust;

    public UnityEvent onCollideWithPlayer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamage>().Damaged(damage);
            other.gameObject.GetComponent<IDamage>().TakeKB(kbTime, kbThrust, transform.position);
            if(onCollideWithPlayer != null)onCollideWithPlayer.Invoke();
        }
    }
}
