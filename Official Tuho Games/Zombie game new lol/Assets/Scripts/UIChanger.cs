using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIChanger : MonoBehaviour
{
    public TMP_Text useText;
    public Image crossHair;

    public void WriteTheText(string text, float speed)
    {
        StopAllCoroutines();
        useText.text = "";
        StartCoroutine(TypeText(text, speed));
    }

    public void UnWriteTheText(float speed)
    {
        StopAllCoroutines();
        StartCoroutine(UnTypeText(speed));
    }

    public void AppearTheText(string text)
    {
        StopAllCoroutines();
        useText.text = text;
    }

    public void EraseTheText()
    {
        StopAllCoroutines();
        useText.text = "";
    }

    private IEnumerator TypeText(string text, float speed)
    {
        useText.text = "";  // Clear the text first
        foreach (char letter in text)
        {
            useText.text += letter;  // Add one letter at a time
            yield return new WaitForSeconds(speed);  // Wait for the specified speed before adding the next letter
        }
    }

    private IEnumerator UnTypeText(float speed)
    {
        while (useText.text.Length > 0)
        {
            useText.text = useText.text.Substring(0, useText.text.Length - 1);  // Remove one letter at a time
            yield return new WaitForSeconds(speed);  // Wait for the specified speed before removing the next letter
        }
    }

    public void ChangeCrossHairColor(Color color)
    {
        crossHair.color = color;
    }
}
