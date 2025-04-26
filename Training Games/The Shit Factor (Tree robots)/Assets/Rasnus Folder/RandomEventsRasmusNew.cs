using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RandomEventsRasmusNew : MonoBehaviour
{
    [Header("Human Resource Reference")]
    [SerializeField] private HumanResource humanResource;

    [Header("UI Elements")]
    [SerializeField] private GameObject eventMenu;
    [SerializeField] private TMP_Text eventTitle;
    [SerializeField] private TMP_Text eventDescription;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;
    private bool isEventActive = false;
    public RectTransform scaleTo;

    public float minEventInterval = 5f;
    public float maxEventInterval = 10f;

    public bool CanDoRandomEvent = true;
    [SerializeField] TwoD_Camera camr;

    [System.Serializable]
    public class BadRandomEvent
    {
        public string title;
        [TextArea(1, 10)] public string description;
        [TextArea(1, 10)] public string acceptDesscription;
        public int acceptLoss;
        public int declineMinLoss;
        public int declineMaxLoss;
        [TextArea(1, 10)] public string declineDesscription;
        public bool reusable; // <- NEW
    }

    [System.Serializable]
    public class GoodRandomEvent
    {
        public string title;
        [TextArea(1, 10)] public string description;
        [TextArea(1, 10)] public string acceptDesscription;
        public int acceptGain;
        public string declineDesscription;
        public bool reusable; // <- NEW
    }



    public List<BadRandomEvent> badEvents = new List<BadRandomEvent>();
    public List<GoodRandomEvent> goodEvents = new List<GoodRandomEvent>();

    [Header("Floating Text")]
    [SerializeField] private FloatingText floatingTextPrefab;

    private void Start()
    {
        StartCoroutine(RandomEventTimer());
        Debug.Log("In start calling startcoroutine");
    }

    private IEnumerator RandomEventTimer()
    {
        Debug.Log("RandomEventTimer started");

        while (true) // Always keep the coroutine running
        {
            if (CanDoRandomEvent && !isEventActive)
            {
                Debug.Log("Waiting before next random event...");
                float waitTime = Random.Range(minEventInterval, maxEventInterval);
                yield return new WaitForSeconds(waitTime);

                if (CanDoRandomEvent && !isEventActive) // Double-check again after wait
                {
                    TriggerRandomEvent();
                    Debug.Log("Triggering random event");
                }
            }

            yield return null; // Yield every frame if event not allowed yet
        }
    }


    private void TriggerRandomEvent()
    {
        isEventActive = true;
        camr.canMove = false;
        OpenEventMenu();

        Debug.Log("Triggering RandomEvent");

        bool isBadEvent = Random.value > 0.5f;

        if (isBadEvent && badEvents.Count > 0)
        {
            int index = Random.Range(0, badEvents.Count);
            BadRandomEvent selectedEvent = badEvents[index];

            eventTitle.text = selectedEvent.title;
            eventDescription.text = selectedEvent.description;

            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(() => AcceptBadEvent(selectedEvent));

            declineButton.onClick.RemoveAllListeners();
            declineButton.onClick.AddListener(() => DeclineBadEvent(selectedEvent));

            if (!selectedEvent.reusable)
            {
                badEvents.RemoveAt(index); // <- Remove if not reusable
            }
        }
        else if (!isBadEvent && goodEvents.Count > 0)
        {
            int index = Random.Range(0, goodEvents.Count);
            GoodRandomEvent selectedEvent = goodEvents[index];

            eventTitle.text = selectedEvent.title;
            eventDescription.text = selectedEvent.description;

            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(() => AcceptGoodEvent(selectedEvent));

            declineButton.onClick.RemoveAllListeners();
            declineButton.onClick.AddListener(() => DeclineGoodEvent(selectedEvent));

            if (!selectedEvent.reusable)
            {
                goodEvents.RemoveAt(index); // <- Remove if not reusable
            }
        }
        else
        {
            // No more events
            Debug.Log("No more events left in the list.");
            CloseEventMenu();
            isEventActive = false;
            camr.canMove = true;
        }
    }



    private void AcceptBadEvent(BadRandomEvent selectedEvent)
    {
        Time.timeScale = 1f;
        CloseEventMenu();
        isEventActive = false;
        camr.canMove = true;
        Debug.Log("Triggering AcceptingBad");
        if (humanResource.howManyPeople > 5)
        {
            int lostPeople = selectedEvent.acceptLoss;
            humanResource.UseHumans(lostPeople);
            ShowFloatingText(selectedEvent.acceptDesscription + " " + lostPeople + " people");
        }
        else
        {
            Debug.Log("Not enough people to send to handle the event.");
        }
    }

    private void DeclineBadEvent(BadRandomEvent selectedEvent)
    {
        Time.timeScale = 1f;
        Debug.Log("Declinign BadEvent");
        CloseEventMenu();
        isEventActive = false;
        camr.canMove = true;
        int lostPeople = Random.Range(selectedEvent.declineMinLoss, selectedEvent.declineMaxLoss);
        humanResource.UseHumans(lostPeople);
        ShowFloatingText(selectedEvent.declineDesscription + " " + lostPeople + " people");
    }

    private void AcceptGoodEvent(GoodRandomEvent selectedEvent)
    {
        Time.timeScale = 1f;
        Debug.Log("Accepting Good");
        CloseEventMenu();
        isEventActive = false;
        camr.canMove = true;
        int gainedPeople = selectedEvent.acceptGain;
        humanResource.GetHumans(gainedPeople); // Assuming you have an AddHumans method
        ShowFloatingText(selectedEvent.acceptDesscription + " " + gainedPeople + " people");
    }

    private void DeclineGoodEvent(GoodRandomEvent selectedEvent)
    {
        Time.timeScale = 1f;
        Debug.Log("Declinging Good");
        CloseEventMenu();
        isEventActive = false;
        camr.canMove = true;
        ShowFloatingText(selectedEvent.declineDesscription);
    }

    private void ShowFloatingText(string message)
    {
        StartCoroutine(DelayedFloatingText(message, 1f));
    }

    private IEnumerator DelayedFloatingText(string message, float delay)
    {
        yield return new WaitForSeconds(delay);

        FloatingText floatingText = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        floatingText.ShowText(message);
    }

    private void OpenEventMenu()
    {
        eventMenu.SetActive(true);
        StartCoroutine(ScaleUI(scaleTo, Vector3.one, 0.3f, () => {
            Time.timeScale = 0f;
        }));
    }


    private void CloseEventMenu()
    {
        Debug.Log("Closing EventMenu");
        StartCoroutine(ScaleUI(scaleTo, Vector3.zero, 0.3f, () =>
        {
            eventMenu.SetActive(false);
        }));
    }

    public void CancelCurrentEvent()
    {
        Debug.Log("cancel EventMenu");
        if (!isEventActive) return;

        Debug.Log("Closing if open menu EventMenu");
        CloseEventMenu();
        StartCoroutine(RandomEventTimer());
        isEventActive = false; 
        camr.canMove = true;
        Time.timeScale = 1f;
    }


    private IEnumerator ScaleUI(RectTransform target, Vector3 targetScale, float duration, System.Action onComplete = null)
    {
        Vector3 initialScale = target.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            yield return null;
        }

        target.localScale = targetScale;

        if (onComplete != null)
            onComplete.Invoke();
    }
}
