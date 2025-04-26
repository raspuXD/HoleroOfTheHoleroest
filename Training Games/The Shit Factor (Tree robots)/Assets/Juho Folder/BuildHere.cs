using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildHere : MonoBehaviour
{
    public GameObject highLight;
    public GameObject canvas;

    public GameObject tesla, canoon;

    public HumanResource humans;
    public ProcentageManager procents;
    public SpriteRenderer spriteRed;

    private void Start()
    {
        highLight.SetActive(false);
        spriteRed = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                OnInteract();
            }
        }
    }

    private void OnMouseEnter()
    {
        highLight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highLight.SetActive(false);
    }

    void OnInteract()
    {
        canvas.SetActive(!canvas.activeSelf);
    }

    public void BuyCannon()
    {
        if (humans.howManyPeople >= 8 && procents.MetalProcentager >= 40f)
        {
            humans.UseHumans(8);
            canvas.SetActive(false);
            spriteRed.enabled = false;
            canoon.SetActive(true);
            Destroy(this);
        }
    }

    public void BuyTesla()
    {
        if (humans.howManyPeople >= 8 && procents.EnergyProcentager >= 60)
        {
            humans.UseHumans(10);
            canvas.SetActive(false);
            spriteRed.enabled = false;
            tesla.SetActive(true);
            Destroy(this);
        }
    }
}
