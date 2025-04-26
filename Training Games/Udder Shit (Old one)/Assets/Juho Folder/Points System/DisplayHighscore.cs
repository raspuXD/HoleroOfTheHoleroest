using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayHighscore : MonoBehaviour
{
    public bool usesPoints, usesStars, usesTime;
    public TMP_Text scoreText;
    public GameObject[] stars;

    private void Start()
    {
        if(usesPoints)
        {
            int points = PlayerPrefs.GetInt("HighestPoints", 0);
            scoreText.text = points.ToString() + " points";
        }
        if (usesStars)
        {
            int starAmount = PlayerPrefs.GetInt("HighestStars", 0); // Assuming stars are stored under "HighestStars"
            scoreText.text = ""; // Clear the text since stars will be displayed graphically

            // Activate stars based on the amount
            for (int i = 0; i < stars.Length; i++)
            {
                if (stars[i] != null)
                    stars[i].SetActive(i < starAmount);
            }
        }
        if (usesTime)
        {
            float time = PlayerPrefs.GetFloat("HighestTime", 0);
            scoreText.text = time.ToString("F0") + " seconds";
        }
    }
}
