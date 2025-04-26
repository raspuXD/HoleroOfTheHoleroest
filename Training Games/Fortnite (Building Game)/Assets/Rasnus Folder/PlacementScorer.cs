using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlacementScorer : MonoBehaviour
{
    [Header("References")]
    public List<PiecePlacer> pieces;
    public TextMeshProUGUI scoreText;
    public float totalScore;
    public bool hasBeenDone = false;

    public Button nextStageButton;
    public GameObject thisLevel, nextLevel;

    public GameObject allStageProgress;

    public void UpdateScore()
    {
        totalScore = 0f;

        int totalPieceCount = pieces.Count;
        float pieceWeight = (totalPieceCount > 0) ? (100f / totalPieceCount) : 0f;

        foreach (var piece in pieces)
        {
            if (piece != null && piece.Placed)
            {
                PlacementArea bestArea = FindBestPlacementAreaFor(piece.gameObject);
                if (bestArea != null)
                {
                    float overlapScore = bestArea.GetOverlapScore(piece.gameObject);
                    float contribution = Mathf.Clamp01(overlapScore) * pieceWeight;
                    totalScore += contribution;
                }
            }
        }

        scoreText.text = totalScore.ToString("F1") + "%";
    }

    public void Update()
    {
        if (!Input.GetMouseButton(0))
        {
            UpdateScore();
            if(!hasBeenDone)
            {
                IsAllPlaced();
            }
        }
    }

    public void IsAllPlaced()
    {
        bool allPlaced = true;

        // Iterate through each piece in the list to check if it's placed
        foreach (var piece in pieces)
        {
            if (piece != null && !piece.Placed)  // If any piece is not placed, set allPlaced to false
            {
                allPlaced = false;
                break;
            }
        }

        // If all pieces are placed, handle the transition to the next stage
        if (allPlaced)
        {
            hasBeenDone = true;
            allStageProgress.SetActive(true);

            if (nextLevel != null)
            {
                nextStageButton.gameObject.SetActive(true);

                // Add functionality to switch levels when the nextStageButton is pressed
                nextStageButton.onClick.AddListener(() => LoadNextLevel());
            }
        }
    }

    // Method to transition to the next level (you can modify it based on your needs)
    void LoadNextLevel()
    {
        if(nextLevel != null)
        {
            // Deactivate the current level
            thisLevel.SetActive(false);

            // Activate the next level
            nextLevel.SetActive(true);

            // Deactivate the next stage button after clicking
            nextStageButton.gameObject.SetActive(false);

            allStageProgress.SetActive(false);
        }
    }


    PlacementArea FindBestPlacementAreaFor(GameObject piece)
    {
        PlacementArea[] allAreas = FindObjectsOfType<PlacementArea>();
        PlacementArea bestArea = null;
        float bestScore = 0f;

        foreach (var area in allAreas)
        {
            float score = area.GetOverlapScore(piece);
            if (score > bestScore)
            {
                bestScore = score;
                bestArea = area;
            }
        }

        return bestArea;
    }
}
