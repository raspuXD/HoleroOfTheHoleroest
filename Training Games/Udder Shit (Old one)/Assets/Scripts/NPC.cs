using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Inventory theInventoty;
    public SceneHandler sceneHandler;
    public PointSystem points;
    public GameObject theName, theSpeakBubble;
    public string whatToSayKai, whatToSayJuho, whatToSayKalle, whatToSayPetteri;
    public TMP_Text speakText;

    public AudioSource source;

    public bool isKai, isJuho, isKalle, isPetteri;
    bool canInteract;

    private void Start()
    {
        theInventoty = FindObjectOfType<Inventory>();
        sceneHandler = FindObjectOfType<SceneHandler>();
        points = FindObjectOfType<PointSystem>();

    }

    private void Update()
    {
        if(canInteract)
        {
            if(isKai)
            {
                speakText.text = whatToSayKai;
                if(Input.GetKeyDown(KeyCode.E))
                {
                    if(points.points >= 200)
                    {
                        points.REMEMBERTOCALLIFSCENESWITCH();
                        sceneHandler.LoadSceneNamed("win");
                    }
                }
            }
            if (isJuho)
            {
                speakText.text = whatToSayJuho;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (theInventoty.Milk >= 1)
                    {
                        theInventoty.Milk--;
                        points.points += 5;
                    }
                }
            }
            if (isKalle)
            {
                speakText.text = whatToSayKalle;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (points.points >= 3)
                    {
                        source.Play();
                        theInventoty.Bullets++;
                        points.points -= 3;
                    }
                }
            }
            if (isPetteri)
            {
                speakText.text = whatToSayPetteri;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if(theInventoty.Insanity >= 5)
                    {
                        theInventoty.Insanity -= 5;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            canInteract = true;
            theName.SetActive(false);
            theSpeakBubble.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
            theName.SetActive(true);
            theSpeakBubble.SetActive(false);
        }
    }
}
