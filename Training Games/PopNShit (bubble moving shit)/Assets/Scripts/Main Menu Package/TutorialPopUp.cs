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

    [Header("If want to from trigger")]
    public bool fromATrigger = false;
    public string wantToWriteInTrigger;

    [Header("See only once ever")]
    public bool willSeeOnlyOnce = true;
    public string thePlayerPrefName = "TutorialPopUp0";

    private void Start()
    {
        if(showInStart)
        {
            StartThePopUp(wantToWriteInStart);
        }
    }

    public void StartThePopUp(string textToShow)
    {
        if (willSeeOnlyOnce)
        {
            int hasSeen = PlayerPrefs.GetInt(thePlayerPrefName, 0);
            if (hasSeen == 0)
            {
                thePopUp.transform.localScale = Vector3.zero; // Ensure starting scale is zero
                theTutorialButton.gameObject.SetActive(true);
                theInfoText.text = textToShow;
                theTutorialButton.interactable = false;
                AudioManager.Instance.PlaySFX("PopUp");
                StartCoroutine(ScaleTheButton(true));
            }
            else
            {
                Debug.Log("Has Seen Already");
            }
        }
        else
        {
            thePopUp.transform.localScale = Vector3.zero; // Ensure starting scale is zero
            theTutorialButton.gameObject.SetActive(true);
            theInfoText.text = textToShow;
            theTutorialButton.interactable = false;
            AudioManager.Instance.PlaySFX("PopUp");
            StartCoroutine(ScaleTheButton(true));
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (fromATrigger)
            {
                StartThePopUp(wantToWriteInTrigger);
            }
        }
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
            AudioManager.Instance.PlaySFX("PopUpBuff");
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
        if(willSeeOnlyOnce)
        {
            PlayerPrefs.SetInt(thePlayerPrefName, 1);
            PlayerPrefs.Save();
        }
        theTutorialButton.interactable = false;
        AudioManager.Instance.PlaySFX("PopDown");
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