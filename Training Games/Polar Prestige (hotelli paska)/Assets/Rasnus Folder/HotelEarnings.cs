using TMPro;
using UnityEngine;

public class HotelEarnings : MonoBehaviour
{
    public UpgradeableItem[] HotelPieces;
    public float EarningsInterval = 5f; // Seconds
    public TextMeshProUGUI countdownText; // Reference to the TextMeshProUGUI component

    public AudioSource source;
    private float timeRemaining;

    private void Start()
    {
        timeRemaining = EarningsInterval;
        InvokeRepeating(nameof(GenerateEarnings), EarningsInterval, EarningsInterval);
    }

    private void Update()
    {
        // Decrease the time remaining each frame
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            // Update the countdown display
            countdownText.text = "You will get your paycheck in " + Mathf.Ceil(timeRemaining).ToString() + "s";
        }
    }

    private void GenerateEarnings()
    {
        // Reset the countdown timer
        timeRemaining = EarningsInterval;

        float totalEarnings = 0;
        foreach (var item in HotelPieces)
        {
            if (item.gameObject.activeInHierarchy)
            {
                totalEarnings += item.GetEarnings();
            }
        }

        MoneyManager.Instance.AddMoney(totalEarnings);
        Debug.Log("Total earnings generated: " + totalEarnings);
        source.Play();
    }
}