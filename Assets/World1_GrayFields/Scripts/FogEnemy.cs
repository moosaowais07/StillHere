
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FogEnemy : MonoBehaviour
{
    public int health = 2;
    public float damageCooldown = 1f;

    public float moveSpeed = 1.5f;
    public float patrolDistance = 2f;

    private float nextDamageTime = 0f;
    private Rigidbody2D body;
    private Vector2 startPosition;
    private bool movingRight = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        startPosition = body.position;
    }

    private void FixedUpdate()
    {
        float targetX = movingRight ? startPosition.x + patrolDistance: startPosition.x - patrolDistance;
        Vector2 target = new Vector2(targetX, startPosition.y);

        Vector2 nextPosition = Vector2.MoveTowards(body.position, target, moveSpeed * Time.fixedDeltaTime);
        body.MovePosition(nextPosition);

        // Turn around when the enemy reaches either end.
        if (nextPosition == target)
        {
            movingRight = !movingRight;
        }
    }

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