using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetSelector : MonoBehaviour
{
    public EnemyAI mama;
    List<Transform> players = new List<Transform>();
    public bool indicateAnger;
    public bool playerInSight, pWasInRange;
    public GameObject angerIndicator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            players.Add(other.transform);
            //Debug.Log(other.gameObject);
            if (indicateAnger) { angerIndicator.SetActive(true); angerIndicator.GetComponent<AudioSource>().Play(); }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            players.Remove(other.transform);
            if (players.Count < 1) { pWasInRange = false; playerInSight = false; }
        }
    }

    private void Update()
    {
        if (players.Count > 0 && !pWasInRange && !playerInSight)
        {
            Vector3 dir = (players[0].position - mama.transform.position).normalized;
            float distance = Vector3.Distance(players[0].position, mama.transform.position);
            LayerMask lyMask = UIReferences.instance.obstacleNoArrow;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distance, lyMask);
            if (hit.collider == null || hit.collider.CompareTag("ArrowGoThru")){ pWasInRange = true; playerInSight = true; }
        }
        if (playerInSight)
        {
            if (indicateAnger) angerIndicator.SetActive(true);
            mama.idle = false;
            float nearestDistance = 200f;
            foreach (Transform player in players)
            {
                float distance = Vector2.Distance(player.transform.position, mama.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    mama.target = player;
                }
            }
        }
        else
        {
            mama.idle = true;
            if(indicateAnger)angerIndicator.SetActive(false);
        }
    }
}
