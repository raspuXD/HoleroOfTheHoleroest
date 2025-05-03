using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentDrops : MonoBehaviour
{
    public bool isInstakill, isDoublePoints;
    [SerializeField] Image thisImage;
    [SerializeField] GameObject doublePoints, instaKill;
    [SerializeField] TMP_Text doublePointsTimer, instaKillTimer;
    [SerializeField] RoundManager roundManager;
    [SerializeField] Points points;

    Coroutine insta;
    Coroutine doubleP;

    public void ActivateSome(int id)
    {
        if(id == 0)
        {
            if(insta != null)
            {
                StopCoroutine(insta);
            }

            insta = StartCoroutine(Instakill());
        }

        else if(id == 1)
        {
            if (doubleP != null)
            {
                StopCoroutine(doubleP);
            }

            doubleP = StartCoroutine(DoublePoints());
        }
        else
        {
            ResetZombies();
        }
    }

    void ResetZombies()
    {
        if (roundManager.theZombies != null)
        {
            foreach (ZombieHealth zombie in roundManager.theZombies)
            {
                Destroy(zombie.gameObject);
            }
            roundManager.theZombies.Clear();
        }

        points.EarnPoints(400);
    }

    IEnumerator DoublePoints()
    {
        isDoublePoints = true;
        doublePoints.SetActive(true);
        float timer = 30f; // Duration of the effect in seconds

        while (timer > 0)
        {
            doublePointsTimer.text = ((int)timer).ToString();
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        doublePointsTimer.text = ""; // Clear text when the timer ends
        doublePoints.SetActive(false);
        isDoublePoints = false;
    }

    IEnumerator Instakill()
    {
        isInstakill = true;
        instaKill.SetActive(true);
        float timer = 30f; // Duration of the effect in seconds

        while (timer > 0)
        {
            instaKillTimer.text = ((int)timer).ToString();
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }

        instaKillTimer.text = ""; // Clear text when the timer ends
        instaKill.SetActive(false);
        isInstakill = false;
    }
}
