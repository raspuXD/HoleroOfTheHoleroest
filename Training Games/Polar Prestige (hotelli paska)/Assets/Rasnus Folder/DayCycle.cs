using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DayCycle : MonoBehaviour
{
    [Header("Day Cycle Settings")]
    public float dayLengthInSeconds = 300f; // 5 minutes = 300 seconds
    private float currentTime = 0f; // Elapsed time
    private bool isDayOver = false; // Flag to check if day is over

    public  bool Day2 = false;

    [Header("UI Elements")]
    public TextMeshProUGUI dayTimerText; // TextMeshPro for displaying the timer
    public GameObject menu; // The menu to pop up at the end of the day (initially hidden)

    private void Start()
    {
        // Initially hide the menu
        if (menu != null)
        {
            menu.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isDayOver)
        {
            // Increase the time counter by the time passed each frame
            currentTime += Time.deltaTime;

            // Update the UI timer if there's a TextMeshProUGUI element
            if (dayTimerText != null)
            {
                float remainingTime = dayLengthInSeconds - currentTime;
                dayTimerText.text = $"Time Remaining: {remainingTime:F2}s";
            }

            // If the day ends (5 minutes have passed), trigger the event
            if (currentTime >= dayLengthInSeconds)
            {
                EndOfDay();
            }
        }
    }

    private void EndOfDay()
    {
        // Set the flag to true, indicating the day is over
        isDayOver = true;

        // Show the menu (or any event you want to trigger)
        if (menu != null && !Day2)
        {
            menu.SetActive(true); // Show the menu
        }
        else{
            SceneManager.LoadScene("winscene");
        }

        // Optionally, you can reset the time and restart the day cycle:
        // currentTime = 0f;
        // isDayOver = false;
    }
}
