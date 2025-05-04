using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoD_Interact : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    [SerializeField] public Color hoverColor;
    [SerializeField] public Color selectedColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
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
        spriteRenderer.color = hoverColor;
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }

    void OnInteract()
    {
        Debug.Log("Item interacted with!");
    }
}
