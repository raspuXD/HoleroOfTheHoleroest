using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextDay : MonoBehaviour
{
    public Button nextDayButton;

    private void Start()
    {
        // Ensure the button has a listener attached to it
        nextDayButton.onClick.AddListener(OnNextDayButtonClicked);
    }

    // Method that gets called when the button is clicked
    void OnNextDayButtonClicked()
    {
        // Get the current active scene's name
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Hotel1")
        {
            // Load Hotel2 if the current scene is Hotel1
            SceneManager.LoadScene("Hotel2");
        }
        else
        {
            // Load WinMenu if the current scene is not Hotel1
            SceneManager.LoadScene("WinMenu");
        }
    }
}
