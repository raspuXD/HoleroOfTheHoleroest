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

    [Header("Closing options")]
    public KeyCode closeKey = KeyCode.Escape;

    [Header("Scale Speed and stuff")]
    public float howFastScales = .33f;
    public float howLongToCanClose = 2f;

    [Header("If want to in the start")]
    public bool showInStart = false;
    public string wantToWriteInStart;
    bool hasBeenShow;

    private IEnumerator Start()
    {
        if(showInStart)
        {
            yield return new WaitForSeconds(1.5f);
            StartThePopUp(wantToWriteInStart);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !hasBeenShow)
        {
            hasBeenShow = true;
            StartThePopUp(wantToWriteInStart);
        }
    }

    public void StartThePopUp(string textToShow)
    {
        thePopUp.transform.localScale = Vector3.zero; // Ensure starting scale is zero
        theTutorialButton.gameObject.SetActive(true);
        theInfoText.text = textToShow;
        theTutorialButton.interactable = false;
        StartCoroutine(ScaleTheButton(true));
    }

    IEnumerator ScaleTheButton(bool scaleUp)
    {
        Vector3 initialScale = thePopUp.transform.localScale;
        Vector3 targetScale = scaleUp ? Vector3.one : Vector3.zero;
        float timeElapsed = 0f;

        while (timeElapsed < howFastScales)
        {
            thePopUp.transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / howFastScales);
            timeElapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        thePopUp.transform.localScale = targetScale;

        if (scaleUp)
        {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(howLongToCanClose);
            canCountinueNow.SetActive(true);
            howToGetAwayText.text = "Click to Close or\r\n"+closeKey.ToString();
            theTutorialButton.interactable = true;
        }
        else
        {
            Time.timeScale = 1f;
            theInfoText.text = "";
            howToGetAwayText.text = "";
            canCountinueNow.SetActive(false);
            theTutorialButton.gameObject.SetActive(false);
        }
    }

    public void CloseThePopUp()
    {
        theTutorialButton.interactable = false;
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