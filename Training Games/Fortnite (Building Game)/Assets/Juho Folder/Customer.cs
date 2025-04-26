using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using static Unity.Collections.AllocatorManager;
using UnityEditor.Rendering;

[System.Serializable]
public class CustomerData
{
    public string name;
    public string line1, line2, playerChoiceLine, accecptLine, declineLine;
    public float howMuchMoneyWillGive;
    public bool hasSeen = false;
}

public class Customer : MonoBehaviour
{
    public CustomerData[] males;
    public CustomerData[] women;
    public GameObject theButton, theAccpectAndDeclineHolder, countinueDialog, fadeToComic;
    public Image CustomerImage;
    public CanvasGroup SpeakBubbleGroup;
    public TMP_Text SpeakText, nameText;
    private bool hasClicked = false;

    public Sprite[] maleSprites, femaleSprites;

    public FadeIn theNoCustomerFade;
    private bool currentIsMale;


    private CustomerData currentCustomer;

    public GameObject theCustomerCanvas, theComic, cameras;
    public KnockKnock knock;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(4f);
        knock.Activate();
        AudioManager.Instance.PlaySFX("Knock");
        theButton.SetActive(true);
    }

    public void CallCustomer()
    {
        nameText.text = "";

        List<(CustomerData, bool)> unseenCustomers = new List<(CustomerData, bool)>();

        foreach (var male in males)
            if (!male.hasSeen) unseenCustomers.Add((male, true));

        foreach (var female in women)
            if (!female.hasSeen) unseenCustomers.Add((female, false));

        if (unseenCustomers.Count == 0)
        {
            theNoCustomerFade.StartFade();
            return;
        }

        var (selectedCustomer, isMale) = unseenCustomers[Random.Range(0, unseenCustomers.Count)];
        currentIsMale = isMale;
        currentCustomer = selectedCustomer;
        StartCoroutine(ShowCustomer(isMale));
    }


    public void ContinueDialogue()
    {
        hasClicked = true;
    }


    IEnumerator ShowCustomer(bool isMale)
    {
        if(isMale)
        {
            int whatSkin = Random.Range(0, maleSprites.Length);
            CustomerImage.sprite = maleSprites[whatSkin];
        }
        else
        {
            int whatSkin = Random.Range(0, femaleSprites.Length);
            CustomerImage.sprite = femaleSprites[whatSkin];
        }

        float time = 0f;
        float duration = 1f;

        Color imageColor = CustomerImage.color;
        imageColor.a = 0f;
        CustomerImage.color = imageColor;

        Color nameColor = nameText.color;
        nameColor.a = 0f;
        nameText.color = nameColor;
        nameText.text = currentCustomer.name;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            imageColor.a = t;
            nameColor.a = t;
            CustomerImage.color = imageColor;
            nameText.color = nameColor;
            yield return null;
        }

        StartCoroutine(FadeInSpeakBubble());
    }


    IEnumerator FadeInSpeakBubble()
    {
        float time = 0f;
        float duration = 0.5f;
        SpeakBubbleGroup.alpha = 0f;
        SpeakBubbleGroup.gameObject.SetActive(true);

        while (time < duration)
        {
            time += Time.deltaTime;
            SpeakBubbleGroup.alpha = Mathf.Clamp01(time / duration);
            yield return null;
        }

        StartCoroutine(DialogueSequence());
    }

    IEnumerator DialogueSequence()
    {
        yield return StartCoroutine(TypeText(currentCustomer.line1));
        yield return new WaitForSeconds(1.5f);
        countinueDialog.SetActive(true);
        yield return WaitForClick();

        yield return StartCoroutine(TypeText(currentCustomer.line2));
        yield return new WaitForSeconds(1.5f);
        countinueDialog.SetActive(true);
        yield return WaitForClick();

        yield return StartCoroutine(TypeText(currentCustomer.playerChoiceLine));
        theAccpectAndDeclineHolder.SetActive(true);
        currentCustomer.hasSeen = true;
    }

    IEnumerator WaitForClick()
    {
        hasClicked = false;
        while (!hasClicked)
        {
            yield return null;
        }
    }


    public void Accept()
    {
        AudioManager.Instance.PlaySFX("Accept");
        StartCoroutine(AcceptSequence());
    }

    public void Decline()
    {
        AudioManager.Instance.PlaySFX("Decline");
        StartCoroutine(DeclineSequence());
    }

    IEnumerator AcceptSequence()
    {
        theAccpectAndDeclineHolder.SetActive(false);
        yield return StartCoroutine(TypeText(currentCustomer.accecptLine));
        yield return new WaitForSeconds(2f);
        Debug.Log("accepted over");

        fadeToComic.SetActive(true);

        yield return new WaitForSeconds(.8f);

        theComic.SetActive(true);
        theCustomerCanvas.SetActive(false);
        StopAllCoroutines();
        cameras.SetActive(false);
        enabled = false;
    }

    IEnumerator DeclineSequence()
    {
        theAccpectAndDeclineHolder.SetActive(false);
        yield return StartCoroutine(TypeText(currentCustomer.declineLine));
        yield return new WaitForSeconds(2f);

        float time = 0f;
        float duration = 1f;

        Color imageColor = CustomerImage.color;
        float initialAlphaImage = imageColor.a;

        float initialAlphaBubble = SpeakBubbleGroup.alpha;

        Color nameColor = nameText.color;
        float initialAlphaName = nameColor.a;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            imageColor.a = Mathf.Lerp(initialAlphaImage, 0f, t);
            nameColor.a = Mathf.Lerp(initialAlphaName, 0f, t);
            CustomerImage.color = imageColor;
            nameText.color = nameColor;
            SpeakBubbleGroup.alpha = Mathf.Lerp(initialAlphaBubble, 0f, t);
            yield return null;
        }

        nameText.text = "";
        currentCustomer = null;
        SpeakText.text = "";
        yield return new WaitForSeconds(4f);
        theButton.SetActive(true);
        knock.Activate();
        AudioManager.Instance.PlaySFX("Knock");
    }

    IEnumerator TypeText(string text)
    {
        SpeakText.text = "";
        int logInterval = Random.Range(6, 10);
        int counter = 0;

        for (int i = 0; i < text.Length; i++)
        {
            SpeakText.text += text[i];
            counter++;

            if (counter >= logInterval)
            {
                if(currentIsMale)
                {
                    AudioManager.Instance.PlaySFX("MSpeak");
                }
                else
                {
                    AudioManager.Instance.PlaySFX("FSpeak");
                }
                counter = 0;
                logInterval = Random.Range(6, 10);
            }

            float delay = Input.GetMouseButton(1) ? 0.01f : 0.04f;
            yield return new WaitForSeconds(delay);
        }
    }
}
