
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 5;
    private bool isDead = false;

    public void TakeDamage()
    {
        if (isDead)
            return;

        health = Mathf.Max(health - 1, 0);
        Debug.Log("Players health: " + health);

        if (health <= 0){
            Respawn();
        }
    }

    private void Respawn()
    {
        isDead = true;

        CheckpointManager manager =FindFirstObjectByType<CheckpointManager>();

        if (manager != null)
        {
            manager.RespawnPlayer(gameObject);
        }
        else
        {
            Debug.LogWarning("CheckpointManager not found");
        }

        health = 5;
        Debug.Log("Player respawned with 5 health");

        isDead = false;
    }

    public void FallDamage()
    {
        if (isDead)
            return;

        health = Mathf.Max(health - 1, 0);
        Debug.Log("Player fell Health: " + health);
        isDead = true;
        CheckpointManager manager =FindFirstObjectByType<CheckpointManager>();

        if (manager != null)
        {
            manager.RespawnPlayer(gameObject);
        }

        else
        {
            Debug.LogWarning("CheckpointManager not found");
        }

        if (health <= 0)
        {
            health = 5;
            Debug.Log("Player respawned with 5 health");
        }

        isDead = false;
    }
}
