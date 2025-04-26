using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VirusBreaker : MonoBehaviour
{
    public float speed = 5f; // Movement speed of the player
    public Vector2 direction = Vector2.right; // Initial direction
    public float rotationCooldown = 0.5f; // Cooldown between rotations (in seconds)
    private float lastRotationTime;

    public LayerMask virusLayer; // Layer for viruses
    private int totalViruses;

    public Transform currentSpawnPoint;
    bool canMove = false;
    private Rigidbody2D rb; // Reference to Rigidbody2D for movement
    public TrailRenderer trial;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Count all virus objects in the scene
        totalViruses = GameObject.FindGameObjectsWithTag("Virus").Length;

        if (totalViruses == 0)
        {
            Debug.LogWarning("No viruses found! Add objects tagged as 'Virus' to the scene.");
        }
    }

    void Update()
    {
        // Handle direction change (only after cooldown period)
        if (Time.time - lastRotationTime >= rotationCooldown)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) 
            {
                direction = Vector2.up;
                lastRotationTime = Time.time; // Reset cooldown timer
                canMove = true;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                direction = Vector2.down;
                lastRotationTime = Time.time; // Reset cooldown timer
                canMove = true;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                direction = Vector2.left;
                lastRotationTime = Time.time; // Reset cooldown timer
                canMove = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                direction = Vector2.right;
                lastRotationTime = Time.time; // Reset cooldown timer
                canMove = true;
            }
        }

        // Apply movement to the Rigidbody2D to make it physics-based (instant movement)
        MovePlayer();
    }

    private void MovePlayer()
    {
        if(canMove)
        {
            rb.velocity = direction * speed;
            trial.gameObject.SetActive(true);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collides with a virus
        if (collision.CompareTag("Virus"))
        {
            CastDestory caster = collision.GetComponent<CastDestory>();
            if(caster != null)
            {
                caster.CallThis();
            }
            Destroy(collision.gameObject);
            totalViruses--;

            if (totalViruses <= 0)
            {
                Debug.Log("You win! All viruses destroyed.");
                SceneHandler scenen = FindObjectOfType<SceneHandler>();
                scenen.LoadSceneNamed("win");
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            Debug.Log("You hit a wall! Game Over.");
            RestartGame();
        }
    }

    private void RestartGame()
    {
        AudioManager.Instance.PlaySFX("DiePlayer");
        transform.position = currentSpawnPoint.position;
        canMove = false;
        trial.Clear();
        trial.gameObject.SetActive(false);
        rb.velocity = Vector2.zero;
    }
}
