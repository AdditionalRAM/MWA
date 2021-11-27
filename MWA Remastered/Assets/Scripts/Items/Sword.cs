using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Item, IUsableItem
{
    Animator an;
    public float animationDuration;
    public bool reverseAnim;

    private void Awake()
    {
        an = GetComponentInChildren<Animator>();
    }

    public void OnUse()
    {
        StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        canAttack = true;
        animating = true;
        reverseAnim = Random.value > 0.5f;
        an.SetTrigger("Swing");
        an.SetBool("reverse", reverseAnim);
        yield return new WaitForSeconds(animationDuration);
        animating = false;
        canAttack = false;
    }

}
