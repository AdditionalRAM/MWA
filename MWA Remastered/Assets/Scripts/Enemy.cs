using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;

public class Enemy : MonoBehaviour, IDamage
{
    //legacy enemy
    Rigidbody2D rb;
    public AIPath aipath;
    public AIDestinationSetter aids;
    public float health;
    public float meleeDamage;
    public float kbTime, kbThrust;
    public SpriteRenderer sRenderer;
    public Color damageColor;
    public Color normalColor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        aipath = GetComponent<AIPath>();
    }

    public void Damaged(float dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(DamageTint());
        }
    }

    public void TakeKB(System.Single time, float thrust, Vector3 otherPos)
    {
        aipath.enabled = false;
        aids.enabled = false;
        Vector3 diff = transform.position - otherPos;
        diff = diff.normalized * thrust;
        Vector2 targetPos = new Vector2(transform.position.x + diff.x, transform.position.y + diff.y);
        //iTween.MoveTo(gameObject, iTween.Hash("position", targetPos, "time", time, "easetype", iTween.EaseType.easeInOutSine));
        if (rb != null)
        {
            //rb.DOMove(targetPos, time);
            //Debug.Log(time + "    " + thrust + "    " + otherPos + "     " + diff);
            rb.AddForce(diff);
        }
        //transform.DOMove(targetPos, time, false);
    }

    IEnumerator DamageTint()
    {
        sRenderer.color = damageColor;
        yield return new WaitForSecondsRealtime(.2f);
        aipath.enabled = true;
        aids.enabled = true;
        sRenderer.color = normalColor;
    }

    /*private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamage>().Damaged(meleeDamage);
            other.gameObject.GetComponent<IDamage>().TakeKB(kbTime, kbThrust, transform.position);
        }
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<IDamage>().Damaged(meleeDamage);
            other.GetComponent<IDamage>().TakeKB(kbTime, kbThrust, transform.position); 
        }
    }
}
