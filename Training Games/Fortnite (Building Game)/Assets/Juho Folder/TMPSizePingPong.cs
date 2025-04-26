using UnityEngine;
using TMPro;

public class TMPSizePingPong : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public float maxSize = 150f;
    public float smallestSize = 100f;
    public float speed = 1f;

    string baseText;

    void Start()
    {
        if (tmpText != null)
        {
            baseText = tmpText.text;
        }
    }

    void Update()
    {
        if (tmpText != null)
        {
            float t = Mathf.PingPong((Time.time * speed) + 1f, 1f); // +1f ensures t starts at 0
            float currentSize = Mathf.Lerp(maxSize, smallestSize, t);
            tmpText.text = $"<size={currentSize}%>{baseText}";
        }
    }
}
