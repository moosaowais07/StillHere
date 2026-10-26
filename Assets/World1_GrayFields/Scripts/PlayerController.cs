
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f;
    public float attackRange = 1.5f;
    public float minimumMoveSpeed = 2f;
    public float minimumJumpForce = 1f;
    public float jumpHoldForce = 5f;
    public float maxJumpHoldTime = 0.3f;
    private float jumpHoldTimer;
    private bool isJumping;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2( move * moveSpeed, rb.linearVelocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x,jumpForce);
            isGrounded = false;
            isJumping = true;
            jumpHoldTimer = 0f;
        }

        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpHoldTimer < maxJumpHoldTime){
                rb.linearVelocity += Vector2.up*jumpHoldForce*Time.deltaTime;
                jumpHoldTimer += Time.deltaTime;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)){
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.J)||Input.GetKeyDown(KeyCode.X)){
            Attack();
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

        foreach (Collider2D enemy in enemies)
        {
            FogEnemy fog = enemy.GetComponent<FogEnemy>();

            if (fog!=null)
                fog.TakeDamage();
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,attackRange);

        foreach (Collider2D hit in hits)
        {
            RainBoss boss = hit.GetComponent<RainBoss>();
            if (boss != null){
                boss.TakeDamage();
            }
        }

    }
}
