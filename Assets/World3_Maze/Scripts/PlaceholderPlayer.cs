using System.Collections;
using UnityEngine;

// A simple stand in for Saad, so World 3 can be tested before Hamza's
// player prefab is ready. Swap it for his prefab later.
//
// Needs on the same object: Rigidbody2D, BoxCollider2D, SpriteRenderer,
// and the tag "Player".
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlaceholderPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 12f;

    [Header("Attack (press J)")]
    public float attackRange = 1.2f;

    Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer sprite;
    Color normalColor;
    int facing = 1;
    bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
        if (sprite != null) normalColor = sprite.color;
        rb.freezeRotation = true;
    }

    void Update()
    {
        float h = MazeInput.Horizontal();
        if (h > 0) facing = 1;
        else if (h < 0) facing = -1;

        grounded = IsGrounded();

        Vector2 v = Velocity;
        v.x = h * moveSpeed;
        if (MazeInput.JumpPressed() && grounded) v.y = jumpForce;
        Velocity = v;

        if (MazeInput.AttackPressed()) Attack();
    }

    // Checks for any solid collider just under the player's feet.
    bool IsGrounded()
    {
        Bounds b = col.bounds;
        Vector2 feet = new Vector2(b.center.x, b.min.y - 0.05f);
        Vector2 size = new Vector2(b.size.x * 0.9f, 0.1f);
        Collider2D[] hits = Physics2D.OverlapBoxAll(feet, size, 0f);
        foreach (Collider2D hit in hits)
        {
            if (hit != col && !hit.isTrigger) return true;
        }
        return false;
    }

    void Attack()
    {
        Vector2 center = (Vector2)transform.position + new Vector2(facing * attackRange * 0.6f, 0f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, attackRange * 0.6f);
        foreach (Collider2D hit in hits)
        {
            IHittable target = hit.GetComponentInParent<IHittable>();
            if (target != null) target.Hit();
        }
        if (sprite != null) StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        sprite.color = normalColor;
    }

    Vector2 Velocity
    {
#if UNITY_6000_0_OR_NEWER
        get { return rb.linearVelocity; }
        set { rb.linearVelocity = value; }
#else
        get { return rb.velocity; }
        set { rb.velocity = value; }
#endif
    }

    // Draws the attack area in the Scene view so you can see it.
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position + new Vector3(facing * attackRange * 0.6f, 0f, 0f);
        Gizmos.DrawWireSphere(center, attackRange * 0.6f);
    }
}
