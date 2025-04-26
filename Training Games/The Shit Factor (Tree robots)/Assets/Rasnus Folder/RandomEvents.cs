using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RandomEvents : MonoBehaviour
{
    [Header("Human Resource Reference")]
    [SerializeField] private HumanResource humanResource;

    [Header("UI Elements")]
    [SerializeField] private GameObject eventMenu;
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
        [TextArea(1, 10)]
        public string description;
        [TextArea(1, 10)]
        public string acceptDesscription;
        public int acceptLoss;
        public int declineMinLoss;
        public int declineMaxLoss;
        [TextArea(1, 10)]
        public string declineDesscription;
    }

    [System.Serializable]
    public class GoodRandomEvent
    {
        [TextArea(1, 10)]
        public string description;
        [TextArea(1, 10)]
        public string acceptDesscription;
        public int acceptGain;
        public string declineDesscription;
    }

    public List<BadRandomEvent> badEvents = new List<BadRandomEvent>();
    public List<GoodRandomEvent> goodEvents = new List<GoodRandomEvent>();

    [Header("Floating Text")]
    [SerializeField] private FloatingText floatingTextPrefab;

    private void Start()
    {
        StartCoroutine(RandomEventTimer());
    }

    private IEnumerator RandomEventTimer()
    {
        while (true && CanDoRandomEvent)
        {
            while (isEventActive)
            {
                yield return null;
            }

            float waitTime = Random.Range(minEventInterval, maxEventInterval);
            yield return new WaitForSeconds(waitTime);

            TriggerRandomEvent();
        }
    }

    private void TriggerRandomEvent()
    {
        isEventActive = true;
        camr.canMove = false;
        OpenEventMenu();

        bool isBadEvent = Random.value > 0.5f; // Randomly select between bad and good event

        if (isBadEvent)
        {
            BadRandomEvent selectedEvent = badEvents[Random.Range(0, badEvents.Count)];
            eventDescription.text = selectedEvent.description;

            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(() => AcceptBadEvent(selectedEvent));

            declineButton.onClick.RemoveAllListeners();
            declineButton.onClick.AddListener(() => DeclineBadEvent(selectedEvent));
        }
        else
        {
            GoodRandomEvent selectedEvent = goodEvents[Random.Range(0, goodEvents.Count)];
            eventDescription.text = selectedEvent.description;

            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(() => AcceptGoodEvent(selectedEvent));

            declineButton.onClick.RemoveAllListeners();
            declineButton.onClick.AddListener(() => DeclineGoodEvent(selectedEvent));
        }
    }

    private void AcceptBadEvent(BadRandomEvent selectedEvent)
    {
        CloseEventMenu();
        isEventActive = false;
        camr.canMove = true;

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
        CloseEventMenu();
        isEventActive = false;
        camr.canMove = true;
        int lostPeople = Random.Range(selectedEvent.declineMinLoss, selectedEvent.declineMaxLoss);
        humanResource.UseHumans(lostPeople);
        ShowFloatingText(selectedEvent.declineDesscription + " " + lostPeople + " people");
    }

    private void AcceptGoodEvent(GoodRandomEvent selectedEvent)
    {
        CloseEventMenu();
        isEventActive = false;
        camr.canMove = true;
        int gainedPeople = selectedEvent.acceptGain;
        humanResource.GetHumans(gainedPeople); // Assuming you have an AddHumans method
        ShowFloatingText(selectedEvent.acceptDesscription + " " + gainedPeople + " people");
    }

    private void DeclineGoodEvent(GoodRandomEvent selectedEvent)
    {
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
        StartCoroutine(ScaleUI(scaleTo, Vector3.one, 0.3f));
    }

    private void CloseEventMenu()
    {
        StartCoroutine(ScaleUI(scaleTo, Vector3.zero, 0.3f, () =>
        {
            eventMenu.SetActive(false);
        }));
    }

    public void CancelCurrentEvent()
    {
        if (!isEventActive) return;

        CloseEventMenu();
        isEventActive = false; 
        camr.canMove = true;
    }


    private IEnumerator ScaleUI(RectTransform target, Vector3 targetScale, float duration, System.Action onComplete = null)
    {
        Vector3 initialScale = target.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            target.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            yield return null;
        }

        target.localScale = targetScale;

        if (onComplete != null)
            onComplete.Invoke();
    }
}
