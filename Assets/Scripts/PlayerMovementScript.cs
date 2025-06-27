using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 30f;
    public float gravityScale = 10f;
    public float fallingGravityScale = 12f;

    Vector2 moveInput;
    Rigidbody2D rb;
    bool canJump = false;
    bool jumpPressed = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
    }

    void FixedUpdate()
    {
        // Horizontal movement
        float xVelocity = 0;
        if (moveInput.x > 0)
        {
            xVelocity = moveSpeed;
        }
        else if (moveInput.x < 0)
        {
            xVelocity = -moveSpeed;
        }
            rb.linearVelocityX = xVelocity;

        // Jump (only once per press)
        if (jumpPressed && canJump)
        {
            rb.linearVelocityY = jumpForce;
            canJump = false;
            jumpPressed = false; // consume the press
        }

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
            jumpPressed = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                canJump = true;
                break;
            }
        }
    }
}
