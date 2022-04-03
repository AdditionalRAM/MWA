using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerSetActive : MonoBehaviour
{
    public GameObject obj;
    public string triggerTag;
    public bool destroyMe, deactivateObj;
    public float deactivateObjDelay;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(triggerTag == "" || other.CompareTag(triggerTag))
        {
            obj.SetActive(true);
            if (deactivateObj) StartCoroutine(DeactivateObjDelay());
            if (destroyMe) Destroy(gameObject);
        }
    }

    IEnumerator DeactivateObjDelay()
    {
        yield return new WaitForSeconds(deactivateObjDelay);
        obj.SetActive(false);
    }
}
