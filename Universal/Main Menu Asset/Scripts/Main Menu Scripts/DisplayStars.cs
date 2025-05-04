using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayStars : MonoBehaviour
{
    public bool usesPoints, usesStars, usesTime;
    public GameObject[] stars;
    public string[] messagesForEachAmountOfStars;
    public TMP_Text theStarMessageText;

    private void Start()
    {
        if (usesPoints)
        {
            int howManyPoints = PlayerPrefs.GetInt("HowManyPoints", 0);
            theStarMessageText.text = "You gathered " + howManyPoints + " points!";
        }

        if (usesTime)
        {
            float howLong = PlayerPrefs.GetFloat("HowLong", 0);
            theStarMessageText.text = "You survived for " + howLong.ToString("F0") + " seconds!";
        }

        if(usesStars)
        {
            int howManyStars = PlayerPrefs.GetInt("HowManyStars", 0);

            for (int i = 0; i < howManyStars && i < stars.Length; i++)
            {
                stars[i].SetActive(true);
            }

            if (howManyStars >= 0 && howManyStars < messagesForEachAmountOfStars.Length)
            {
                theStarMessageText.text = messagesForEachAmountOfStars[howManyStars];
            }
        }
    }
}
