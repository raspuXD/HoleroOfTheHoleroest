using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UseText : MonoBehaviour
{
    public TMP_Text uiText;

    public void WriteTheText(string text)
    {
        uiText.text = text;
    }

    public void UnWriteTheText()
    {
        uiText.text = "";
    }
}
