using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public bool usesAudio = true;
    public float whenTo = 5f;

    Coroutine theDestroy;

    private void Start()
    {
        if(usesAudio)
        {
            AudioManager.Instance.PlaySFX("Vam");
        }

        theDestroy = StartCoroutine(DestroyMUHAHAH(whenTo));
    }

    IEnumerator DestroyMUHAHAH(float howFast)
    {
        yield return new WaitForSeconds(howFast);
        Destroy(gameObject);
    }

    public void RefuseForNow(float forHowLong)
    {
        Debug.Log("Huvä");

        if(theDestroy != null)
        {
            StopCoroutine(theDestroy);
            Debug.Log("Parempi");
        }

        theDestroy = StartCoroutine(DestroyMUHAHAH(forHowLong));
    }
}
