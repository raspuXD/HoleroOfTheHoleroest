using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelOfFortune : MonoBehaviour
{
    public GameObject Arrow;             // The arrow object
    public GameObject Wheel;             // The wheel object
    public float spinDuration = 5f;      // Time in seconds for how long the wheel will spin
    public float minSpinSpeed = 300f;    // Minimum spin speed
    public float maxSpinSpeed = 1000f;   // Maximum spin speed
    public LayerMask multiplierLayerMask; // Layer mask to filter raycast hits for multipliers
    public float RaycastRange;            // The range of the raycast
    private bool isSpinning = false;     // To track if the wheel is currently spinning 
    public MultiplierSlot[] multiplierarray; // Currently selected multiplier segment
    public GameObject theCanvas;
    public SceneHandler sceneHandler;

    public GameObject theGamnblingCanvas;
    public TMP_Text theMoneyText;

    private void Start()
    {
        StartSpin();
        theGamnblingCanvas.SetActive(true);
        sceneHandler.UpdateMouseState(false);
    }

    private void Update()
    {
        CheckForMultiplierOften();
    }

    public void StartSpin()
    {
        if (!isSpinning)
        {
            float randomSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);
            StartCoroutine(SpinWheel(randomSpeed));
        }
    }

    // Coroutine to handle the wheel spinning logic
    private IEnumerator SpinWheel(float initialSpeed)
    {
        isSpinning = true;
        float elapsedTime = 0f;
        float currentSpeed = initialSpeed;

        while (elapsedTime < spinDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / spinDuration;
            currentSpeed = Mathf.Lerp(initialSpeed, 0, t);

            Wheel.transform.Rotate(currentSpeed * Time.unscaledDeltaTime, 0, 0);

            if (elapsedTime >= spinDuration)
            {
                CheckForMultiplier();
            }

            yield return null;
        }

        isSpinning = false;
    }


    public void CheckForMultiplier()
    {
        RaycastHit hit;
        Vector3 arrowPosition = Arrow.transform.position;
        Vector3 rayDirection = Vector3.down;

        if (Physics.Raycast(arrowPosition, rayDirection, out hit, RaycastRange, multiplierLayerMask))
        {
            Debug.Log("Good");

            MultiplierSlot multiplier = hit.collider.GetComponent<MultiplierSlot>();
            if (multiplier != null)
            {
                Debug.Log("Toimii");
                multiplier.Multiplymoney();
                StartCoroutine(WAitlOl());
            }
        }
        else
        {
            Debug.Log("SATTANA");
        }
    }

    public void CheckForMultiplierOften()
    {
        RaycastHit hit;
        Vector3 arrowPosition = Arrow.transform.position;
        Vector3 rayDirection = Vector3.down;

        if (Physics.Raycast(arrowPosition, rayDirection, out hit, RaycastRange, multiplierLayerMask))
        {
            MultiplierSlot multiplier = hit.collider.GetComponent<MultiplierSlot>();
            theMoneyText.text = "Rahan kerroin " + multiplier.multiplierValue.ToString("F1") + "x";
        }
    }

    IEnumerator WAitlOl()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        sceneHandler.UpdateMouseState(true);
        theCanvas.SetActive(true);
    }

    private void OnDrawGizmos()
    {
        if (Arrow != null)
        {
            Gizmos.color = Color.red;
            Vector3 arrowPosition = Arrow.transform.position;
            Vector3 rayDirection = Vector3.down * RaycastRange;

            // Draw the raycast direction
            Gizmos.DrawLine(arrowPosition, arrowPosition + rayDirection);
        }
    }

}
