using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 8f;

    [Header("Ground check")]
    [Tooltip("A small empty object at the player's feet.")]
    public Transform groundCheck;
    [Tooltip("How big the circle at the feet is.")]
    public float groundCheckRadius = 0.15f;
    [Tooltip("Layers that count as ground. If this is set to Nothing, ANY solid object counts as ground.")]
    public LayerMask groundsLayer;

    [Header("Weight (World 1 mechanic: set Weight Gain Per Sec to 0 in World 2)")]
    public float weight = 0f;
    public float weightGainPerSec = 0.05f;
    public float minSpeedMultiplier = 0.4f;
    public float minJumpMultiplier = 0.6f;

    Rigidbody2D rigbody;
    SpriteRenderer spriterend;

    void Start()
    {
        rigbody = GetComponent<Rigidbody2D>();
        spriterend = GetComponent<SpriteRenderer>();
        if (groundCheck == null)
            Debug.LogError("PlayerMovement: Ground Check is empty, so the player can never jump. Drag the GroundCheck object into it.");
    }

    void Update()
    {
        weight = Mathf.Clamp01(weight + weightGainPerSec * Time.deltaTime);
        float speedMult = Mathf.Lerp(1f, minSpeedMultiplier, weight);
        float jumpMult = Mathf.Lerp(1f, minJumpMultiplier, weight);

        if (spriterend != null)
            spriterend.color = Color.Lerp(Color.white, new Color(0.25f, 0.25f, 0.3f), weight);

        Keyboard k = Keyboard.current;
        if (k == null) return;

        float move = 0f;
        if (k.aKey.isPressed || k.leftArrowKey.isPressed) move = -1f;
        if (k.dKey.isPressed || k.rightArrowKey.isPressed) move = 1f;

        rigbody.linearVelocity = new Vector2(move * speed * speedMult, rigbody.linearVelocity.y);

        if (k.spaceKey.wasPressedThisFrame && IsGrounded())
        {
            rigbody.linearVelocity = new Vector2(rigbody.linearVelocity.x, jumpForce * jumpMult);
        }
    }

    // True if the circle at the player's feet touches solid ground.
    bool IsGrounded()
    {
        if (groundCheck == null) return false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.attachedRigidbody == rigbody) continue; // ignore the player's own collider
            if (hit.isTrigger) continue;                     // ignore triggers like doors and cars

            bool layerIsGround = groundsLayer.value == 0 || ((1 << hit.gameObject.layer) & groundsLayer.value) != 0;
            if (layerIsGround) return true;
        }
        return false;
    }

    public void Lighten(float amount)
    {
        weight = Mathf.Clamp01(weight - amount);
    }

    // Shows the ground check circle in the Scene view (select the player to see it).
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}