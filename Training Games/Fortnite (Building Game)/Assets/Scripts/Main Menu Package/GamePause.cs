using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePause : MonoBehaviour
{
    [Header("Pause Settings")]
    public KeyCode pauseKey = KeyCode.Escape;
    public GameObject thePauseCanvas;
    public GameObject theOptions;
    private bool isPaused = false;

    [Header("Scale visually")]
    public bool doesScaleWhenOptions = true;
    public GameObject theScaleObject;
    public float howFast = .4f;

    public GameObject[] objectsThatPreventPausing;

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    public void OpenTheSettings()
    {
        theOptions.SetActive(true);
        if (doesScaleWhenOptions && theScaleObject != null)
        {
            StartCoroutine(ScaleObject(theScaleObject.transform, Vector3.zero, Vector3.one, howFast));
        }
    }

    public void TogglePause()
    {
        if (!isPaused && AnyPreventingObjectActive()) return;

        theOptions.SetActive(false);
        isPaused = !isPaused;
        thePauseCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;

        if (doesScaleWhenOptions && theScaleObject != null)
        {
            StartCoroutine(ScaleObject(theScaleObject.transform, theScaleObject.transform.localScale, isPaused ? Vector3.zero : Vector3.one, howFast));
        }
    }

    private bool AnyPreventingObjectActive()
    {
        for (int i = 0; i < objectsThatPreventPausing.Length; i++)
        {
            if (objectsThatPreventPausing[i] != null && objectsThatPreventPausing[i].activeInHierarchy)
                return true;
        }
        return false;
    }

    private IEnumerator ScaleObject(Transform target, Vector3 fromScale, Vector3 toScale, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(fromScale, toScale, elapsed / duration);
            yield return null;
        }

        target.localScale = toScale;
    }
}
