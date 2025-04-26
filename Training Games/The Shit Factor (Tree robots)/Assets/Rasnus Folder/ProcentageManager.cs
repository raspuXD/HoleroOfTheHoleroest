using TMPro;
using UnityEngine;

public class ProcentageManager : MonoBehaviour
{
    public float EnergyProcentager = 10f;
    public float MaxEnergyProcentager = 15f;
    public TextMeshProUGUI EnergyTXT;
    public TextMeshProUGUI MaxEnergyTXT;

    public float MetalProcentager = 10f;
    public float MaxMetalProcentager = 15f;
    public TextMeshProUGUI MetalTXT;
    public TextMeshProUGUI MaxMetalTXT;

    [SerializeField] private HumanResource humanResource;

    private void Update()
    {
        // Calculate and update the percentages based on the number of people
        UpdatePercentageBasedOnHumans();
    }

    private void UpdatePercentageBasedOnHumans()
    {
        // Assuming 1 person = 1% of both energy and metal
        float humanPercentage = humanResource.GetHumanPercentage();

        // Set Energy and Metal percentages based on the available people
        EnergyProcentager = Mathf.Clamp(humanPercentage, 0f, MaxEnergyProcentager);
        MetalProcentager = Mathf.Clamp(humanPercentage, 0f, MaxMetalProcentager);

        // Update the UI text
        UpdatePercentageText(EnergyTXT, EnergyProcentager, MaxEnergyTXT, MaxEnergyProcentager, "Energy");
        UpdatePercentageText(MetalTXT, MetalProcentager, MaxMetalTXT, MaxMetalProcentager, "Metal");
    }

    private void UpdatePercentageText(TMP_Text currentTextComponent, float currentPercentage, TMP_Text maxTextComponent, float maxPercentage, string label)
    {
        if (currentTextComponent != null)
        {
            currentTextComponent.text = $"{label}: {currentPercentage.ToString("F0")}%";
        }

        if (maxTextComponent != null)
        {
            maxTextComponent.text = $"{label} Max: {maxPercentage.ToString("F0")}%";
        }
    }

    public void AddEnergyPercentage(float amount)
    {
        MaxEnergyProcentager = Mathf.Clamp(MaxEnergyProcentager + amount, 0f, 100f); // Ensure the max value is within 0-100
        EnergyProcentager = Mathf.Clamp(EnergyProcentager, 0f, MaxEnergyProcentager); // Clamp EnergyProcentager to MaxEnergyProcentager

        // Update the UI text
        UpdatePercentageText(EnergyTXT, EnergyProcentager, MaxEnergyTXT, MaxEnergyProcentager, "Energy");

        Debug.Log("New Energy Percentage: " + EnergyProcentager);
    }

    public void AddMetalPercentage(float amount)
    {
        MaxMetalProcentager = Mathf.Clamp(MaxMetalProcentager + amount, 0f, 100f); // Ensure the max value is within 0-100
        MetalProcentager = Mathf.Clamp(MetalProcentager, 0f, MaxMetalProcentager); // Clamp MetalProcentager to MaxMetalProcentager

        // Update the UI text
        UpdatePercentageText(MetalTXT, MetalProcentager, MaxMetalTXT, MaxMetalProcentager, "Metal");

        Debug.Log("New Metal Percentage: " + MetalProcentager);
    }
}
