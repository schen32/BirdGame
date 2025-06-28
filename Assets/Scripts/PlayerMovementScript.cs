using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovementScript : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;

    public float gravityScale;
    public float fallingGravityScale;

    Vector2 boostLeftForce;
    Vector2 boostRightForce;
    Vector2 boostUpForce;

    public ParticleSystem jumpParticles;

    float jumpBufferTime = 0.2f;
    float jumpBufferCounter = 0f;

    float coyoteTime = 0.1f; // Optional: allows jump shortly after falling
    float coyoteTimeCounter = 0f;
    bool isGrounded = false;
    bool isTouchingLeftWall = false;
    bool isTouchingRightWall = false;

    bool usedLeftBoost = false;
    bool usedUpBoost = false;
    bool usedRightBoost = false;

    Vector2 moveInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;
    AudioSource audioSource;

    public AudioClip boostSound;
    public AudioClip landSound;
    
    void Awake()
    {
        moveSpeed = 10f;
        jumpForce = 20f;

        gravityScale = 5f;
        fallingGravityScale = 10f;

        boostLeftForce = new Vector2(-1.5f, 1.5f);
        boostRightForce = new Vector2(1.5f, 1.5f);
        boostUpForce = new Vector2(0, 2);

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;

        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
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
            sr.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            rb.linearVelocityX = -moveSpeed;
            sr.flipX = true;
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
        if (isTouchingLeftWall)
        {
            rb.AddForce(jumpForce * boostRightForce, ForceMode2D.Impulse);
        }
        else if (isTouchingRightWall)
        {
            rb.AddForce(jumpForce * boostLeftForce, ForceMode2D.Impulse);
        }
        jumpBufferCounter = jumpBufferTime;
    }

    public void OnBoostLeft()
    {
        if (usedLeftBoost) return;

        rb.AddForce(jumpForce * boostLeftForce, ForceMode2D.Impulse);
        usedLeftBoost = true;

        playParticles(45);
        playAudio(boostSound, 0.2f);
    }

    public void OnBoostRight()
    {
        if (usedRightBoost) return;

        rb.AddForce(jumpForce * boostRightForce, ForceMode2D.Impulse);
        usedRightBoost = true;

        playParticles(135);
        playAudio(boostSound, 0.2f);
    }
    public void OnBoostUp()
    {
        if (usedUpBoost) return;

        rb.AddForce(jumpForce * boostUpForce, ForceMode2D.Impulse);
        usedUpBoost = true;

        playParticles(90);
        playAudio(boostSound, 0.2f);
    }

    void playParticles(int angle, float lifetime = 0.4f, int emitCount = 4)
    {
        var emission = jumpParticles.emission;
        emission.SetBurst(0, new ParticleSystem.Burst(0f, emitCount));

        var main = jumpParticles.main;
        main.startLifetime = lifetime;

        jumpParticles.transform.position = rb.position - new Vector2(0, sr.size.y);
        var shape = jumpParticles.shape;
        shape.rotation = new Vector3(0, angle, 0);
        jumpParticles.Play();
    }

    void playAudio(AudioClip sound, float volume)
    {
        audioSource.clip = sound;
        audioSource.volume = volume;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.x > 0.5f)
            {
                isTouchingLeftWall = true;
            }
            else if (contact.normal.x < -0.5f)
            {
                isTouchingRightWall = true;
            }

            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                coyoteTimeCounter = coyoteTime;

                usedLeftBoost = false;
                usedUpBoost = false;
                usedRightBoost = false;

                playParticles(90, 0.2f, 2);
                playAudio(landSound, 0.1f);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {    
        isGrounded = false;
        isTouchingLeftWall = false;
        isTouchingRightWall = false;
    }
}
