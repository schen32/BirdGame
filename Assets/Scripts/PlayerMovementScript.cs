using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 30f;
    public float gravityScale = 10f;
    public float fallingGravityScale = 12f;

    float jumpBufferTime = 0.2f;
    float jumpBufferCounter = 0f;

    float coyoteTime = 0.1f; // Optional: allows jump shortly after falling
    float coyoteTimeCounter = 0f;
    bool isGrounded = false;

    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
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
        float xVelocity = 0;
        if (moveInput.x > 0)
        {
            xVelocity = moveSpeed;
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            xVelocity = -moveSpeed;
            spriteRenderer.flipX = true;
        }
        rb.linearVelocityX = xVelocity;

        // Jump (only once per press)
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.linearVelocityY = jumpForce;
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
            isGrounded = false;
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
        if (value.isPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                isGrounded = true;
                coyoteTimeCounter = coyoteTime;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

}
