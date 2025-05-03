using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public Image thecross;

    public Sprite[] allPossibleSprites;
    public Color[] colors;

    public GameObject thePetteriPickUP;
    
    public void ChangeTheCursor(bool showIt)
    {
        if(showIt)
        {
            thecross.enabled = true;
        }
        else
        {
            thecross.enabled = false;
        }
    }

    public void UpdateTheCrosshair(int whatSprite, int whatColor)
    {
        thecross.sprite = allPossibleSprites[whatSprite];
        thecross.color = colors[whatColor];
    }
}
