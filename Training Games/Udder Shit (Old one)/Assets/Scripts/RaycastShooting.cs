using UnityEngine;

public class RaycastShooting : MonoBehaviour
{
    public Transform firePoint; // Point where the raycast starts
    public LineRenderer lineRenderer; // LineRenderer component to visualize the ray
    public float rayLength = 10f;

    public GameObject BulletPrefab;  // The bullet prefab to spawn
    public float bulletSpeed = 10f;  // Speed of the bullet

    public Inventory inventory;  // Reference to the inventory system

    public float fireRate = 0.5f; // Delay between shots (in seconds)
    private float nextFireTime = 0f; // Time at which the next shot is allowed

    public float shakeDuration;
    public float shakeMagnitude;
    public CameraShake camShake;
    public AudioSource source;

    void Update()
    {
        Shoot();

        if(inventory.Bullets > 0)
        {
            ShootRaycast();
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    public void Shoot()
    {
        // Only allow shooting if the cooldown has passed
        if (Time.time >= nextFireTime)
        {
            if (Input.GetMouseButton(0))  // Left mouse button clicked
            {
                if (inventory.Bullets > 0)  // Check if there are bullets available
                {
                    // Instantiate a bullet and shoot it
                    GameObject bullet = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    camShake.TriggerShake(shakeDuration, shakeMagnitude);
                    if (rb != null)
                    {
                        rb.velocity = firePoint.up * bulletSpeed;  // Shoot the bullet in the firePoint's up direction
                    }

                    // Decrease the bullet count in the inventory
                    inventory.DecreaseBullets(1);

                    // Set the time for the next allowed shot
                    nextFireTime = Time.time + fireRate;
                    source.Play();
                    // You can optionally add more behavior here like playing shooting sound, etc.
                }
                else
                {
                    Debug.Log("Not enough bullets!");
                }
            }
        }
    }

    // This method is to visualize the raycast (if you still want to show it for debugging purposes)
    void ShootRaycast()
    {
        Vector2 firePointPosition = firePoint.position;
        Vector2 direction = firePoint.up;

        // Get the layer index for NPC and Player
        int npcLayer = LayerMask.NameToLayer("NPC");
        int playerLayer = LayerMask.NameToLayer("Player");

        // Create a mask that excludes both the NPC and Player layers
        LayerMask layerMask = ~(1 << npcLayer | 1 << playerLayer);

        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, direction, rayLength, layerMask);

        if (hit)
        {
            DrawLine(firePointPosition, hit.point);
        }
        else
        {
            DrawLine(firePointPosition, firePointPosition + direction * rayLength);
        }
    }


    void DrawLine(Vector2 start, Vector2 end)
    {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }
}
