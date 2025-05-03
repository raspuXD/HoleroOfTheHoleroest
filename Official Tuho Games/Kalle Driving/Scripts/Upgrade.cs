using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    public int[] motorCosts, duraCosts, petteriCosts, maxSpeedCosts;
    public TMP_Text MoneyText, MotorCost, DuraCost, PetteriCost, MaxSpeedCost;
    public Button motorB, duraB, petteriB, speedB;
    public TMP_ColorGradient notEnough, enough;
    public int howMuchMoney;
    public TMP_Text motorL, duraL, petteL, speedL;

    [Header("Single Buy")]
    public int boostCost, turretCost;
    public TMP_Text boostCostText, turretCostText;
    public Button boostButton, turretButton;

    private int whatLevelM, whatLevelD, whatLevelP, whatLevelS, whatLevelBoost, whatLevelTurret;

    private void Start()
    {
        LoadPlayerData();
        UpdateTheTexts();
        CheckIfButtons();
    }

    public void ResetAllStats()
    {
        PlayerPrefs.SetInt("Money", 0);
        PlayerPrefs.SetInt("MotorUpgrade", 1);
        PlayerPrefs.SetInt("DuraUpgrade", 1);
        PlayerPrefs.SetInt("PetteriUpgrade", 1);
        PlayerPrefs.SetInt("SpeedUpgrade", 1);
        PlayerPrefs.SetInt("HasABooster", 0);
        PlayerPrefs.SetInt("HasATurret", 0);
        PlayerPrefs.Save();

        LoadPlayerData();
        UpdateTheTexts();
        CheckIfButtons();
    }

    private void LoadPlayerData()
    {
        howMuchMoney = PlayerPrefs.GetInt("Money", 0);
        whatLevelM = PlayerPrefs.GetInt("MotorUpgrade", 1);
        whatLevelD = PlayerPrefs.GetInt("DuraUpgrade", 1);
        whatLevelP = PlayerPrefs.GetInt("PetteriUpgrade", 1);
        whatLevelS = PlayerPrefs.GetInt("SpeedUpgrade", 1);
        whatLevelBoost = PlayerPrefs.GetInt("HasABooster", 0);
        whatLevelTurret = PlayerPrefs.GetInt("HasATurret", 0);
    }

    public void UpdateTheTexts()
    {
        MoneyText.text = howMuchMoney + "€";
        UpdateCostTexts();
        UpdateLevelTexts();
        UpdateButtonStates();
    }

    private void UpdateCostTexts()
    {
        MotorCost.text = GetUpgradeCost(motorCosts, whatLevelM) + "€";
        DuraCost.text = GetUpgradeCost(duraCosts, whatLevelD) + "€";
        PetteriCost.text = GetUpgradeCost(petteriCosts, whatLevelP) + "€";
        MaxSpeedCost.text = GetUpgradeCost(maxSpeedCosts, whatLevelS) + "€";
        // Update the booster and turret texts with the "Ostettu" or cost
        boostCostText.text = (whatLevelBoost == 0) ? boostCost + "€" : "Ostettu";
        turretCostText.text = (whatLevelTurret == 0) ? turretCost + "€" : "Ostettu";
    }

    private void UpdateLevelTexts()
    {
        motorL.text = $"{whatLevelM}/10";
        duraL.text = $"{whatLevelD}/10";
        petteL.text = $"{whatLevelP}/10";
        speedL.text = $"{whatLevelS}/10";
    }

    private void UpdateButtonStates()
    {
        SetButtonState(motorB, MotorCost, motorCosts, whatLevelM);
        SetButtonState(duraB, DuraCost, duraCosts, whatLevelD);
        SetButtonState(petteriB, PetteriCost, petteriCosts, whatLevelP);
        SetButtonState(speedB, MaxSpeedCost, maxSpeedCosts, whatLevelS);

        // Update booster and turret button states
        SetButtonStateForSingleBuy(boostButton, boostCostText, boostCost, whatLevelBoost);
        SetButtonStateForSingleBuy(turretButton, turretCostText, turretCost, whatLevelTurret);
    }

    private int GetUpgradeCost(int[] costArray, int level)
    {
        return (level - 1) < costArray.Length ? costArray[level - 1] : 0;
    }

    private void SetButtonState(Button button, TMP_Text costText, int[] costArray, int level)
    {
        int cost = GetUpgradeCost(costArray, level);
        bool canUpgrade = cost > 0 && howMuchMoney >= cost;
        costText.colorGradientPreset = canUpgrade ? enough : notEnough;
        button.interactable = canUpgrade;
    }

    // This method is specifically for buying the booster and turret
    private void SetButtonStateForSingleBuy(Button button, TMP_Text costText, int cost, int itemLevel)
    {
        bool canBuy = itemLevel == 0 && howMuchMoney >= cost;  // Check if item is not bought and player has enough money
        costText.colorGradientPreset = canBuy ? enough : notEnough;
        button.interactable = canBuy;
    }

    public void UpgradeMotor() => TryUpgrade(ref whatLevelM, motorCosts);
    public void UpgradeDurability() => TryUpgrade(ref whatLevelD, duraCosts);
    public void UpgradePetteriMoney() => TryUpgrade(ref whatLevelP, petteriCosts);
    public void UpgradeMaxSpeed() => TryUpgrade(ref whatLevelS, maxSpeedCosts);

    private void TryUpgrade(ref int level, int[] costArray)
    {
        if (level < 10)
        {
            int cost = GetUpgradeCost(costArray, level);
            if (cost <= howMuchMoney)
            {
                howMuchMoney -= cost;
                PlayerPrefs.SetInt("Money", howMuchMoney);
                PlayerPrefs.Save();
                level++;
                UpdateTheTexts();
                CheckIfButtons();
            }
        }
    }

    public void BuyBooster()
    {
        if (whatLevelBoost == 0 && howMuchMoney >= boostCost)  // Check if booster is not bought and user has enough money
        {
            howMuchMoney -= boostCost;
            whatLevelBoost = 1;  // Mark booster as bought
            PlayerPrefs.SetInt("Money", howMuchMoney);
            PlayerPrefs.SetInt("HasABooster", whatLevelBoost);
            PlayerPrefs.Save();
            UpdateTheTexts();
            CheckIfButtons();
        }
    }

    public void BuyTurret()
    {
        if (whatLevelTurret == 0 && howMuchMoney >= turretCost)  // Check if turret is not bought and user has enough money
        {
            howMuchMoney -= turretCost;
            whatLevelTurret = 1;  // Mark turret as bought
            PlayerPrefs.SetInt("Money", howMuchMoney);
            PlayerPrefs.SetInt("HasATurret", whatLevelTurret);
            PlayerPrefs.Save();
            UpdateTheTexts();
            CheckIfButtons();
        }
    }

    public void SaveStuff()
    {
        PlayerPrefs.SetInt("Money", howMuchMoney);
        PlayerPrefs.SetInt("MotorUpgrade", whatLevelM);
        PlayerPrefs.SetInt("DuraUpgrade", whatLevelD);
        PlayerPrefs.SetInt("PetteriUpgrade", whatLevelP);
        PlayerPrefs.SetInt("SpeedUpgrade", whatLevelS);
        PlayerPrefs.SetInt("HasABooster", whatLevelBoost);
        PlayerPrefs.SetInt("HasATurret", whatLevelTurret);
        PlayerPrefs.Save();
    }

    public void CheckIfButtons()
    {
        DisableButtonIfMaxLevel(motorB, MotorCost, whatLevelM);
        DisableButtonIfMaxLevel(duraB, DuraCost, whatLevelD);
        DisableButtonIfMaxLevel(petteriB, PetteriCost, whatLevelP);
        DisableButtonIfMaxLevel(speedB, MaxSpeedCost, whatLevelS);
        SaveStuff();
    }

    private void DisableButtonIfMaxLevel(Button button, TMP_Text costText, int level)
    {
        if (level >= 10)
        {
            button.interactable = false;
            costText.text = "";
        }
    }
}
