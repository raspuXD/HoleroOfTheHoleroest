using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DayFader : MonoBehaviour
{
    public Image fadeImage;
    public TMP_Text dayText;
    public TMP_Text normalText;

    public GameObject theHolder;

    private string[] dayNames = { "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN" };

    private int currentDay = 0;
    public float dayDuration = 10f;
    private float fadeDuration = 1f;
    [SerializeField] RandomEventsRasmusNew efent;
    [SerializeField] TwoD_Camera camr;
    [SerializeField] EnemySpawner spawner;

    void Start()
    {
        camr.canMove = false;
        efent.CanDoRandomEvent = false;
        SetAlpha(fadeImage, 1f);
        SetAlpha(dayText, 0f);
        SetAlpha(normalText, 0f);
        StartCoroutine(FirstDayStart());
    }

    IEnumerator FirstDayStart()
    {
        dayText.text = dayNames[currentDay];
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeTexts(dayText, normalText, 0f, 1f, fadeDuration));
        yield return new WaitForSeconds(2f);

        AudioManager.Instance.PlaySFX("day");
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = 1f - (t / fadeDuration);
            SetAlpha(fadeImage, a);
            SetAlpha(dayText, a);
            SetAlpha(normalText, a);
            yield return null;
        }

        SetAlpha(fadeImage, 0f);
        SetAlpha(dayText, 0f);
        SetAlpha(normalText, 0f);

        currentDay++;
        efent.CanDoRandomEvent = true;
        camr.canMove = true;
        theHolder.SetActive(false);
        StartCoroutine(DayRoutine());
    }

    IEnumerator DayRoutine()
    {
        while (currentDay < 6)
        {
            yield return new WaitForSeconds(dayDuration - 5f);
            yield return StartCoroutine(PlayFadeSequence(currentDay));
            currentDay++;
        }

        yield return new WaitForSeconds(dayDuration - 5f);
        yield return StartCoroutine(PlayFadeSequence(currentDay)); // currentDay should be 6 here
        currentDay++;

        Debug.Log("Day 8 reached");
        spawner.StartPlaying();
    }


    IEnumerator PlayFadeSequence(int dayIndex)
    {
        efent.CancelCurrentEvent();
        efent.CanDoRandomEvent = false;
        theHolder.SetActive(true);
        camr.canMove = false;
        SetAlpha(fadeImage, 0f);
        SetAlpha(dayText, 0f);
        SetAlpha(normalText, 0f);

        yield return StartCoroutine(FadeImage(fadeImage, 0f, 1f, fadeDuration));

        if (dayIndex < 7)
        {
            dayText.text = dayNames[dayIndex];
        }
        else
        {
            dayText.text = "FIGHT";
        }

        yield return StartCoroutine(FadeTexts(dayText, normalText, 0f, 1f, fadeDuration));

        yield return new WaitForSeconds(2f);

        AudioManager.Instance.PlaySFX("day");
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = 1f - (t / fadeDuration);
            SetAlpha(fadeImage, a);
            SetAlpha(dayText, a);
            SetAlpha(normalText, a);
            yield return null;
        }

        SetAlpha(fadeImage, 0f);
        SetAlpha(dayText, 0f);
        SetAlpha(normalText, 0f);
        efent.CanDoRandomEvent = true;
        camr.canMove = true;
        theHolder.SetActive(false);
    }

    IEnumerator FadeImage(Image img, float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / duration);
            SetAlpha(img, a);
            yield return null;
        }
        SetAlpha(img, to);
    }

    IEnumerator FadeTexts(TMP_Text t1, TMP_Text t2, float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / duration);
            SetAlpha(t1, a);
            SetAlpha(t2, a);
            yield return null;
        }
        SetAlpha(t1, to);
        SetAlpha(t2, to);
    }

    void SetAlpha(Graphic g, float alpha)
    {
        var c = g.color;
        c.a = alpha;
        g.color = c;
    }
}
