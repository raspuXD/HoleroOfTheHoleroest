using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PiecePlacer : MonoBehaviour
{
    public LayerMask pieceLayer;
    public float rotationSpeed = 15f;
    public float shakeAmount = 0.1f;
    public float randomRotationSpeed = 30f;
    public float shakeDuration = 0.2f;
    public float shakeCooldownMin = 0.5f;
    public float shakeCooldownMax = 2f;

    private Vector3 offset;
    private bool dragging = false;
    private Collider2D pieceCollider;
    private SpriteRenderer spriteRenderer;

    private bool isShaking = false;
    private float shakeTimer = 0f;
    private float shakeCooldownTimer = 0f;

    public bool CanPickup = true;

    public Color locked;

    public bool Placed = false;

    public bool frame, tile, metal;

    void Start()
    {
        pieceCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        
    }

    void Update()
    {
        HandleRotation();

        if (dragging)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;
            Vector3 shake = Vector3.zero;

            if (isShaking)
            {
                shakeTimer -= Time.deltaTime;
                if (shakeTimer <= 0f)
                {
                    isShaking = false;
                    shakeCooldownTimer = UnityEngine.Random.Range(shakeCooldownMin, shakeCooldownMax);
                }
                else
                {
                    shake = new Vector3(
                        UnityEngine.Random.Range(-shakeAmount, shakeAmount),
                        UnityEngine.Random.Range(-shakeAmount, shakeAmount),
                        0f
                    );

                    RandomShake();
                }
            }
            else
            {
                shakeCooldownTimer -= Time.deltaTime;
                if (shakeCooldownTimer <= 0f)
                {
                    isShaking = true;
                    shakeTimer = shakeDuration;
                }
            }

            transform.position = mouseWorld + offset + shake;
        }
    }

    public void RandomShake()
    {
        float randomAngle = UnityEngine.Random.Range(-randomRotationSpeed, randomRotationSpeed) * Time.deltaTime;
        transform.Rotate(Vector3.forward * randomAngle);
    }

    void OnMouseDown()
    {
        if (CanPickup)
        {
            RandomShake();

            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = transform.position - new Vector3(mouseWorld.x, mouseWorld.y, 0f);
            dragging = true;

            shakeCooldownTimer = UnityEngine.Random.Range(shakeCooldownMin, shakeCooldownMax);
            isShaking = false;
        }
    }

    void OnMouseUp()
    {
        dragging = false;

        PlacementArea bestArea = FindBestPlacementArea();

        if (bestArea != null)
        {
            float score = bestArea.GetOverlapScore(gameObject);

            /* Optional: Snap to area if score is high enough
            if (score > 0.8f)
            {
                transform.position = bestArea.transform.position;
            } */
        }
        else
        {
            Debug.Log("No suitable placement area found.");
        }

        CanPickup = false;
        Placed = true;

        spriteRenderer.color = locked;

        if (metal)
        {
            AudioManager.Instance.PlaySFX("Metal");
        }
        if(tile)
        {
            AudioManager.Instance.PlaySFX("Tile");
        }
        if(frame)
        {
            AudioManager.Instance.PlaySFX("Frame");
        }
    }

    PlacementArea FindBestPlacementArea()
    {
        PlacementArea[] allAreas = FindObjectsOfType<PlacementArea>();
        PlacementArea bestArea = null;
        float bestScore = 0f;

        foreach (var area in allAreas)
        {
            float score = area.GetOverlapScore(gameObject);

            if (score > bestScore)
            {
                bestScore = score;
                bestArea = area;
            }
        }

        return bestArea;
    }

    void HandleRotation()
    {
        if (dragging)
        {
            float rotateInput = 0f;

            if (Input.GetKey(KeyCode.Q))
                rotateInput += 1f;
            if (Input.GetKey(KeyCode.E))
                rotateInput -= 1f;

            transform.Rotate(Vector3.forward * rotateInput * rotationSpeed * Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
{
    if (GetComponent<Collider2D>() != null)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center, GetComponent<Collider2D>().bounds.size);
    }
}

}
