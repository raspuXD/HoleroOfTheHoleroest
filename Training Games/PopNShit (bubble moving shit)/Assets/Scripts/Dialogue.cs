using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ThoseNastyStrings
{
    public string dialogue;
}

public class Dialogue : MonoBehaviour
{
    public ThoseNastyStrings[] ifNone, if25, if50, if75, if100;
    public CheckAllItems items;
    public TMP_Text writeTheString;
    public GameObject theCloud;
    Coroutine theWriting;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(theWriting);
            theCloud.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int collectedCount = items.allFurniture.Count;
            int totalCount = items.allItemsInScene.Count;

            if (totalCount == 0) // Avoid division by zero
            {
                Debug.Log("No items in the scene to collect.");
                return;
            }

            float percentage = (float)collectedCount / totalCount * 100;
            string dialogue = "";

            if (percentage == 100)
            {
                int i = Random.Range(0, if100.Length);
                dialogue = if100[i].dialogue;
            }
            else if (percentage >= 75)
            {
                int i = Random.Range(0, if75.Length);
                dialogue = if75[i].dialogue;
            }
            else if (percentage >= 50)
            {
                int i = Random.Range(0, if50.Length);
                dialogue = if50[i].dialogue;
            }
            else if (percentage >= 25)
            {
                int i = Random.Range(0, if25.Length);
                dialogue = if25[i].dialogue;
            }
            else
            {
                int i = Random.Range(0, ifNone.Length);
                dialogue = ifNone[i].dialogue;
            }

            theWriting = StartCoroutine(TypeText(dialogue));
        }
    }

    private IEnumerator TypeText(string text)
    {
        AudioManager.Instance.PlaySFX("Speak");
        theCloud.SetActive(true);
        writeTheString.text = ""; // Clear the text field
        foreach (char letter in text)
        {
            writeTheString.text += letter; // Add one letter at a time
            yield return new WaitForSeconds(0.06f); // Wait for 0.06 seconds
        }
    }
}
