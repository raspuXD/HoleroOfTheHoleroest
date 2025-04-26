using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Factories : MonoBehaviour
{
    [SerializeField] private GameObject buyMenu;
    [SerializeField] private int BuildingLvl = 1;

    [SerializeField] public bool EnergyFactory;
    [SerializeField] public bool MetalFactory;

    [SerializeField] private TextMeshProUGUI infoText;


    public int totalCost;
    public HumanResource Currancy;

    public ProcentageManager Manager;

    private void Start()
    {
        if (Manager == null)
            Manager = GetComponent<ProcentageManager>();

        UpdateInfoText(); // Call here to show initial info
    }

    public void AddProcentage()
    {
        // Check if enough people are available for Energy Factory
        if (EnergyFactory && Manager != null && Currancy.howManyPeople >= totalCost)
        {
            Manager.AddEnergyPercentage(25f);  // Add 25 to Energy percentage
            
        }

        // Check if enough people are available for Metal Factory
        if (MetalFactory && Manager != null && Currancy.howManyPeople >= totalCost)
        {
            Manager.AddMetalPercentage(25f);  // Add 25 to Metal percentage
            
        }
    }
    private void UpdateInfoText()
    {
        string factoryType = EnergyFactory ? "Energy" : MetalFactory ? "Metal" : "Unknown";
        int cost = BuildingLvl * totalCost;

        infoText.text = $"Factory: {factoryType}\nLevel: {BuildingLvl}\nCost: {cost}";
    }

    public void BuyUpgrade(int baseCost)
    {
        int NewtotalCost = baseCost * BuildingLvl;

        totalCost = NewtotalCost;

        if (Currancy != null && Currancy.UseHumans(totalCost))
        {
            BuildingLvl++;
            AudioManager.Instance.PlaySFX("upgrade");
            AddProcentage();
            Debug.Log($"Upgrade bought! New level: {BuildingLvl}, Cost: {totalCost}");
            UpdateInfoText(); // Update text after upgrading
        }
        else
        {
            Debug.Log("Not enough currency to buy upgrade.");
        }
    }


    public void OpenBuyMenu()
    {
        if (buyMenu == null)
        {
            Debug.LogWarning("Buy menu GameObject is not assigned.");
            return;
        }

        // Toggle the menu
        buyMenu.SetActive(!buyMenu.activeSelf);
    }
}
