using UnityEngine;

public class PointSystem : MonoBehaviour
{
    public bool usesPoints, usesStars, usesTime;

    public int points = 0;
    public int stars = 0;
    public float timePassed = 0f;

    public int[] howManyPointsPerStar;
    private bool[] starAwarded;

    private void Start()
    {
        starAwarded = new bool[howManyPointsPerStar.Length];
        stars = 0;
    }

    private void Update()
    {
        if (usesTime)
        {
            timePassed += Time.deltaTime;
        }
    }

    public void AddPoints(int amount)
    {
        points += amount;

        if (usesStars)
        {
            CalculateStars();
        }
    }

    private void CalculateStars()
    {
        for (int i = 0; i < howManyPointsPerStar.Length; i++)
        {
            if (points >= howManyPointsPerStar[i] && !starAwarded[i])
            {
                stars++;
                starAwarded[i] = true;
            }
        }

        stars = Mathf.Clamp(stars, 0, howManyPointsPerStar.Length);
    }

    public void REMEMBERTOCALLIFSCENESWITCH()
    {
        if (usesPoints)
        {
            int lastHighScore = PlayerPrefs.GetInt("HighestPoints", 0);

            if (points > lastHighScore)
            {
                PlayerPrefs.SetInt("HighestPoints", points);
            }

            PlayerPrefs.SetInt("HowManyPoints", points);
        }
        if (usesStars)
        {
            int lastHighStar = PlayerPrefs.GetInt("HighestStars", 0);

            if (stars > lastHighStar)
            {
                PlayerPrefs.SetInt("HighestStars", stars);
            }

            PlayerPrefs.SetInt("HowManyStars", stars);
        }
        if (usesTime)
        {
            float lastHighTime = PlayerPrefs.GetFloat("HighestTime", 0);

            if (timePassed > lastHighTime)
            {
                PlayerPrefs.SetFloat("HighestTime", timePassed);
            }

            PlayerPrefs.SetFloat("HowLong", timePassed);
        }

        PlayerPrefs.Save();
    }
}