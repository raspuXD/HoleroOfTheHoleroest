using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopUp : MonoBehaviour
{
    public Button theTutorialButton;
    public GameObject thePopUp, canCountinueNow;
    public TMP_Text theInfoText, howToGetAwayText;

    [Header("Start")]

    public bool atTheStart = false;
    public string theTextIfStart;

    [Header("Closing options")]
    public KeyCode closeKey = KeyCode.Escape;
    public CameraMovement movement;

    public void AssingCloseKey(KeyCode theNewKey)
    {
        closeKey = theNewKey;
    }

    private void Start()
    {
        if(atTheStart)
        {
            StartThePopUp(theTextIfStart);
        }
    }

    public void StartThePopUp(string textToShow)
    {
        if(movement != null)
        {
            Time.timeScale = 0;
        }
        theTutorialButton.gameObject.SetActive(true);
        theInfoText.text = textToShow;
        theTutorialButton.interactable = false;
        StartCoroutine(ScaleTheButton(true));
    }

    IEnumerator ScaleTheButton(bool scaleUp)
    {
        float duration = 0.33f;
        Vector3 initialScale = thePopUp.transform.localScale;
        Vector3 targetScale = scaleUp ? Vector3.one : Vector3.zero;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            thePopUp.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        thePopUp.transform.localScale = targetScale;

        if (scaleUp)
        {
            yield return new WaitForSecondsRealtime(1f);
            canCountinueNow.SetActive(true);
            howToGetAwayText.text = "Click to Close or\r\n"+closeKey.ToString();
            theTutorialButton.interactable = true;
        }
        else
        {
            theInfoText.text = "";
            howToGetAwayText.text = "";
            theTutorialButton.gameObject.SetActive(false);
            if (movement != null)
            {
                Time.timeScale = 1;
            }
        }
    }

    public void CloseThePopUp()
    {
        theTutorialButton.interactable = false;
        canCountinueNow.SetActive(false);
        StartCoroutine(ScaleTheButton(false));
    }
    void Update()
    {
        if (canCountinueNow.activeSelf && Input.GetKeyDown(closeKey))
        {
            CloseThePopUp();
        }
    }
}