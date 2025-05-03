using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectRotator : MonoBehaviour
{
    public Button targetButton;
    private bool isHovering = false;
    private float timer = 0f;
    private float callInterval = 0.2f;
    public Transform theCar;
    public bool isWhat;

    void Start()
    {
        if (targetButton != null)
        {
            EventTrigger trigger = targetButton.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { isHovering = true; });
            trigger.triggers.Add(entryEnter);

            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { isHovering = false; });
            trigger.triggers.Add(entryExit);
        }
    }

    void Update()
    {
        if (isHovering)
        {
            timer += Time.deltaTime;
            if (timer >= callInterval)
            {
                RotateObject(isWhat);
                timer = 0f;
            }
        }
        else
        {
            timer = 0f;
        }
    }

    public void RotateObject(bool clockwise)
    {
        float direction = clockwise ? 1f : -1f;
        theCar.Rotate(Vector3.up, direction * 10f);
    }
}
