using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplierSlot : MonoBehaviour
{
    public float multiplierValue; // Set the multiplier value in the Inspector
    public MoneyManager moneyManager;
    public string theSound = "0X";

    public void Multiplymoney()
    {
        //do math
        AudioManager.Instance.PlaySFX(theSound);
        moneyManager = FindObjectOfType<MoneyManager>();
        moneyManager.theMoneyAmount = (int)(moneyManager.theMoneyAmount * multiplierValue);
        moneyManager.UpdateTheText();
    }
}
