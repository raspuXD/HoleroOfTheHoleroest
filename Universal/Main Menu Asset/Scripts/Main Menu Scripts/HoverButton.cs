using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Button button;
    bool lerp;
    float timer;
    [SerializeField] Vector3 desiredScale = new Vector3(1.1f, 1.1f, 1.1f);
    Vector3 ogScale;

    public TMP_Text buttonText;
    public bool wantTheTextToBeCool = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        timer = 0;
        lerp = true;
        if (wantTheTextToBeCool)
        {
            DoStuffToText();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        timer = 0;
        lerp = false;
        if (wantTheTextToBeCool)
        {
            ReverTheTextChanges();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Scale back to original size when button is clicked
        button.transform.localScale = ogScale;
    }

    void Start()
    {
        button = GetComponent<Button>();
        if (wantTheTextToBeCool)
        {
            buttonText = GetComponentInChildren<TMP_Text>();
        }

        ogScale = button.transform.localScale;
    }

    public void DoStuffToText()
    {
        // Underline the text
        buttonText.fontStyle |= FontStyles.Underline;
    }

    public void ReverTheTextChanges()
    {
        // Remove underline from text
        buttonText.fontStyle &= ~FontStyles.Underline;
    }

    void Update()
    {
        if (lerp)
        {
            button.transform.localScale = Vector3.Lerp(button.transform.localScale, desiredScale, timer);
            timer += Time.deltaTime;
        }
        else
        {
            button.transform.localScale = Vector3.Lerp(button.transform.localScale, ogScale, timer);
            timer += Time.deltaTime;
        }
    }
}
