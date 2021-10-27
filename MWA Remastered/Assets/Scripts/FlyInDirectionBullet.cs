using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyInDirectionBullet : MonoBehaviour
{
    Rigidbody2D rb;

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
}
