using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerSwitch : MonoBehaviour
{
    public bool isPowerOn = false;
    bool canTurnPowerOn;
    Animator animat;

    [SerializeField] ParticleSystem sparks;
    [SerializeField] TMP_Text stateText;
    [SerializeField] Color color;

    UIChanger textChanger;

    private void Start()
    {
        animat = GetComponent<Animator>();
    }

    private void Update()
    {
        //change to use custom keys
        if(canTurnPowerOn && Input.GetKeyDown(KeyCode.E) && isPowerOn == false)
        {
            isPowerOn = true;
            textChanger.UnWriteTheText(.02f);
            textChanger.ChangeCrossHairColor(Color.white);
            //play voice and animation
            animat.SetTrigger("Activate");
            sparks.Play();
            stateText.text = "ON";
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            textChanger = other.GetComponent<UIChanger>();

            if (isPowerOn)
            {
                textChanger.WriteTheText("Power Is On Already", 0.04f);
            }
            else
            {
                canTurnPowerOn = true;
                textChanger.ChangeCrossHairColor(color);
                textChanger.WriteTheText("Turn Power On", 0.04f);
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textChanger.UnWriteTheText(.02f);
            textChanger.ChangeCrossHairColor(Color.white);
            canTurnPowerOn = false;
        }
    }
}
