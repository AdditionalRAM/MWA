using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public PlayerItem player;
    public float rotateOffset;
    public float range, damage;
    public ItemObject myItem;

    public float kbTime, kbThrust;
    public Vector3 defaultPos;

    public bool animating, useDuringAnim, canAttack, flipSprite;

    public enum ItemType{MeleeWeapon, RangedWeapon, Tool};

    public ItemType itemType;


    public void Use()
    {
        GetComponent<IUsableItem>().OnUse();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(itemType == ItemType.MeleeWeapon && canAttack && other.GetComponent<IDamage>() != null && other.gameObject != player.gameObject)
        {
            other.GetComponent<IDamage>().Damaged(damage);
            other.GetComponent<IDamage>().TakeKB(kbTime, kbThrust, transform.position);
            canAttack = false;
        }
    }
}
