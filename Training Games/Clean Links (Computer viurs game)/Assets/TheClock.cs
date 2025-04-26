using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TheClock : MonoBehaviour
{
    public TMP_Text timeText;  // Assign your TMP text object in the Inspector
    private bool isCountingDown = false;
    private float countdownDuration = 180f; // Default is 3 minutes (in seconds)
    private float remainingTime;

    void Start()
    {
        // Start showing the current time
        InvokeRepeating(nameof(UpdateCurrentTime), 0f, 1f);
    }

    void UpdateCurrentTime()
    {
        if (isCountingDown) return;

        System.DateTime now = System.DateTime.Now;
        string amPm = now.Hour >= 12 ? "PM" : "AM";
        int hour = now.Hour % 12;
        if (hour == 0) hour = 12;  // Handle midnight and noon
        timeText.text = $"{hour}:{now.Minute:D2} {amPm}";
        timeText.color = Color.green;
        AudioManager.Instance.PlaySFX("Clock");
    }

    public void StartCountdown(float duration = 180f)
    {
        countdownDuration = duration; // Update the countdown duration if provided
        isCountingDown = true;
        remainingTime = countdownDuration;
        CancelInvoke(nameof(UpdateCurrentTime)); // Stop updating the current time
        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        while (remainingTime > 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timeText.text = $"{minutes}:{seconds:D2}";
            timeText.color = Color.red;

            if (Mathf.FloorToInt(remainingTime) != Mathf.FloorToInt(remainingTime - Time.deltaTime))  // Check if a second has passed
            {
                AudioManager.Instance.PlaySFX("Clock");
            }

            remainingTime -= Time.deltaTime;
            yield return null;
        }

        timeText.text = "Time's Up!";
        SceneHandler scenen = FindObjectOfType<SceneHandler>();
        scenen.LoadSceneNamed("gameOver");
        isCountingDown = false;
    }


    public void StopCountdown()
    {
        StopAllCoroutines();
        isCountingDown = false;
        timeText.text = "Countdown Stopped";
        InvokeRepeating(nameof(UpdateCurrentTime), 0f, 1f); // Resume showing the current time
    }
}
