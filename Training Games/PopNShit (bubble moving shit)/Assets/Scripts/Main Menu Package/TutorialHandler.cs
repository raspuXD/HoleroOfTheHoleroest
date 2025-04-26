using UnityEngine;
using UnityEngine.UI;

//This script is nice feature for tutorial, there are pages for each thing we want to show in tutorial and allows key presses so visually you can test if your keys work

[System.Serializable]
public class ControlExample
{
    public Image theKeyImage;
    public Color notPressedColor;
    public Color pressedColor;
    public KeyCode theKey;
}

[System.Serializable]
public class Pages
{
    public int whatPage = 0;
    public GameObject pageObject;
}

public class TutorialHandler : MonoBehaviour
{
    [SerializeField] ControlExample[] controls;
    [SerializeField] Pages[] pages;
    [SerializeField] Button nextPageButton;
    [SerializeField] Button lastPageButton;
    private int currentPageIndex = 0;

    private void Start()
    {
        nextPageButton.onClick.AddListener(ShowNextPage);
        lastPageButton.onClick.AddListener(ShowPreviousPage);
        ShowPage(currentPageIndex);
    }

    public void ChangeThePage(int pageNumber)
    {
        currentPageIndex = pageNumber;
        ShowPage(currentPageIndex);
    }

    private void ShowPage(int pageIndex)
    {
        foreach (var page in pages)
        {
            page.pageObject.SetActive(false);
        }

        pages[pageIndex].pageObject.SetActive(true);
        UpdateButtonInteractivity();
    }

    private void UpdateButtonInteractivity()
    {
        nextPageButton.interactable = currentPageIndex < pages.Length - 1;
        lastPageButton.interactable = currentPageIndex > 0;
    }

    private void ShowNextPage()
    {
        if (currentPageIndex < pages.Length - 1)
        {
            currentPageIndex++;
            ShowPage(currentPageIndex);
        }
    }

    private void ShowPreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowPage(currentPageIndex);
        }
    }

    private void Update()
    {
        foreach (ControlExample control in controls)
        {
            if (Input.GetKeyDown(control.theKey))
            {
                control.theKeyImage.color = control.pressedColor;
            }
            if (Input.GetKeyUp(control.theKey))
            {
                control.theKeyImage.color = control.notPressedColor;
            }
        }
    }
}
