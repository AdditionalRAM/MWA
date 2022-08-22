using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;

public class EnemyAI : MonoBehaviour, IDamage
{
    public Transform target;
    public AudioSource hurtSound;
    public bool playHurtSound;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    public float kbCooldown;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool takingKB;
    public bool canMove = true;

    public Animator an;
    public SpriteRenderer sRenderer;
    Seeker seeker;
    public Rigidbody2D rb;


    public bool idle;
    public float health;
    public float meleeDamage;
    public float kbTime, kbThrust;
    public Color damageColor;
    public Color normalColor, explosionColor;
    public GameObject[] loot;
    public GameObject deathBum;
    public int[] lootChances, lootAmounts;
    public Dictionary<GameObject, int> lootAndChances = new Dictionary<GameObject, int>();
    public Dictionary<GameObject, int> lootAndAmounts = new Dictionary<GameObject, int>();
    public Vector3 lootRangeTop, lootRangeBottom;

    private void Awake()
    {
        if (playHurtSound) hurtSound = GetComponent<AudioSource>();
        if(loot.Length > 0)
        for (int i = 0; i < loot.Length; i++)
        {
            lootAndChances.Add(loot[i], lootChances[i]);
            lootAndAmounts.Add(loot[i], lootAmounts[i]);
        }
    }

    public void Damaged(float dmg)
    {
        health -= dmg;
        if (hurtSound != null) hurtSound.Play();
        if (health <= 0)
        {
            if (loot.Length > 0)
            {
                DropLoot();
            }
            CreateDeathBum();
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(DamageTint());
        }
    }

    public void TakeKB(float time, float thrust, Vector3 otherPos)
    {
        Vector3 diff = transform.position - otherPos;
        diff = diff.normalized * thrust;
        Vector2 targetPos = new Vector2(transform.position.x + diff.x, transform.position.y + diff.y);
        if (rb != null)
        {
            takingKB = true;
            rb.DOMove(targetPos, time).OnComplete(DoneKB);
        }
    }

    IEnumerator KBCooldown()
    {
        yield return new WaitForSeconds(kbCooldown);
        takingKB = false;
    }

    public void DoneKB()
    {
        StartCoroutine(KBCooldown());
    }

    IEnumerator DamageTint()
    {
        sRenderer.color = damageColor;
        yield return new WaitForSecondsRealtime(.2f);
        sRenderer.color = normalColor;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<IDamage>() != null && other.isTrigger)
        {
            other.GetComponent<IDamage>().Damaged(meleeDamage);
            other.GetComponent<IDamage>().TakeKB(kbTime, kbThrust, transform.position);
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && !idle && target != null)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

    }

    /*private void FixedUpdate()
    {
        if (path == null) 
        {
            return; 
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        float disToPlayer = Vector2.Distance(rb.position, target.position);
        if (disToPlayer <= 5)
        {
            rb.drag = 1;
        }
        else rb.drag = 2f;

        if (!idle && !takingKB) {
            rb.MovePosition(rb.position + force);
            //transform.Translate(force);
            //rb.MovePosition(force + rb.position);
            //iTween.MoveTo(gameObject, force, tweenSpeed);
            //rb.DOMove(force, tweenSpeed);
            //rb.AddForce(force);
            //rb.AddRelativeForce(force, ForceMode2D.Impulse);
            //rb.velocity = force;
            //rb.MovePosition(path.vectorPath[currentWaypoint]); 
            //transform.position = Vector3.MoveTowards(transform.position, path.vectorPath[currentWaypoint], speed*Time.deltaTime);
            //rb.velocity = Vector3.MoveTowards(transform.position, path.vectorPath[currentWaypoint], speed * Time.deltaTime) - transform.position;
            //rb.MovePosition(Vector3.MoveTowards(transform.position, path.vectorPath[currentWaypoint], speed * Time.deltaTime));
            an.SetBool("walking", true);
        }
        else { rb.velocity = Vector2.zero; an.SetBool("walking", false); }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        Vector3 dirToPlayer = target.position - transform.position;
        if (dirToPlayer != Vector3.zero)
        {
            if (dirToPlayer.x >= 0.1f) sRenderer.flipX = false;
            else if (dirToPlayer.x <= 0.1f) sRenderer.flipX = true;
        }
    }*/

    private void FixedUpdate()
    {
        if (path == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;

        if (!idle && !takingKB && canMove) {
            rb.MovePosition(transform.position += velocity * Time.deltaTime);
            Vector3 dirToPlayer = target.position - transform.position;
            if (dirToPlayer != Vector3.zero)
            {
                if (dirToPlayer.x >= 0.1f) sRenderer.flipX = false;
                else if (dirToPlayer.x <= 0.1f) sRenderer.flipX = true;
            }
            an.SetBool("walking", true);
        }else an.SetBool("walking", false);

        // If you are writing a 2D game you should remove the CharacterController code above and instead move the transform directly by uncommenting the next line
        // transform.position += velocity * Time.deltaTime;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        else
        {
            Debug.Log(p.errorLog);
        }
    }

    public void DropLoot()
    {
        if(lootRangeBottom != Vector3.zero && lootRangeTop != Vector3.zero)
        {
            foreach (GameObject lootItem in loot)
            {
                float coordX = Random.Range(lootRangeBottom.x * 10, lootRangeTop.x * 10) / 10;
                float coordY = Random.Range(lootRangeBottom.y * 10, lootRangeTop.y * 10) / 10;
                bool spawnItem = Random.Range(0, 100) < lootAndChances[lootItem];
                if (spawnItem)
                {
                    GameObject currentLoot = Instantiate(lootItem, transform.position + new Vector3(coordX, coordY, 0), Quaternion.identity);
                    currentLoot.GetComponent<DroppedItem>().itemAmount = lootAndAmounts[lootItem];
                }
            }
        }
        else
        {
            foreach (GameObject lootItem in loot)
            {
                bool spawnItem = Random.Range(0, 100) <= lootAndChances[lootItem];
                if (spawnItem)
                {
                    GameObject currentLoot = Instantiate(lootItem, transform.position, Quaternion.identity);
                    currentLoot.GetComponent<DroppedItem>().itemAmount = lootAndAmounts[lootItem];
                }
            }
        }
    }

    public void CreateDeathBum()
    {
        GameObject _deathBum = Instantiate(deathBum, transform.position, transform.rotation);
        _deathBum.GetComponent<ParticleSystem>().startColor = explosionColor;
        _deathBum.GetComponent<AudioSource>().Play();
        Destroy(_deathBum, 1f);
    }
}
