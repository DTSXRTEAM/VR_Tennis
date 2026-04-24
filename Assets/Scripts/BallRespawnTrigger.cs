using UnityEngine;

public class BallRespawnTrigger : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // Reset velocity
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Move ball to spawn position
            other.transform.position = respawnPoint.position;
        }
    }
}