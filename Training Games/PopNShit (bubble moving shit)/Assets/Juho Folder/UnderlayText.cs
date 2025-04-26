using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UnderlayText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text textMeshPro;

    private string originalText;
    private string underlinedText;

    private void Start()
    {
        textMeshPro = GetComponent<TMP_Text>();
        originalText = textMeshPro.text;
        underlinedText = "<b>" + originalText + "</b>";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Add underline when mouse enters
        textMeshPro.text = underlinedText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Remove underline when mouse exits
        textMeshPro.text = originalText;
    }
}
