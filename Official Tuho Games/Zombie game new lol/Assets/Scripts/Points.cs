using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] TMP_Text pointsText;
    [SerializeField] CurrentDrops currentDrops;
    public int points;

    public void Start()
    {
        points = 500;
        UpdateUI();
    }

    public void UpdateUI()
    {
        pointsText.text = points.ToString();
    }

    public void BuySomething(int cost)
    {
        points = Mathf.Max(points - cost, 0);
        UpdateUI();
    }

    public void EarnPoints(int amount)
    {
        if(currentDrops.isDoublePoints)
        {
            amount *= 2;
        }

        points += amount;
        UpdateUI();
    }
}
