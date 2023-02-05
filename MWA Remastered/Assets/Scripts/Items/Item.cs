using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Item : MonoBehaviour
{
    public GameObject owner;
    public float rotateOffset;
    public float range, damage;
    public ItemObject myItem;
    public AudioSource useSound;
    public bool autoPlaySound = true;
    public Transform crosshair;
    public bool use = true;

    public List<GameObject> attackedList = new List<GameObject>();
    public List<GameObject> triggerList = new List<GameObject>();

    public float kbTime, kbThrust;
    public Vector3 defaultPos;

    public bool animating, useDuringAnim, canAttack, flipSprite;

    public enum ItemType{MeleeWeapon, RangedWeapon, Tool};

    public ItemType itemType;

    public UnityEvent onFinishAttack;


    public void Use()
    {
        if (useSound != null && autoPlaySound) useSound.Play();
        if(use)
        attackedList.Clear();
        GetComponent<IUsableItem>().OnUse();
    }

    public void TriggerAttack()
    {
        if (!use) return;
        triggerList.RemoveAll(s => s == null);
        foreach (GameObject other in triggerList.ToArray())
        {
            if (itemType == ItemType.MeleeWeapon && canAttack && other.gameObject != owner
                && !attackedList.Contains(other.gameObject) && other.GetComponent<IDamage>() != null)
            {
                other.GetComponent<IDamage>().Damaged(damage);
                other.GetComponent<IDamage>().TakeKB(kbTime, kbThrust, transform.position);
                //canAttack = false;
                attackedList.Add(other.gameObject);
            }
            else if (itemType == ItemType.MeleeWeapon && canAttack && other.GetComponent<ISelectiveDamage>() != null && other.gameObject != owner
                && !attackedList.Contains(other.gameObject))
            {
                other.GetComponent<ISelectiveDamage>().Damaged(damage, myItem);
                //canAttack = false;
                attackedList.Add(other.gameObject);
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<IDamage>() != null || other.GetComponent<ISelectiveDamage>() != null)
        {
            triggerList.Add(other.gameObject);
            triggerList.RemoveAll(s => s == null);
        }
        TriggerAttack();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.GetComponent<IDamage>() != null)
        {
            triggerList.Remove(other.gameObject);
            triggerList.RemoveAll(s => s == null);
        }
    }
}
