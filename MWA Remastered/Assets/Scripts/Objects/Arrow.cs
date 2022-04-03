using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, ISelectiveDamage
{
    Rigidbody2D rb;

    public AudioSource aS;
    public GameObject owner, arrowBum;
    public float speed;
    public float damage;
    public float kbTime, kbThrust;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject != owner && !other.isTrigger && !other.CompareTag("ArrowGoThru"))
        {
            if(other.GetComponent<IDamage>() != null)
            {
                other.gameObject.GetComponent<IDamage>().Damaged(damage);
                other.gameObject.GetComponent<IDamage>().TakeKB(kbTime, kbThrust, transform.position);
                aS.Play();
            }else
            {
                GameObject _arrowBum = Instantiate(arrowBum, transform.position, transform.rotation);
                Destroy(_arrowBum, 2f);
            }
            Destroy(gameObject);
        }
    }

    public void Damaged(float dmg, ItemObject item)
    {
        GameObject _arrowBum = Instantiate(arrowBum, transform.position, transform.rotation);
        Destroy(_arrowBum, 2f);
        Destroy(gameObject);
    }
}
