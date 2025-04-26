using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class OverallScore : MonoBehaviour
{
    [Header("References")]
    public List<PlacementScorer> scorers;
    public TextMeshProUGUI averageText;

    bool finalScoreLogged = false;
    public FadeIn endFade;
    public GameObject lastLevel;
    public TMP_Text finalText;
    void Update()
    {
        float total = 0f;
        int completedCount = 0;
        bool allDone = true;
        bool anyDone = false;

        foreach (var scorer in scorers)
        {
            if (scorer == null) continue;

            if (scorer.hasBeenDone)
            {
                anyDone = true;
                total += scorer.totalScore;
                completedCount++;
            }
            else
            {
                allDone = false;
            }
        }

        float average = (completedCount > 0) ? (total / completedCount) : 0f;
        averageText.text = average.ToString("F1") + "%";

        if (allDone && !finalScoreLogged)
        {
            Debug.Log("Final Average Score: " + average.ToString("F1") + "%");
            finalText.text = "You built a house with a build quality of \r\n" + average.ToString("F1") + "% out of 100%";
            lastLevel.SetActive(false);
            finalScoreLogged = true;
            endFade.StartFade();
        }
    }
}
