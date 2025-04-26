using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FadeIn : MonoBehaviour
{
    public Image fadeImage;
    public float HowFast = 1f;
    public float howLongWaitUntil = 1f;
    public UnityEvent onFadeComplete;

    bool isFading = false;
    bool hasFaded = false;
    float timer = 0f;

    void Start()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    void Update()
    {
        if (isFading && fadeImage != null)
        {
            timer += Time.deltaTime / HowFast;
            Color c = fadeImage.color;
            c.a = Mathf.Clamp01(timer);
            fadeImage.color = c;

            if (timer >= 1f)
            {
                isFading = false;
                hasFaded = true;
                StartCoroutine(WaitThenInvoke());
            }
        }
    }

    public void StartFade()
    {
        if (!isFading && !hasFaded)
        {
            timer = 0f;
            isFading = true;

            if (fadeImage != null)
            {
                fadeImage.gameObject.SetActive(true);
                fadeImage.enabled = true;

                Animator anim = fadeImage.GetComponent<Animator>();
                anim.enabled = false;

                Color c = fadeImage.color;
                c.a = 0f;
                fadeImage.color = c;
            }
        }
    }

    System.Collections.IEnumerator WaitThenInvoke()
    {
        yield return new WaitForSeconds(howLongWaitUntil);
        onFadeComplete.Invoke();
    }
}
