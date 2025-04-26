using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockKnock : MonoBehaviour
{
    public float scaleUpDuration = 0.5f;
    public float waitDuration = 1.75f;
    public float scaleDownDuration = 0.5f;

    public void Activate()
    {
        transform.localScale = Vector3.zero;
        StopAllCoroutines();
        StartCoroutine(ScaleRoutine());
    }

    System.Collections.IEnumerator ScaleRoutine()
    {
        yield return StartCoroutine(ScaleTo(Vector3.one, scaleUpDuration));
        yield return new WaitForSeconds(waitDuration);
        yield return StartCoroutine(ScaleTo(Vector3.zero, scaleDownDuration));
    }

    System.Collections.IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
