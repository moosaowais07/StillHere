
using UnityEngine;

public class FogEnemy : MonoBehaviour
{
    public int health = 2;

    public float damageCooldown = 1f;

    private float nextDamageTime = 0f;

    public void TakeDamage()
    {
        health = health - 1;

        Debug.Log("Fog enemy hit! Health left: " + health);

        if (health <= 0)
        {
            Debug.Log("Fog enemy defeated!");
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time < nextDamageTime)
        {
            return;
        }

        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage();
            nextDamageTime = Time.time + damageCooldown;
        }
    }

}
