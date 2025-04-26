using TMPro;
using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private TMP_Text textMesh; // Assign in prefab or via script
    [SerializeField] private float floatDuration = 5f; // How long it stays
    [SerializeField] private float floatHeight = 50f; // How high it floats
    [SerializeField] private float floatSpeed = 1f;

    private Vector3 startPosition;
    private float elapsed;

    public void ShowText(string message)
    {
        if (textMesh == null)
            textMesh = GetComponentInChildren<TMP_Text>();

        textMesh.text = message;
        startPosition = transform.position;

        StartCoroutine(FloatUpAndDown());
    }

    private IEnumerator FloatUpAndDown()
    {
        while (elapsed < floatDuration)
        {
            elapsed += Time.deltaTime;

            // Simple float up and down animation
            float yOffset = Mathf.Sin(elapsed * floatSpeed) * 5f;
            transform.position = startPosition + new Vector3(0, yOffset, 0);

            yield return null;
        }

        Destroy(gameObject);
    }
}
