using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public Vector2 moveSpeed;
    Vector2 moveInput;
    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = new Vector2(300, 300);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float xVelocity = moveInput.x * moveSpeed.x * Time.deltaTime;
        float yVelocity = moveInput.y * moveSpeed.y * Time.deltaTime;

        rb.linearVelocityX = xVelocity;
        if (yVelocity > 0)
        {
            rb.linearVelocityY = yVelocity;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
