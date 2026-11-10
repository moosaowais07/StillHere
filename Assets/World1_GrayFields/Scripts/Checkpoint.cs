
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        CheckpointManager manager =
            FindFirstObjectByType<CheckpointManager>();

        if (manager != null)
        {
            manager.SetCheckpoint(transform.position);
            activated = true;
            Debug.Log("Checkpoint activated!");
        }
    }
}
