using UnityEngine;
using UnityEngine.UI;  // Import the UI namespace

public class ImageChecker : MonoBehaviour
{
    public Sprite og;
    public Sprite correctImage;  // The sprite to check against as "correct"
    public Sprite wrongImage;    // The sprite to check against as "wrong"

    public GameObject puzzle1;
    public Image[] theanswerCircle;

    public GameObject desktop;
    public GameObject puzzleItself;

    public void Answer1(bool what)
    {
        theanswerCircle[0].sprite = what ? correctImage : wrongImage;
        if (what)
        {
            AudioManager.Instance.PlaySFX("Correct");
        }
        else
        {
            AudioManager.Instance.PlaySFX("Wrong");
        }
    }

    public void Answer2(bool what)
    {
        theanswerCircle[1].sprite = what ? correctImage : wrongImage;
        if (what)
        {
            AudioManager.Instance.PlaySFX("Correct");
        }
        else
        {
            AudioManager.Instance.PlaySFX("Wrong");
        }
    }

    public void Answer3(bool what)
    {
        theanswerCircle[2].sprite = what ? correctImage : wrongImage;
        if (what)
        {
            AudioManager.Instance.PlaySFX("Correct");
        }
        else
        {
            AudioManager.Instance.PlaySFX("Wrong");
        }
    }

    public void Answer4(bool what)
    {
        theanswerCircle[3].sprite = what ? correctImage : wrongImage;
        if(what)
        {
            AudioManager.Instance.PlaySFX("Correct");
        }
        else
        {
            AudioManager.Instance.PlaySFX("Wrong");
        }
    }

    public void Answer5(bool what)
    {
        theanswerCircle[4].sprite = what ? correctImage : wrongImage;
        if (what)
        {
            AudioManager.Instance.PlaySFX("Correct");
        }
        else
        {
            AudioManager.Instance.PlaySFX("Wrong");
        }
        CheckChildrenImages();
    }

    public void CheckChildrenImages()
    {
        int wrongImagesCount = 0;

        foreach (Image theImage in theanswerCircle)
        {
            if (theImage.sprite == wrongImage)
            {
                wrongImagesCount++;
            }
        }

        if (wrongImagesCount >= 2)
        {
            // Player loses
            puzzle1.SetActive(true);
            foreach (Image image in theanswerCircle)
            {
                image.sprite = og;
            }
        }
        else
        {
            // Player wins
            Debug.Log("YOU WIN");
            desktop.SetActive(true);
            puzzleItself.SetActive(false);
        }
    }
}
