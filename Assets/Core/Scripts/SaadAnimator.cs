using UnityEngine;

// Plays Saad's animations (Idle, Run, Jump, Fall) and turns him to face the way he's moving.
// It only watches the player's Rigidbody, so it works with any movement script
// (the PlaceholderPlayer now, or Hamza's player later) without changing that script.

//   Speed          (Float)  how fast he's moving left/right
//   VerticalSpeed  (Float)  going up (+) or falling (-)
//   IsGrounded     (Bool)   standing on something
[RequireComponent(typeof(Animator))]
public class SaadAnimator : MonoBehaviour
{
    [Tooltip("Leave empty to use the Rigidbody 2D on this object.")]
    public Rigidbody2D body;

    [Tooltip("Leave empty to use the Sprite Renderer on this object.")]
    public SpriteRenderer sprite;

    [Tooltip("Tick this if the drawing in your sprite sheet faces LEFT instead of right.")]
    public bool artFacesLeft = false;

    Animator animator;
    Collider2D col;

    void Awake()
    {
        animator = GetComponent<Animator>();
        if (body == null) body = GetComponent<Rigidbody2D>();
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (body == null) return;

#if UNITY_6000_0_OR_NEWER
        Vector2 v = body.linearVelocity;
#else
        Vector2 v = body.velocity;
#endif

        animator.SetFloat("Speed", Mathf.Abs(v.x));
        animator.SetFloat("VerticalSpeed", v.y);
        animator.SetBool("IsGrounded", IsGrounded());

        // Face the direction he's moving 
        if (sprite != null)
        {
            if (v.x > 0.05f) sprite.flipX = artFacesLeft;
            else if (v.x < -0.05f) sprite.flipX = !artFacesLeft;
        }
    }

    //tests if there is anything solid under feet
    bool IsGrounded()
    {
        if (col == null) return true;
        Bounds b = col.bounds;
        Vector2 feet = new Vector2(b.center.x, b.min.y - 0.05f);
        Vector2 size = new Vector2(b.size.x * 0.9f, 0.1f);
        foreach (Collider2D hit in Physics2D.OverlapBoxAll(feet, size, 0f))
        {
            if (hit != col && !hit.isTrigger) return true;
        }
        return false;
    }
}
