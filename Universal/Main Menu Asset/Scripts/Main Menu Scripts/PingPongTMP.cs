using TMPro;
using UnityEngine;

public class PingPongTMP : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public float maxSize = 100f;
    public float smallestSize = 75f;
    public float speed = 1f;

    string baseText;
    float startTime;

    void Start()
    {
        if (tmpText == null)
        {
            tmpText = GetComponent<TextMeshProUGUI>();
        }

        baseText = tmpText.text;
        startTime = Time.time;
    }

    void Update()
    {
        if (tmpText != null)
        {
            float elapsed = Time.time - startTime;
            float t = Mathf.PingPong(elapsed * speed, 1f);
            float currentSize = Mathf.Lerp(maxSize, smallestSize, t);
            tmpText.text = $"<size={currentSize}%>{baseText}";
        }
    }
}
