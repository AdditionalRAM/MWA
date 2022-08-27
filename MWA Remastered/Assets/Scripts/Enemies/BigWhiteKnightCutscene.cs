using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWhiteKnightCutscene : EnemyAI
{
    public float stopDistance, farDistance;
    public float disFromPlayer;
    public GameObject triggerDialog, playerDipDialog, getTeaDialog, teaStartDialog;
    public Transform secondTarget;
    public PlayerMovement player;
    public bool hasApproachedPlayer, askedPlayerSeat;
    public GameObject falsePathStuff;
    public GameObject brother;


    private void Update()
    {
        disFromPlayer = Vector3.Distance(transform.position, target.position);
        float disToSecondTarget = Vector3.Distance(transform.position, secondTarget.position);
        if (disFromPlayer < stopDistance)
        {
            canMove = false;
            ApproachedPlayer();
        }
        if(disToSecondTarget < stopDistance && !askedPlayerSeat)
        {
            askedPlayerSeat = true;
            getTeaDialog.SetActive(true);
            getTeaDialog.transform.position = player.transform.position;
            teaStartDialog.SetActive(true);
        }
        if (hasApproachedPlayer)
        {
            target = secondTarget;
        }
        else target = player.transform;
        idle = false;
    }

    void ApproachedPlayer()
    {
        if (hasApproachedPlayer) { Destroy(triggerDialog); return; }
        hasApproachedPlayer = true;
        player.freeze = false;
        triggerDialog.SetActive(true);
        triggerDialog.transform.position = target.transform.position;
    }

    public void PlayerDipped()
    {
        if (askedPlayerSeat) return;
        canMove = false;
        playerDipDialog.SetActive(true);
        playerDipDialog.transform.position = player.transform.position;
    }

    public void PlayerCame()
    {
        StartCoroutine(PlayerCameCo());
    }

    IEnumerator PlayerCameCo()
    {
        if (hasApproachedPlayer && triggerDialog != null)
            Destroy(triggerDialog);
            yield return new WaitForSeconds(.7f);
        canMove = true;
    }

    public void CanMove(bool move)
    {
        canMove = move;
    }

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerMovement>();
        player.freeze = true;
        target = player.transform;
        falsePathStuff.SetActive(false);
    }

    public void DipSpawnBrother()
    {
        StartCoroutine(BrotherSpawnCo());
    }

    IEnumerator BrotherSpawnCo()
    {
        transform.localScale = Vector3.zero;
        CreateDeathBum();
        yield return new WaitForSeconds(0.7f);
        //transform.position = brother.transform.position;
        //CreateDeathBum();
        brother.SetActive(true);
        Destroy(gameObject);
    }
}
