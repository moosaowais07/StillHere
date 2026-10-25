using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour

{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 5f;
    public float jumpForce = 8f;
    public Transform groundCheck;
    public LayerMask groundsLayer;
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
    }

    // Update is called once per frame
    void Update()
    {
        weight = Mathf.Clamp01(weight + weightGainPerSec * Time.deltaTime);
        float speedMult = Mathf.Lerp(1f, minSpeedMultiplier, weight);
        float jumpMult = Mathf.Lerp(1f, minJumpMultiplier, weight);

        spriterend.color = Color.Lerp(Color.white, new Color(0.25f, 0.25f, 0.3f), weight);

        float move = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            move = -1f;
        }
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            move = 1f;
        }

        rigbody.linearVelocity = new Vector2(move * speed * speedMult, rigbody.linearVelocity.y);

        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundsLayer);
        if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rigbody.linearVelocity = new Vector2(rigbody.linearVelocity.x, jumpForce * jumpMult);
        }
    }
    public void Lighten(float amount)
    {
        weight = Mathf.Clamp01(weight - amount);
    }
}
