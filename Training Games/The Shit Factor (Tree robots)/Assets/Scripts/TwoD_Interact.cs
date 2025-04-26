using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoD_Interact : MonoBehaviour
{
    public GameObject highLight;
    [SerializeField] private Factories factory;

    private void Start()
    {
        highLight.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
        factory.OpenBuyMenu();
    }
}
