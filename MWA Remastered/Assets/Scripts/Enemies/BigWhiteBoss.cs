using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWhiteBoss : EnemyAI
{
    public enum BigWhiteBossState
    {
        Wait, SwordAttack, BallAttack, Charge
    }

    UIReferences ui;

    public bool localizeOnAwake;

    public int angerHealth;
    public Color angerColor;
    public string bossTitle;
    public BigWhiteBossState state;
    public Item mySword;
    public BlueOrbController orb;
    public Transform crosshair, player;
    public GameObject archers;
    public BreakableObject table;
    public float normalSpeed;
    public float ballCooldown, swordCooldown, chargeCooldown, waitCooldown, moveCooldown;
    public bool canCoroutine, receivedEndMsg, angry, moveCrosshair = true;
    public int attackCount;

    public AudioClip ambience, bossMusic;
    public RoomSwitcher myRoom;
    public GameObject enableOnDeath;

    public int bossID, dungeonID;

    private void OnEnable()
    {
        bossTitle = LocalManager.Localize(bossTitle);
        ui = FindObjectOfType<UIReferences>();
        CreateDeathBum();
        state = BigWhiteBossState.Wait;
        player = FindObjectOfType<PlayerMovement>().transform;
        target = player;
        normalSpeed = speed;
        an.SetBool("eyes", false);
        an.SetTrigger("statechange");
        archers.SetActive(true);
        StartCoroutine(WaitCo());
        ui.bossTitle.text = bossTitle;
        ui.bossBar.gameObject.SetActive(true);
        ui.bossBar.SetMaxValue(health, true);
        table.CommitDie();
        myRoom.myMusic = bossMusic;
        myRoom.UpdateMusic();
        if (SaveGame.bossesKilled[bossID]) Debug.Log("Hey I shouldnt be alive rn");
    }

    IEnumerator WaitCo()
    {
        yield return new WaitForSeconds(waitCooldown);
        SelectRandomAttackState();
    }

    private void Update()
    {
        ui.bossBar.SetValue(health);
        if (!angry)
        {
            if(health <= angerHealth)
            {
                angry = true;
                normalColor = angerColor;
                sRenderer.color = normalColor;
                ballCooldown /= 2;
                swordCooldown /= 2;
                chargeCooldown /= 2;
                waitCooldown /= 2;
                moveCooldown *= 1.2f;
                speed *= 1.5f;
                normalSpeed = speed;
            }
        }

        if(attackCount >= 3)
        {
            attackCount = 0;
            state = BigWhiteBossState.Wait;
            StartCoroutine(WaitCo());
        }

        if (target != null && moveCrosshair) { crosshair.position = target.position; RotateItem(); }

        if (state == BigWhiteBossState.Wait)
        {
            idle = true;
        }
        else idle = false;

        if(state == BigWhiteBossState.SwordAttack && canCoroutine)
        {
            StartCoroutine(SwordAttackCo());
        }
        else if (state == BigWhiteBossState.BallAttack && canCoroutine)
        {
            StartCoroutine(BallAttackCo());
        }else if(state == BigWhiteBossState.Charge && canCoroutine)
        {
            StartCoroutine(ChargeAttackCo());
        }
    }

    public void EndMessage()
    {
        receivedEndMsg = true;
    }

    private void OnDestroy()
    {
        archers.SetActive(false);
        ui.bossBar.gameObject.SetActive(false);
    }
    public bool ReceivedEndMessage()
    {
        return receivedEndMsg;
    }

    public bool GotToCrosshair()
    {
        return Vector3.Distance(transform.position, crosshair.position) < 1.5f;
    }

    public void SelectRandomAttackState()
    {
        mySword.gameObject.SetActive(false);
        orb.gameObject.SetActive(false);
        state = (BigWhiteBossState)Random.Range(1, 4);
    }

    public void RotateItem()
    {
        if (crosshair != null && mySword != null)
        {
            Vector3 dir = crosshair.transform.position - mySword.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            mySword.transform.rotation = Quaternion.AngleAxis(angle + mySword.rotateOffset, mySword.transform.forward);
            SpriteRenderer susRenderer;
            if (mySword.GetComponent<SpriteRenderer>() != null) susRenderer = mySword.GetComponent<SpriteRenderer>();
            else susRenderer = mySword.GetComponentInChildren<SpriteRenderer>();
            if (angle < 90 && angle > -90 && mySword.flipSprite)
            {
                susRenderer.flipX = false;
            }
            else if (mySword.flipSprite)
            {
                susRenderer.flipX = true;
            }
            if (mySword.GetComponent<IOnRotateItem>() != null) mySword.GetComponent<IOnRotateItem>().OnRotate();
        }
    }

    IEnumerator SwordAttackCo()
    {
        canCoroutine = false;
        mySword.gameObject.SetActive(true);
        yield return new WaitForSeconds(moveCooldown);
        canMove = false;
        yield return new WaitForSeconds(swordCooldown);
        mySword.Use();
        yield return new WaitUntil(ReceivedEndMessage);
        receivedEndMsg = false;
        yield return new WaitForSeconds(swordCooldown/2);
        canMove = true;
        mySword.gameObject.SetActive(false);
        canCoroutine = true;
        attackCount++;
    }

    IEnumerator BallAttackCo()
    {
        canCoroutine = false;
        orb.gameObject.SetActive(true);
        an.SetBool("eyes", true);
        an.SetTrigger("statechange");
        yield return new WaitForSeconds(moveCooldown);
        canMove = false;
        yield return new WaitForSeconds(ballCooldown);
        orb.aiming = false;
        yield return new WaitUntil(ReceivedEndMessage);
        receivedEndMsg = false;
        yield return new WaitForSeconds(ballCooldown/2);
        canMove = true;
        orb.gameObject.SetActive(false);
        an.SetBool("eyes", false);
        an.SetTrigger("statechange");
        canCoroutine = true;
        attackCount++;
    }

    IEnumerator ChargeAttackCo()
    {
        canCoroutine = false;
        yield return new WaitForSeconds(moveCooldown/1.5f);
        canMove = false;
        yield return new WaitForSeconds(chargeCooldown);
        moveCrosshair = false;
        canMove = true;
        speed = 3 * normalSpeed;
        kbCooldown = 0;
        target = crosshair;
        yield return new WaitUntil(GotToCrosshair);
        speed = normalSpeed;
        kbCooldown = 0.15f;
        canMove = false;
        target = player;
        yield return new WaitForSeconds(chargeCooldown);
        canMove = true;
        moveCrosshair = true;
        canCoroutine = true;
        attackCount++;
    }

    public void OnDeath()
    {
        myRoom.myMusic = ambience;
        myRoom.UpdateMusic();
        SaveGame.bossesKilled[bossID] = true;
        SaveGame.dungeonsComplete[dungeonID] = true;
        FindObjectOfType<CheckDungeonComplete>().OnAfterGameLoad();
        enableOnDeath.SetActive(true);
    }
}
