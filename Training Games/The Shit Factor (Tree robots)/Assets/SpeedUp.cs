using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public GameObject[] prevent;
    public KeyCode pauseKey = KeyCode.Space;
    public TMP_Text theTes;

    private float[] timeScales = { 1f, 2f, 3f };  // The time scales to toggle through

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }

        theTes.text = Time.timeScale.ToString("F0") + "X";  // Display the current time scale
    }

    public void TogglePause()
    {
        if (AnyPreventingObjectActive()) return;

        // Check the current time scale and increment it by 1, cycling back to 1 if necessary
        float currentTimeScale = Time.timeScale;

        // Find the next time scale in the array
        for (int i = 0; i < timeScales.Length; i++)
        {
            if (timeScales[i] == currentTimeScale)
            {
                int nextIndex = (i + 1) % timeScales.Length;
                Time.timeScale = timeScales[nextIndex];
                return;
            }
        }

        // If current time scale is not found in the array, default to 1
        Time.timeScale = timeScales[0];
    }

    private bool AnyPreventingObjectActive()
    {
        for (int i = 0; i < prevent.Length; i++)
        {
            if (prevent[i] != null && prevent[i].activeInHierarchy)
                return true;
        }
        return false;
    }
}
