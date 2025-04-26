using UnityEngine;

public class LaunchPlayerOnTrigger : MonoBehaviour
{
    public float launchForce = 10f; // Adjust this for the strength of the launch
    public float directionInfluence = 0.5f; // 0 for only upward, 1 for fully influenced by velocity

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                Vector3 oppositeDirection = -playerRigidbody.velocity.normalized;
                Vector3 blendedDirection = Vector3.Lerp(Vector3.up, oppositeDirection + Vector3.up, directionInfluence);
                blendedDirection.Normalize();
                playerRigidbody.AddForce(blendedDirection * launchForce, ForceMode.Impulse);
            }
        }

        if (other.CompareTag("canPickUp"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null && !playerRigidbody.isKinematic)
            {
                Vector3 oppositeDirection = -playerRigidbody.velocity.normalized;
                Vector3 blendedDirection = Vector3.Lerp(Vector3.up, oppositeDirection + Vector3.up, directionInfluence);
                blendedDirection.Normalize();
                playerRigidbody.AddForce(blendedDirection * launchForce * 100f, ForceMode.Impulse);
            }
        }
    }
}
