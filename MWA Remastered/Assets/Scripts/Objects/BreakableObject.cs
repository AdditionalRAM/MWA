using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, ISelectiveDamage
{
    public bool animate;
    public int breakState;
    Animator an;
    public float health, maxHealth;
    public GameObject breakBum;
    public Color breakColor;
    public AudioSource damageSound;
    public ItemObject requiredTool;

    private void Awake()
    {
        if (animate) an = GetComponent<Animator>();
        health = maxHealth;
    }

    public void Damaged(float dmg, ItemObject usedItem)
    {
        if (requiredTool != null) if (usedItem != requiredTool) return;
        if (damageSound != null) damageSound.Play();
        health -= dmg;
        if (animate) AnimateBreaking();
        if(health <= 0)
        {
            GameObject _deathBum = Instantiate(breakBum, transform.position, transform.rotation);
            _deathBum.GetComponent<ParticleSystem>().startColor = breakColor;
            _deathBum.GetComponent<AudioSource>().Play();
            Destroy(_deathBum, 1f);
            Destroy(gameObject);
        }
    }

    public void AnimateBreaking()
    {
        breakState = GetBreakingLevel();
        an.SetInteger("BreakLevel", breakState);
    }

    public int GetBreakingLevel()
    {
        if (health == maxHealth) return 1;
        else if (health > (maxHealth / 2)) return 2;
        else if (health <= (maxHealth / 2)) return 3;
        return 0;
    }
}
