
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Rigidbody2D rb;
    private Vector2 target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = pointB.position;
    }

    void FixedUpdate()
    {
        if (pointA == null || pointB == null)
            return;

        Vector2 newPosition = Vector2.MoveTowards(rb.position,target,speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);

        if (Vector2.Distance(newPosition, target) < 0.05f)
        {
            if (target == (Vector2)pointB.position)
                target = pointA.position;
            else
                target = pointB.position;
        }
    }
}
