using UnityEngine;


// If Saad touches it, he's sent back to the start of the room he's in.

[RequireComponent(typeof(Collider2D))]
public class MazeHazard : MonoBehaviour
{
    void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        bool isPlayer = other.CompareTag("Player") ||
                        (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"));
        if (isPlayer && MazeManager.Instance != null)
            MazeManager.Instance.SendBackToRoomStart("Not yet.");
    }
}
