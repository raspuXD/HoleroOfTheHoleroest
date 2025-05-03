using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutomaticButtonSound : MonoBehaviour
{
    public string theSoundName;

    private HashSet<Button> processedButtons = new HashSet<Button>();
    private HashSet<TMP_Dropdown> processedDropdowns = new HashSet<TMP_Dropdown>();
    private HashSet<Toggle> processedToggles = new HashSet<Toggle>();

    private bool isChecking = false;

    void Start()
    {
        // Initial processing of all existing UI elements
        ProcessUIElements();
    }

    void Update()
    {
        // Detect left mouse click
        if (Input.GetMouseButtonDown(0) && !isChecking)
        {
            StartCoroutine(CheckForUIElements());
        }
    }

    private void ProcessUIElements()
    {
        // Check for all buttons in the scene
        Button[] allButtons = FindObjectsOfType<Button>(true);
        foreach (Button button in allButtons)
        {
            if (!processedButtons.Contains(button))
            {
                processedButtons.Add(button);
                button.onClick.AddListener(() => AudioManager.Instance.PlaySFX(theSoundName));
            }
        }

        // Check for all TMP_Dropdowns in the scene
        TMP_Dropdown[] allDropdowns = FindObjectsOfType<TMP_Dropdown>(true);
        foreach (TMP_Dropdown dropdown in allDropdowns)
        {
            if (!processedDropdowns.Contains(dropdown))
            {
                processedDropdowns.Add(dropdown);
                dropdown.onValueChanged.AddListener((value) => AudioManager.Instance.PlaySFX(theSoundName));
            }
        }

        // Check for all Toggles in the scene
        Toggle[] allToggles = FindObjectsOfType<Toggle>(true);
        foreach (Toggle toggle in allToggles)
        {
            if (!processedToggles.Contains(toggle))
            {
                processedToggles.Add(toggle);
                toggle.onValueChanged.AddListener((isOn) => AudioManager.Instance.PlaySFX(theSoundName));
            }
        }
    }

    private IEnumerator CheckForUIElements()
    {
        isChecking = true;

        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.2f);

        // Process UI elements after the delay
        ProcessUIElements();

        isChecking = false;
    }
}
