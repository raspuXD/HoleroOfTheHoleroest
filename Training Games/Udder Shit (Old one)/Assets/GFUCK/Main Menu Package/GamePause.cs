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

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        theOptions.SetActive(false);
        isPaused = !isPaused;
        thePauseCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }
}
