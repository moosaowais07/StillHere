
using UnityEngine;

public class PlayerMemory : MonoBehaviour
{
    public int memoriesCollected = 0;
    public float speedIncreasePerMemory = 0.3f;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public void CollectMemory()
    {
        memoriesCollected++;

        if (playerController != null)
        {
            playerController.moveSpeed += speedIncreasePerMemory;
            Debug.Log("Saad's speed: " + playerController.moveSpeed);
        }

        Debug.Log("Memories collected: " + memoriesCollected);
    }
}
