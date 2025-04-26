using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBubble : MonoBehaviour
{
    public InventoryShow show;
    public Inventory inve;
    public GameObject theBubble, theName;
    bool canPressE;

    public bool isKalle, isJuho;

    [Header("Kai Teleport")]
    public Transform whereTo;
    public GameObject playerPref;
    public CowSpawner spawner;
    public BulletSpawner bulletSpawner;
    public GameObject healtBar1, healtBar2, snake, lowTaperFade, hah, ha3, porhelo;



    private void Update()
    {
        if(canPressE)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if (isKalle)
                {
                    if(inve.Money >= 3)
                    {
                        inve.DecreaseMoney(3);
                        inve.IncreaseBullets(4);
                    }
                }
                else if (isJuho)
                {
                    if (inve.Milk >= 1)
                    {
                        inve.DecreaseMilk(1);
                        inve.IncreaseMoney(5);
                    }
                }
                else
                {
                    if(inve.Money >= 100)
                    {
                        inve.DecreaseMoney(100);
                        Teleport();
                    }
                }
            }
        }
    }

    void Teleport()
    {
        playerPref.transform.position = whereTo.position;
        spawner.DestroyAllCows();
        bulletSpawner.enabled = true;
        healtBar1.SetActive(true);
                    healtBar2.SetActive(true);
        hah.SetActive(true);
        ha3.SetActive(true);
        lowTaperFade.SetActive(true);
        snake.SetActive(true);
        porhelo.SetActive(false);
        AudioManager.Instance.ChangeMusic("Boss", 1.5f, 1f);
        Destroy(spawner);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            theBubble.SetActive(true);
            canPressE = true;
            show.ActivateThis();
            theName.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            theBubble.SetActive(false);
            canPressE=false;
            show.HideInventory();
            theName.SetActive(true);
        }
    }
}
