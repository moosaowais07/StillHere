
using UnityEngine;

public class RainBoss : MonoBehaviour
{
    public int health = 5;

    public GameObject rainPrefab;
    public Transform player;

    public float moveSpeed = 2f;
    public float horizontalDistance = 3f;
    public float verticalDistance = 1.5f;

    public float rainInterval = 1.5f;
    public float activationDistance = 8f;

    private Vector3 startPosition;
    private bool isAttacking = false;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (health <= 0)
        {
            return;
        }

        // Move the cloud left, right, up and down
        float x = startPosition.x +
            Mathf.Sin(Time.time * moveSpeed) * horizontalDistance;

        float y = startPosition.y +
            Mathf.Sin(Time.time * moveSpeed * 1.5f) * verticalDistance;

        transform.position = new Vector3(x, y, startPosition.z);

        // Start attacking when Saad gets close
        if (player != null)
        {
            float distance = Vector2.Distance(
                transform.position,
                player.position
            );

            if (distance <= activationDistance && !isAttacking)
            {
                isAttacking = true;
                InvokeRepeating("DropRain", 0f, rainInterval);
            }
            else if (distance > activationDistance && isAttacking)
            {
                isAttacking = false;
                CancelInvoke("DropRain");
            }
        }
    }

    void DropRain()
    {
        if (rainPrefab != null)
        {
            // Random horizontal position underneath the cloud
            float randomX = Random.Range(-1.5f, 1.5f);

            Vector3 rainPosition = new Vector3(
                transform.position.x + randomX,
                transform.position.y - 0.5f,
                0f
            );

            Instantiate(
                rainPrefab,
                rainPosition,
                Quaternion.identity
            );
        }
    }

    public void TakeDamage()
    {
        health--;

        Debug.Log("RainBoss health: " + health);

        if (health <= 0)
        {
            CancelInvoke("DropRain");
            Destroy(gameObject);
            Debug.Log("World 1 Complete");
        }
    }
}
