using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GotTheEnding : MonoBehaviour
{
    public string playerPefs;
    Image thisImage;
    public Color ifDoesntHave;

    private void Start()
    {
        thisImage = GetComponent<Image>();
        int hasIt = PlayerPrefs.GetInt(playerPefs, 0);

        if(hasIt == 0)
        {
            thisImage.color = ifDoesntHave;
        }
        else
        {
            thisImage.color = Color.white;
        }
    }
}
