using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAI
{
    public Item selectedItem;
    public float attackRange;
    public float attackCooldown, attackDelay;
    public Transform crosshair;
    public bool attacking;

    private void Update()
    {
        if (!idle && selectedItem != null && target != null)
        {
            crosshair.transform.localPosition = (transform.position - target.position).normalized * selectedItem.range * -1;
            selectedItem.owner = gameObject;
            selectedItem.crosshair = crosshair;
            if (!selectedItem.useDuringAnim && !selectedItem.animating)
            {
                RotateItem();
            }
            else if (selectedItem.useDuringAnim)
            {
                RotateItem();
            }
            if (InAttackRange())
            {
                if (!attacking) StartCoroutine(Attack());
            }
            else
            {
                if(!attacking)canMove = true;
            }
        }
    }

    public bool InAttackRange()
    {
        if (target == null) return false;
        return (transform.position - target.position).magnitude <= attackRange;
    }

    public void RotateItem()
    {
        if (crosshair != null && selectedItem != null)
        {
            Vector3 dir = crosshair.transform.position - selectedItem.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            selectedItem.transform.rotation = Quaternion.AngleAxis(angle + selectedItem.rotateOffset, selectedItem.transform.forward);
            SpriteRenderer susRenderer;
            if (selectedItem.GetComponent<SpriteRenderer>() != null) susRenderer = selectedItem.GetComponent<SpriteRenderer>();
            else susRenderer = selectedItem.GetComponentInChildren<SpriteRenderer>();
            if (angle < 90 && angle > -90 && selectedItem.flipSprite)
            {
                susRenderer.flipX = false;
            }
            else if (selectedItem.flipSprite)
            {
                susRenderer.flipX = true;
            }
            if (InAttackRange() && selectedItem.GetComponent<IOnRotateItem>() != null) selectedItem.GetComponent<IOnRotateItem>().OnRotate();
        }
    }

    void UseWeapon()
    {
        if (selectedItem != null)
        {
            if (!selectedItem.useDuringAnim && !selectedItem.animating)
            {
                selectedItem.Use();
            }
            else if (selectedItem.useDuringAnim)
            {
                selectedItem.Use();
            }
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        canMove = false;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(attackDelay);
        UseWeapon();
        yield return new WaitForSeconds(attackCooldown);
        canMove = true;
        attacking = false;
    }
}
