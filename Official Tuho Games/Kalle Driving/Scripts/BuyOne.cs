using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyOne : MonoBehaviour
{
    public string thePlayerPrefs;
    public int howMuchCost = 200;
    Button thisButton;
    public int howMuchMoney;
    public TMP_Text theCostText, levelText;
    public TMP_ColorGradient notEnough, enough;
    public Upgrade upga;
    private void Start()
    {
        howMuchMoney = PlayerPrefs.GetInt("Money", 0);
        thisButton = GetComponent<Button>();
        int hasIt = PlayerPrefs.GetInt(thePlayerPrefs, 0);

        if (hasIt == 1)
        {
            thisButton.interactable = false;
            levelText.text = "1/1";
            theCostText.text = "";
        }
        else
        {
            levelText.text = "0/1";
            theCostText.text = howMuchCost.ToString() + "€";
        }
    }

    private void Update()
    {
        if (howMuchMoney >= howMuchCost)
        {
            theCostText.colorGradientPreset = enough;
        }
        else
        {
            theCostText.colorGradientPreset = notEnough;
        }
    }

    public void BuyThis()
    {
        if(howMuchMoney >= howMuchCost)
        {
            howMuchMoney -= howMuchCost;
            PlayerPrefs.SetInt("Money", howMuchMoney);
            PlayerPrefs.SetInt(thePlayerPrefs, 1);
            PlayerPrefs.Save();
            upga.UpdateTheTexts();
            thisButton.interactable = false;
            levelText.text = "1/1";
            theCostText.text = "";
        }
    }

    public void ResetStat()
    {
        PlayerPrefs.SetInt(thePlayerPrefs, 0);
        PlayerPrefs.Save();

        howMuchMoney = PlayerPrefs.GetInt("Money", 0);
        thisButton.interactable = true;
        theCostText.text = howMuchCost.ToString() + "€";
        levelText.text = "0/1";
    }
}
