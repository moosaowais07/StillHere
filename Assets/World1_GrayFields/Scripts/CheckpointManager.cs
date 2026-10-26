
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 currentCheckpoint;

    private void Awake()
    {
        currentCheckpoint=GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    public void SetCheckpoint(Vector3 position)
    {
        currentCheckpoint = position;
    }

    public void RespawnPlayer(GameObject player)
    {
        player.transform.position = currentCheckpoint;
        Rigidbody2D rb =player.GetComponent<Rigidbody2D>();

        if (rb!=null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        Debug.Log("Player respawned at checkpoint!");
    }
}
