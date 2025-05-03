using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int theMoneyAmount = 0;
    public TMP_Text[] theText;
    public TMP_Text hahahs;

    public int howMuchPetteriGives = 1;

    private void Awake()
    {
        int theMultiplier = PlayerPrefs.GetInt("PetteriUpgrade", 1);
        howMuchPetteriGives = theMultiplier * 2;
        UpdateTheText();
    }

    public void GiveMoney(int amount)
    {
        theMoneyAmount += amount;
        UpdateTheText();
    }

    public void GivePetteriMoney(float theMultiplier)
    {
        theMoneyAmount += Mathf.RoundToInt(howMuchPetteriGives * theMultiplier);
        UpdateTheText();
    }


    public void UpdateTheText()
    {
        foreach (var item in theText)
        {
            item.text = theMoneyAmount.ToString("F0") + "€";
        }

        hahahs.text = "Tienasit " + theMoneyAmount.ToString("F0") + "€\r\nVoit uhkapelata ja jopa tienata 5 kertaa enemmän!";
    }

    private void OnDisable()
    {
        int currentMonnn = PlayerPrefs.GetInt("Money",0);
        currentMonnn += theMoneyAmount;
        PlayerPrefs.SetInt("Money", currentMonnn);
        Debug.Log(currentMonnn);
        PlayerPrefs.Save();
    }
}
