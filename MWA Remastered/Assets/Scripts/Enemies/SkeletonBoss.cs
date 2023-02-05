using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Pathfinding;
using UnityEngine.UI;

public class SkeletonBoss : MonoBehaviour, IDamage
{
    UIReferences ui;
    public bool localize;

    public AudioSource hurtSound, shitSound;

    public Transform target;

    public GameObject skelePrefab;

    Rigidbody2D rb;
    Seeker seeker;
    public Animator an;
    public BoneDispenser[] dispensers;
    public Transform[] skeleSpawners;

    public float speed = 200f;
    public float kbCooldown;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    public float kbTime, kbThrust, meleeDamage, health;
    public float enrageHealth, boneDelay;
    public SpriteRenderer sRenderer;
    public Color currentColor, normalColor, damageColor, enrageColor;
    bool takingKB;

    public string bossTitle;

    public string[] firstDialogs, deathDialogs;
    public float dialogDelay;

    public bool walk;
    public bool enraged;
    bool rip;

    public GameObject[] changeOnDeath;
    public List<GameObject> mySkeles = new List<GameObject>();

    public AudioClip sussyAmbience;
    public RoomSwitcher room;

    public GameObject[] loot;
    public Vector3 lootRangeTop, lootRangeBottom;

    public GameObject deathParticle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        ui = FindObjectOfType <UIReferences>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("UpdatePath", 0f, .5f);
        Localize();
        StartCoroutine(Dialog(firstDialogs, false));
        ui.bossTitle.text = bossTitle;
        ui.bossBar.gameObject.SetActive(true);
        ui.bossBar.SetMaxValue(health, true);
    }

    void Localize()
    {
        if (!localize) return;
        firstDialogs = LocalManager.LocalizeArray(firstDialogs);
        deathDialogs = LocalManager.LocalizeArray(deathDialogs);
        bossTitle = LocalManager.Localize(bossTitle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.isTrigger)
        {
            other.GetComponent<IDamage>().Damaged(meleeDamage);
            other.GetComponent<IDamage>().TakeKB(kbTime, kbThrust, transform.position);
        }
    }

    public void Damaged(float dmg)
    {
        hurtSound.Play();
        health -= dmg;
        ui.bossBar.SetValue(health);
        if (health <= enrageHealth && !enraged)
        {
            StartCoroutine(EnrageCo());
        }
        if(health <= 0)
        {
            rip = true;
            ChangeOnDeath();
            StartCoroutine(Dialog(deathDialogs, true));
        }
        else
        {
            StartCoroutine(DamageTint());
        }
    }

    public void TakeKB(System.Single time, float thrust, Vector3 otherPos)
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

    public void ThrowBones()
    {
        StartCoroutine(ThrowBoneCo());
    }

    public void SpawnSkele()
    {
        StartCoroutine(SpawnSkeleboi());
    }

    public IEnumerator EnrageCo()
    {
        enraged = true;
        walk = false;
        currentColor = enrageColor;
        sRenderer.color = currentColor;
        an.SetBool("shaking", true);
        yield return new WaitForSeconds(1f);
        InvokeRepeating("ThrowBones", 0, boneDelay);
        InvokeRepeating("SpawnSkele", 0, boneDelay * 2);
        an.SetBool("shaking", false);
        walk = true;
    }

    public IEnumerator SpawnSkeleboi()
    {
        if (!rip)
        {
            yield return new WaitForSeconds(.5f);
            foreach (Transform point in skeleSpawners)
            {
                GameObject _ = Instantiate(skelePrefab, point.position, point.rotation);
                mySkeles.Add(_);
            }
        }
    }

    public IEnumerator ThrowBoneCo()
    {
        if (!rip)
        {
            walk = false;
            yield return new WaitForSeconds(.5f);
            foreach (BoneDispenser disp in dispensers)
            {
                disp.FireBullet();
            }
            shitSound.Play();
            yield return new WaitForSeconds(.5f);
            walk = true;
        }
    }

    IEnumerator DamageTint()
    {
        sRenderer.color = damageColor;
        yield return new WaitForSecondsRealtime(.2f);
        sRenderer.color = currentColor;
    }


    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

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

        if (walk && !takingKB)
        {
            rb.velocity = velocity;
            //rb.MovePosition(transform.position += velocity * Time.deltaTime);
            Vector3 dirToPlayer = target.position - transform.position;
            if (dirToPlayer != Vector3.zero)
            {
                if (dirToPlayer.x >= 0.1f) sRenderer.flipX = false;
                else if (dirToPlayer.x <= 0.1f) sRenderer.flipX = true;
            }
        }
        else rb.velocity = Vector3.zero;
        an.SetBool("Walk", walk);

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
    }

    public void ChangeOnDeath()
    {
        SaveGame.bossesKilled[0] = true;
        walk = false;
        speed = 0;
        target.GetComponent<PlayerMovement>().currentBGMusic = sussyAmbience;
        room.myMusic = sussyAmbience;
        ui.bgMusicSource.Stop();
        ui.bgMusicSource.clip = sussyAmbience;
        ui.bgMusicSource.Play();
        foreach (GameObject skele in mySkeles.ToArray())
        {
            Destroy(skele);
        }
        foreach (GameObject thing in changeOnDeath)
        {
            thing.SetActive(!thing.activeInHierarchy);
        }
    }

    public void Death()
    {
        DropLoot();
        ui.bossBar.gameObject.SetActive(false);
        GameObject kartoffel = Instantiate(deathParticle, room.transform);
        kartoffel.transform.position = transform.position;
        Destroy(gameObject);
    }

    IEnumerator Dialog(string[] dialogs, bool returnAtEnd)
    {
        walk = false;
        target.GetComponent<PlayerMovement>().freeze = true;
        ui.susDialog.gameObject.SetActive(true);
        ui.susDialog.text = "";
        foreach (string dialog in dialogs)
        {     
            foreach (char key in dialog.ToCharArray())
            {
                yield return new WaitForSeconds(dialogDelay);
                ui.susDialog.text += key;
            }
            yield return new WaitForSeconds(1f);
            ui.susDialog.text = "";
        }
        target.GetComponent<PlayerMovement>().freeze = false;
        target.GetComponent<PlayerMovement>().iFrames = false;
        ui.susDialog.gameObject.SetActive(false);
        if(!rip)
        walk = true;
        if (returnAtEnd) Death();
    }

    void DropLoot()
    {
        foreach (GameObject lootItem in loot)
        {
            float coordX = Random.Range(lootRangeBottom.x * 10, lootRangeTop.x * 10) / 10;
            float coordY = Random.Range(lootRangeBottom.y * 10, lootRangeTop.y * 10) / 10;
            GameObject currentLoot = Instantiate(lootItem, transform.position + new Vector3(coordX, coordY, 0), Quaternion.identity);     
        }
    }
}
