using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectVisual : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Button button;
    bool lerp;
    float timer;
    [SerializeField] Vector3 desiredScale;
    Vector3 ogScale;
    public void OnPointerEnter(PointerEventData eventData)
    {
        timer = 0;
        lerp = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        timer = 0;
        lerp = false;
    }

    void Start()
    {
        button = GetComponent<Button>();
        ogScale = button.transform.localScale;
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