using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Animator an;
    public bool spawnOnAwake = true;

    public GameObject enemyToSpawn;
    public GameObject spawnedEnemy;
    public GameObject gfx;
    bool onCooldown;

    public float spawnCooldown, spawnDelay;

    private void Awake()
    {
        if(spawnOnAwake)SpawnEnemy();
    }

    private void Update()
    {
        if(spawnedEnemy == null && !onCooldown)
        {
            Invoke("SpawnDelay", spawnCooldown);
            gfx.SetActive(true);
            onCooldown = true;
        }
    }

    void SpawnDelay()
    {
        Invoke("SpawnEnemy", spawnDelay);
        an.SetBool("spawning", true);
    }

    public void SpawnEnemy()
    {
        spawnedEnemy = Instantiate(enemyToSpawn, transform.position, transform.rotation);
        onCooldown = false;
        an.SetBool("spawning", false);
        gfx.SetActive(false);
    }
}
