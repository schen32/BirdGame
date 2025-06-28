using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    public float gravityScale;
    public float fallingGravityScale;

    public Vector2 jumpLeftForce;
    public Vector2 jumpRightForce;
    public Vector2 jumpUpForce;

    float jumpBufferTime = 0.2f;
    float jumpBufferCounter = 0f;

    float coyoteTime = 0.1f; // Optional: allows jump shortly after falling
    float coyoteTimeCounter = 0f;
    bool isGrounded = false;

    bool usedLeftJump = false;
    bool usedUpJump = false;
    bool usedRightJump = false;

    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public ParticleSystem jumpParticles;

    void Start()
    {
        moveSpeed = 10f;
        jumpForce = 20f;

        gravityScale = 5f;
        fallingGravityScale = 10f;

        jumpLeftForce = new Vector2(-1.5f, 1.5f);
        jumpRightForce = new Vector2(1.5f, 1.5f);
        jumpUpForce = new Vector2(0, 2);

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Update timers
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.fixedDeltaTime;
        }
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        // Horizontal movement
        if (moveInput.x > 0)
        {
            rb.linearVelocityX = moveSpeed;
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            rb.linearVelocityX = -moveSpeed;
            spriteRenderer.flipX = true;
        }

        // Jump (only once per press)
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.linearVelocityY = jumpForce;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
        }
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", Mathf.Abs(rb.linearVelocityY));

        // Gravity adjustment
        if (rb.linearVelocityY >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = fallingGravityScale;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        // Only trigger on key down, not hold
        jumpBufferCounter = jumpBufferTime;
    }
    public void OnJumpLeft()
    {
        if (usedLeftJump) return;

        rb.AddForce(jumpForce * jumpLeftForce, ForceMode2D.Impulse);
        usedLeftJump = true;

        playParticles(45);
    }

    public void OnJumpRight()
    {
        if (usedRightJump) return;

        rb.AddForce(jumpForce * jumpRightForce, ForceMode2D.Impulse);
        usedRightJump = true;

        playParticles(135);
    }
    public void OnJumpUp()
    {
        if (usedUpJump) return;

        rb.AddForce(jumpForce * jumpUpForce, ForceMode2D.Impulse);
        usedUpJump = true;

        playParticles(90);
    }

    void playParticles(int angle)
    {
        jumpParticles.transform.position = transform.position;
        var shape = jumpParticles.shape;
        shape.rotation = new Vector3(0, angle, 0);
        jumpParticles.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                coyoteTimeCounter = coyoteTime;

                usedLeftJump = false;
                usedUpJump = false;
                usedRightJump = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
