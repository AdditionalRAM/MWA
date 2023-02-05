using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyInDirectionBullet : MonoBehaviour, IDamage
{
    Rigidbody2D rb;
    public GameObject bum;
    public Color bumColor;

    public Vector3 direction;
    public float speed;
    public int damage;
    public LayerMask obstacle;

    public bool canCollide;

    public float kbTime, kbThrust;

    public void CollisionIgnoreFix()
    {
        canCollide = true;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        rb.velocity = direction.normalized * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (canCollide)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<IDamage>().Damaged(damage);
                other.gameObject.GetComponent<IDamage>().TakeKB(kbTime, kbThrust, transform.position);
            }
            Destroy(gameObject);
        }
    }

    public void Damaged(float dmg)
    {
        Bum();
    }

    public void TakeKB(float kbtime, float thrust, Vector3 otherPos)
    {
        
    }

    public void Bum()
    {
        GameObject _ = Instantiate(bum, transform.position, Quaternion.identity);
        _.GetComponent<ParticleSystem>().startColor = bumColor;
        Destroy(_, 2f);
        Destroy(gameObject);
    }
}
