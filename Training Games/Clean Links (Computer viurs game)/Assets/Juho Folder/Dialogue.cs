using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HEHEHEH
{
    public string theDialogue;
    public Sprite[] theFaces;
    public string nameForAudio = "ShortTalk";
}

public class Dialogue : MonoBehaviour
{
    public HEHEHEH[] dialogues;
    public SpriteRenderer theFace;
    public TMP_Text dialogueText;
    public Sprite evil;

    private Coroutine currentDialogueCoroutine;
    private Coroutine faceChangeCoroutine;
    private bool canProceedToNextDialogue = false;
    private bool isFaceChangingActive = true; // Flag to control face changing
    public GameObject theCountinue;
    public GameObject thisObject, desktop;
    public TheClock theClock;
    private void Start()
    {
        StartDialogue(0);
    }

    public void StartDialogue(int index)
    {
        if (index < 0 || index >= dialogues.Length)
        {
            Debug.LogError("Invalid dialogue index.");
            return;
        }

        isFaceChangingActive = true;

        // Stop any ongoing coroutines
        if (currentDialogueCoroutine != null)
        {
            StopCoroutine(currentDialogueCoroutine);
        }
        if (faceChangeCoroutine != null)
        {
            StopCoroutine(faceChangeCoroutine);
        }

        // Start the coroutines
        currentDialogueCoroutine = StartCoroutine(PlayDialogue(index));
        faceChangeCoroutine = StartCoroutine(ChangeFaces(index));
    }

    private IEnumerator PlayDialogue(int index)
    {
        HEHEHEH dialogue = dialogues[index];
        dialogueText.text = "";

        // Loop through each letter in the dialogue string
        AudioManager.Instance.PlaySFX(dialogue.nameForAudio);

        foreach (char letter in dialogue.theDialogue)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f); // Adjust typing speed here
        }

        isFaceChangingActive = false;
        theFace.sprite = evil;
        // Wait 2 seconds after the text finishes typing
        yield return new WaitForSeconds(2f);

        // Stop face-changing when dialogue is complet

        // Enable the ability for the player to click to continue
        theCountinue.SetActive(true);
        canProceedToNextDialogue = true;

        // Set the "evil" face after dialogue finishes
    }

    private IEnumerator ChangeFaces(int index)
    {
        HEHEHEH dialogue = dialogues[index];
        Sprite[] faces = dialogue.theFaces;

        if (faces.Length == 0)
        {
            yield break;
        }

        int faceIndex = 0;

        while (isFaceChangingActive) // Check if face-changing is still active
        {
            theFace.sprite = faces[faceIndex];
            faceIndex = (faceIndex + 1) % faces.Length;
            yield return new WaitForSeconds(0.2f); // Adjust face change speed here
        }
    }

    private void Update()
    {
        if (canProceedToNextDialogue && Input.GetMouseButtonDown(0)) // Wait for a click (left mouse button)
        {
            // Find the next dialogue index
            int nextIndex = Array.FindIndex(dialogues, d => d == dialogues[Array.FindIndex(dialogues, x => x.theDialogue == dialogueText.text)]) + 1;

            if (nextIndex < dialogues.Length)
            {
                StartDialogue(nextIndex); // Start the next dialogue
                theCountinue.SetActive(false);
                canProceedToNextDialogue = false; // Reset the flag until the next dialogue is fully typed
            }
            else
            {
                Debug.Log("Oh no, there is no next dialogue!");
                thisObject.SetActive(false);
                desktop.SetActive(true);
                theClock.StartCountdown();
            }
        }
    }
}
