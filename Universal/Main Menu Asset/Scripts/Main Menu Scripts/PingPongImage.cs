using UnityEngine;
using UnityEngine.UI;

public class PingPongImage : MonoBehaviour
{
    public Image uiImage;
    public float maxScale = 1f;
    public float minScale = 0.75f;
    public float speed = 1f;

    Vector3 originalScale;
    float startTime;
    float currentScale;

    void Start()
    {
        if (uiImage == null)
        {
            uiImage = GetComponent<Image>();
        }

        originalScale = transform.localScale;
        startTime = Time.time;
        currentScale = maxScale;
    }

    void Update()
    {
        if (uiImage != null)
        {
            float elapsed = Time.time - startTime;
            float t = Mathf.PingPong(elapsed * speed, 1f);
            currentScale = Mathf.Lerp(maxScale, minScale, t);

            transform.localScale = originalScale * currentScale;
        }
    }
}
