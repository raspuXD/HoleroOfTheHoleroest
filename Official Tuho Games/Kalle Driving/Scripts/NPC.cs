using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string whatDoSay;
    public bool isTheLastString = false;
}

public class NPC : MonoBehaviour
{
    public GameObject theHuutoMerkki, puheKupla, countinueObject;
    public TMP_Text theTeksti;
    public List<Dialogue> dialogues;
    private int currentDialogueIndex = 0;
    private bool isTyping = false;
    private bool playerNear = false;
    bool hasAcceptedTask = false;
    public bool isMissionOn = false;
    public GameObject timerHolder;
    public TMP_Text timerText;
    public MoneyManager moneyManager;
    public NPC[] otherNPCS;

    private float tickTimer = 1f;

    [Header("Mitsulla Petteriä Minuutti")]
    public GameObject taskHudMitsu;
    public bool mitsullaPetterinPaalta;
    public int montaMitsullaTapettu = 0;
    public TMP_Text theKillText;
    public float timePassed;

    [Header("5 asiaa")]
    public GameObject taskHudCollect;
    public GameObject pystit;
    public bool viisiAsiaa;
    public int howManyCurrentlyCollected = 0;
    public TMP_Text theAmountText;

    [Header("Karpas Latka Petteriä Minuutti")]
    public GameObject taskHudSwatter;
    public bool karpasLatkallaPetteria;
    public int montaKarpasLatkallaTapettu = 0;
    public TMP_Text theSwatterText;

    private void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E) && !isTyping && !hasAcceptedTask)
        {
            if (currentDialogueIndex < dialogues.Count - 1)
            {
                currentDialogueIndex++;
                countinueObject.SetActive(false);
                StartCoroutine(DisplayDialogue());
            }
            else
            {
                hasAcceptedTask = true;
                isMissionOn = true;
                puheKupla.SetActive(false);
                theHuutoMerkki.SetActive(false);
                countinueObject.SetActive(false);

                if (mitsullaPetterinPaalta)
                {
                    taskHudMitsu.SetActive(true);
                    timerHolder.SetActive(true);
                    timePassed = 120f;
                }
                else if (viisiAsiaa)
                {
                    taskHudCollect.SetActive(true);
                    pystit.SetActive(true);
                    timerHolder.SetActive(true);
                    timePassed = 60f;
                }
                else if (karpasLatkallaPetteria)
                {
                    taskHudSwatter.SetActive(true);
                    timerHolder.SetActive(true);
                    timePassed = 60f;
                }
            }
        }

        if (isMissionOn)
        {
            if (timePassed > 0)
            {
                timePassed -= Time.deltaTime;
                tickTimer -= Time.deltaTime;

                if (tickTimer <= 0)
                {
                    AudioManager.Instance.PlaySFX("Tick");
                    tickTimer = 1f;
                }

                timerText.text = Mathf.Ceil(timePassed).ToString();
            }
            else
            {
                AudioManager.Instance.PlaySFX("Fail");
                EndMission();
            }
        }

        if (mitsullaPetterinPaalta && isMissionOn)
        {
            theKillText.text = montaMitsullaTapettu.ToString() + "/20";
            if (montaMitsullaTapettu >= 20) CompleteMission(200);
        }
        else if (viisiAsiaa && isMissionOn)
        {
            theAmountText.text = howManyCurrentlyCollected.ToString() + "/5";
            if (howManyCurrentlyCollected >= 5) CompleteMission(125);
        }
        else if (karpasLatkallaPetteria && isMissionOn)
        {
            theSwatterText.text = montaKarpasLatkallaTapettu.ToString() + "/10";
            if (montaKarpasLatkallaTapettu >= 10) CompleteMission(125);
        }
    }

    private void EndMission()
    {
        isMissionOn = false;
        timerHolder.SetActive(false);
        if(taskHudCollect != null)
        {
            taskHudCollect.SetActive(false);
        }

        if(taskHudMitsu != null)
        {
            taskHudMitsu.SetActive(false);
        }

        if(taskHudSwatter != null)
        {
            taskHudSwatter.SetActive(false);
        }

        if(pystit != null)
        {
            pystit.SetActive(false);
        }
    }

    private void CompleteMission(int reward)
    {
        AudioManager.Instance.PlaySFX("Succeed");
        moneyManager = FindObjectOfType<MoneyManager>();
        moneyManager.GiveMoney(reward);
        EndMission();
    }

    private bool isOthers()
    {
        if (otherNPCS != null)
        {
            foreach (NPC npc in otherNPCS)
            {
                if (npc.isMissionOn)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasAcceptedTask && !isOthers())
        {
            if (other.CompareTag("Player"))
            {
                puheKupla.SetActive(true);
                theHuutoMerkki.SetActive(false);
                playerNear = true;
                StartCoroutine(DisplayDialogue());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!hasAcceptedTask)
        {
            if (other.CompareTag("Player"))
            {
                puheKupla.SetActive(false);
                theHuutoMerkki.SetActive(true);
                countinueObject.SetActive(false);
                playerNear = false;
                StopAllCoroutines();
            }
        }
    }

    private IEnumerator DisplayDialogue()
    {
        if (currentDialogueIndex < dialogues.Count)
        {
            isTyping = true;
            theTeksti.text = "";
            string dialogueText = dialogues[currentDialogueIndex].whatDoSay;
            int nextLogIndex = Random.Range(3, 6);
            int letterCount = 0;

            foreach (char letter in dialogueText)
            {
                theTeksti.text += letter;
                letterCount++;

                if (letterCount >= nextLogIndex)
                {
                    AudioManager.Instance.PlaySFX("Speaking");
                    nextLogIndex = letterCount + Random.Range(3, 6);
                }

                yield return new WaitForSeconds(0.04f);
            }

            yield return new WaitForSeconds(1f);
            isTyping = false;
            countinueObject.SetActive(true);
        }
    }
}