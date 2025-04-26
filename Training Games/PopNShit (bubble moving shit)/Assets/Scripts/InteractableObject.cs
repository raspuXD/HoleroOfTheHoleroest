using System.Collections;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
   public Rigidbody rb;
    public string nameForThis = "A Box";
    public float movemeventSpeed;
    [SerializeField] private Vector3 positionOffset = new Vector3(0, 0.5f, 0); // Optional: Offset the object's position
   AudioSource source;
    bool canPlay;
    private void Awake()
    {
        // Cache the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2f);

        if (source == null)
        {
            source = GetComponent<AudioSource>();
        }

        canPlay = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(canPlay)
        {
            if (source == null)
            {
                source = GetComponent<AudioSource>();
            }

            source.Play();
        }
    }

    public void MoveToTarget(Transform target)
    {
        if (target != null)
        {
            // Apply the position offset to prevent the object from going inside the target position
            Vector3 targetPositionWithOffset = target.position + positionOffset;

            // Move the object to the adjusted position and make it a child
            transform.position = targetPositionWithOffset;
            transform.SetParent(target);

            // Set the Rigidbody to be kinematic
            if (rb != null)
            {
                rb.useGravity = false;
                Physics.IgnoreLayerCollision(rb.gameObject.layer, LayerMask.NameToLayer("Player"), true);
            }

            Debug.Log($"{gameObject.name} moved to target position and is now a child of {target.name}, Rigidbody is kinematic.");
        }
        else
        {
            Debug.LogWarning("Target position is not assigned.");
        }
        
    }

    public void ReleaseObject()
    {
        // Remove the object from being a child of the target
        transform.SetParent(null);

        // Set the Rigidbody to be non-kinematic again (so it will be affected by physics)
        if (rb != null)
        {
            rb.useGravity = true;
             Physics.IgnoreLayerCollision(rb.gameObject.layer, LayerMask.NameToLayer("Player"), false);
        }

        Debug.Log($"{gameObject.name} is released and Rigidbody is non-kinematic.");
    }
}
