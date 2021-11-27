using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpostorBoxPuzzle : MonoBehaviour
{
    public ImpostorBox[] boxes;
    public PushableImpostorBox[] pushBoxes;
    public Sprite monsterSprite;
    public GameObject activateAfterPuzzle;

    public float boxImpostDelay;
    bool puzzleComplete;

    private void Update()
    {
        if (AllAreIn() && !puzzleComplete) TurnIntoImpostor();
    }

    public bool AllAreIn()
    {
        foreach (ImpostorBox box in boxes)
        {
            if (!box.boxIn)
            {
                return false;
            }
        }
        return true;
    }

    public void TurnIntoImpostor()
    {
        if (!AllAreIn()) return;
        foreach (ImpostorBox box in boxes)
        {
            box.myBox.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = monsterSprite;
            box.myBox.GetComponent<Rigidbody2D>().mass = 10000;
        }
        Invoke("SpawnEnemies", boxImpostDelay);
        puzzleComplete = true;
    }

    public void SpawnEnemies()
    {
        foreach (ImpostorBox box in boxes)
        {
            box.myBox.GetComponent<EnemySpawner>().SpawnEnemy();
        }
        activateAfterPuzzle.SetActive(!activateAfterPuzzle.activeInHierarchy);
    }

    public void ResetPuzzle()
    {
        Debug.Log("Reset puzzle called");
        if (!AllAreIn())
        {
            for (int i = 0; i < pushBoxes.Length; i++)
            {
                pushBoxes[i].ResetPos();
            }
        }
    }
}
