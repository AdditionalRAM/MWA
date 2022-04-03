using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item, IUsableItem
{
    Animator an;
    public float delayDuration, swingDuration, cooldownDuration;
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
        reverseAnim = Random.value > 0.5f;
        an.SetTrigger("Swing");
        an.SetBool("reverse", reverseAnim);
        yield return new WaitForSeconds(delayDuration);
        canAttack = true;
        animating = true;
        useSound.Play();
        yield return new WaitForSeconds(swingDuration);
        canAttack = false;
        yield return new WaitForSeconds(cooldownDuration);
        animating = false;
    }
}
