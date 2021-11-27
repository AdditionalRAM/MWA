using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneDispenser : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Vector3 bulletPos, boneDir;
    public FlyInDirectionBullet myBullet;
    public float speed, ignoreTime, spawnDelay;
    public bool fireOnAwake = true;

    private void Start()
    {
        if(fireOnAwake)
        InvokeRepeating("FireBullet", 0, spawnDelay);
    }

    public void FireBullet()
    {
        if (!gameObject.activeInHierarchy) return;
        GameObject _myBullet = Instantiate(bulletPrefab, transform.position + bulletPos, Quaternion.identity);
        myBullet = _myBullet.GetComponent<FlyInDirectionBullet>();
        myBullet.speed = speed;
        myBullet.direction = boneDir;
        myBullet.canCollide = false;
        myBullet.Invoke("CollisionIgnoreFix", ignoreTime);
    }
}
