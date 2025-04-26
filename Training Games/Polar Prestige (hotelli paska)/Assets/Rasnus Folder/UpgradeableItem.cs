using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Ensure you have this namespace

public class UpgradeableItem : MonoBehaviour
{
    public string ItemName;
    public int UpgradeLevel { get; private set; } = 0;
    public float BaseUpgradeCost = 100f;
    public float CostMultiplier = 1.5f;

    public float UpgradeEffectMultiplier = 1.2f;
    public float BaseEarning = 10f;

    public Button UpgradeButton;
    public GameObject theCanvas;
    public bool CanOpenMenu = true;

    [Header("UI References")]
    public TextMeshProUGUI UpgradeCostText; // Reference to the TextMeshProUGUI to display the upgrade cost
    public Sprite level1, level2;
    Image thisImage;
    public GameObject workingIcon;

    public AudioSource audioSource;

    private void Start()
    {
        UpdateUpgradeCostUI(); // Initial UI update when the script starts
        thisImage = GetComponent<Image>();
    }

    public void Upgrade()
    {
        if (UpgradeLevel != 2)
        {
            float upgradeCost = GetUpgradeCost();
            if (MoneyManager.Instance.SpendMoney(upgradeCost))
            {
                audioSource.Play();
                UpgradeLevel++;
                Debug.Log($"{ItemName} upgraded to level {UpgradeLevel}!");
                UpdateUpgradeCostUI(); // Update the upgrade cost UI after upgrading
                if(UpgradeLevel == 1)
                {
                    thisImage.sprite = level1;
                }

                if(UpgradeLevel == 2)
                {
                    thisImage.sprite = level2;
                    theCanvas.SetActive(false); 
                    CanOpenMenu = false;
                    Button thisButton = GetComponent<Button>();
                    thisButton.interactable = false;
                    workingIcon.SetActive(false);
                }
            }
        }
        else
        {
            Debug.Log("MAX LEVEL");
            theCanvas.SetActive(false);
            CanOpenMenu = false;
        }
    }

    // When the button is clicked, set the upgrade logic
    public void WhenClicked()
    {
        UpgradeButton.onClick.RemoveAllListeners();
        if(CanOpenMenu)
        {
            UpgradeButton.onClick.AddListener(Upgrade);
            UpdateUpgradeCostUI();
            theCanvas.SetActive(true);
        }
    }

    public float GetUpgradeCost()
    {
        return BaseUpgradeCost * Mathf.Pow(CostMultiplier, UpgradeLevel);
    }

    public float GetEarnings()
    {
        return BaseEarning * Mathf.Pow(UpgradeEffectMultiplier, UpgradeLevel);
    }

    

    // Method to update the UI with the current upgrade cost
    private void UpdateUpgradeCostUI()
    {
        if (UpgradeCostText != null)
        {
            // Ensure the upgrade cost is calculated and the UI text is updated accordingly
            float upgradeCost = GetUpgradeCost();
            UpgradeCostText.text = $"Upgrade Cost: €{upgradeCost:F2}";
        }
    }
}
