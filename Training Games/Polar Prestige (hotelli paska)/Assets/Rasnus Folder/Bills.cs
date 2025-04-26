using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bills : MonoBehaviour
{
    [Header("Bill Settings")]
    public float billAmount; // Initial bill amount
    public float billInterval; // Time in seconds between bill payments

    [Header("UI References")]
    public TextMeshProUGUI billAmountText; // Text to show the current bill amount
    public TextMeshProUGUI timeUntilBillText; // Text to show the time until the next bill is due

    private float timeRemaining;

    private void Start()
    {
        // Start the bill deduction process when the game starts
        timeRemaining = billInterval; // Set the initial time to the bill interval
        billAmountText.text = "Bill Amount: " + billAmount.ToString("F2");
        StartCoroutine(DeductBills());
    }

    private void Update()
    {
        // Update the countdown timer and the UI each frame
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Decrease the timer
            timeUntilBillText.text = "Next payment in: " + Mathf.Ceil(timeRemaining).ToString() + "s";
        }
    }

    private IEnumerator DeductBills()
    {
        while (true)
        {
            yield return new WaitForSeconds(billInterval); // Wait for the interval

            // Deduct the bill before adding earnings
            MoneyManager.Instance.PayBills(billAmount);  // Correctly call the PayBills method on the singleton instance

            // Update the UI with the bill amount
            billAmountText.text = "Bill Amount: " + billAmount.ToString("F2");

            Debug.Log($"Bill deducted: {billAmount}. Remaining: {MoneyManager.Instance.Money}");
            /*
            if (MoneyManager.Instance.Money <= 0)
            {
                // If not enough money, trigger the loss condition
                OnPlayerLose();
                break; // Exit the loop, stop deducting bills
            }*/

            // After paying the bill, multiply the bill amount by 2 or 3x for the next round
            billAmount *= Random.Range(2, 4); // Multiplies the bill by either 2 or 3
            timeRemaining = billInterval; // Reset the timer for the next bill
        }
    }

    private void OnPlayerLose()
    {
        // Trigger a loss condition, for example by showing a "You Lose" message or restarting the game
        Debug.Log("Player has lost due to insufficient funds!");
        //SceneManager.LoadScene("gameover");
        // Example: GameManager.Instance.GameOver();
    }
}
