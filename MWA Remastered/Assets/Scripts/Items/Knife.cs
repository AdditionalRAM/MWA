using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Item, IUsableItem
{
    Animator an;
    public float anMovement, anSpeed;

    private void Awake()
    {
        an = GetComponent<Animator>();
    }

    public void OnUse()
    {
        StartCoroutine(StabAnimation());
    }

    IEnumerator StabAnimation()
    {
        animating = true;
        canAttack = true;
        Vector3 oldPos = transform.position;
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / anSpeed;
            transform.position = Vector3.Lerp(transform.position, crosshair.transform.position, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / anSpeed;
            transform.position = Vector3.Lerp(transform.position, owner.transform.position + defaultPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        animating = false;
        canAttack = false;
    }
}
