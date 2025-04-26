using UnityEngine;
using TMPro;
using System.Collections;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [Header("Money Properties")]
    [SerializeField] private float money; // Backing field for Money property

    [Header("UI Elements")]
    public TextMeshProUGUI MoneyText; // Optional, can still assign a UI text
    public GameObject MoneyDisplayObject; // The GameObject to animate

    public float Money => money; // Public getter for Money

    private Vector3 initialScale;
    private Quaternion initialRotation;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Store the initial scale and rotation at the start
        if (MoneyDisplayObject != null)
        {
            initialScale = MoneyDisplayObject.transform.localScale;
            initialRotation = MoneyDisplayObject.transform.rotation;
        }

        UpdateMoneyUI();
    }

    public void AddMoney(float amount)
    {
        money += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(float amount)
    {
        if (money >= amount)
        {
            money -= amount;  // Deduct the amount from the player's money
            UpdateMoneyUI();   // Update the UI to reflect the new balance
            Debug.Log($"Money spent: {amount}. Remaining: {money}");  // Debug log to track money changes
            return true;
        }
        else
        {
            Debug.Log("Not enough money!");
            return false;  // Return false if the player can't afford the bill
        }
    }

    public void PayBills(float amount)
    {
        money -= amount;
        UpdateMoneyUI();
    }

    private void UpdateMoneyUI()
    {
        if (MoneyText != null)
        {
            MoneyText.text = $"${money:F2}";
        }

        if (MoneyDisplayObject != null)
        {
            StartCoroutine(AnimateMoneyChange(MoneyDisplayObject));
        }
    }

    private IEnumerator AnimateMoneyChange(GameObject targetObject)
    {
        Vector3 targetScale = initialScale * 1.2f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 15); // Rotate 15 degrees on Z-axis

        float duration = 0.3f; // Animation duration
        float elapsedTime = 0;

        // Scale and rotate up
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            targetObject.transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            targetObject.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);

            yield return null;
        }

        // Reset animation
        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            targetObject.transform.localScale = Vector3.Lerp(targetScale, initialScale, t);
            targetObject.transform.rotation = Quaternion.Lerp(targetRotation, initialRotation, t);

            yield return null;
        }

        // Ensure final state is precise
        targetObject.transform.localScale = initialScale;
        targetObject.transform.rotation = initialRotation;
    }
}
