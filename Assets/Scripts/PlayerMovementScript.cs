using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovementScript : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float boostForce;
    public float wallJumpForce;

    public float gravityScale;
    public float fallingGravityScale;

    Vector2 boostLeftDirection = new Vector2(-1.5f, 2f);
    Vector2 boostRightDirection = new Vector2(1.5f, 2f);
    Vector2 boostUpDirection = new Vector2(0, 2.5f);

    public ParticleSystem jumpParticles;
    public ParticleSystem runParticles;
    public AudioClip boostSound;
    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip runSound;

    float jumpBufferTime = 0.2f;
    float jumpBufferCounter = 0f;
    float coyoteTime = 0.1f; // Optional: allows jump shortly after falling
    float coyoteTimeCounter = 0f;

    public float runSoundInterval = 0.4f;
    float runSoundTimer = 0f;

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

    public AudioSource audioSource;
    
    void Awake()
    {
        moveSpeed = 10f;
        jumpForce = 20f;
        boostForce = 20f;
        wallJumpForce = 16f;

        gravityScale = 5f;
        fallingGravityScale = 10f;

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;

        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // Update timers
        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.fixedDeltaTime;
        }
        if (isGrounded || isTouchingLeftWall || isTouchingRightWall)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        // Horizontal movement
        if (moveInput.x != 0)
        {
            if (moveInput.x > 0)
            {
                rb.linearVelocityX = moveSpeed;
                if (isGrounded) playRunParticles(180);
            }
            else if (moveInput.x < 0)
            {
                rb.linearVelocityX = -moveSpeed;
                if (isGrounded) playRunParticles(0);
            }
            if (isGrounded)
            {
                runSoundTimer -= Time.deltaTime;
                if (runSoundTimer <= 0)
                {
                    playAudio(runSound, 0.1f);
                    runSoundTimer = runSoundInterval;
                }
            }
        }
        else
        {
            runSoundTimer = 0f;
        }


        // Jump (only once per press)
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            if (isGrounded)
            {
                rb.linearVelocityY = jumpForce;
                playJumpParticles(90, 0.2f, 2);
            }
            else if (isTouchingLeftWall)
            {
                rb.AddForce(wallJumpForce * boostRightDirection, ForceMode2D.Impulse);
                playJumpParticles(120, 0.2f, 2);

                moveInput.x = 0;
            }
            else if (isTouchingRightWall)
            {
                rb.AddForce(wallJumpForce * boostLeftDirection, ForceMode2D.Impulse);
                playJumpParticles(60, 0.2f, 2);

                moveInput.x = 0;
            }
            else
            {
                rb.linearVelocityY = jumpForce;
                playJumpParticles(90, 0.2f, 2);
            }
            playAudio(jumpSound, 0.30f);

            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;
        }
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocityX));
        animator.SetFloat("yVelocity", Mathf.Abs(rb.linearVelocityY));
        animator.SetBool("isTouchingWall", isTouchingLeftWall || isTouchingRightWall);
        animator.SetBool("isGrounded", isGrounded);

        if (rb.linearVelocityX >= 0)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
        // Gravity adjustment
        if (rb.linearVelocityY >= 0)
        {
            rb.gravityScale = gravityScale;
        }
        else
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
        jumpBufferCounter = jumpBufferTime;
    }

    public void OnBoostLeft()
    {
        if (usedLeftBoost) return;

        rb.AddForce(boostForce * boostLeftDirection, ForceMode2D.Impulse);
        usedLeftBoost = true;

        playJumpParticles(45);
        playAudio(boostSound, 0.6f);
    }

    public void OnBoostRight()
    {
        if (usedRightBoost) return;

        rb.AddForce(boostForce * boostRightDirection, ForceMode2D.Impulse);
        usedRightBoost = true;

        playJumpParticles(135);
        playAudio(boostSound, 0.6f);
    }
    public void OnBoostUp()
    {
        if (usedUpBoost) return;

        rb.AddForce(boostForce * boostUpDirection, ForceMode2D.Impulse);
        usedUpBoost = true;

        playJumpParticles(90);
        playAudio(boostSound, 0.6f);
    }

    void playJumpParticles(int angle, float lifetime = 0.4f, int emitCount = 4)
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
    void playRunParticles(int angle)
    {
        if (!runParticles.isPlaying)
        {
            runParticles.transform.position = rb.position - new Vector2(0, sr.size.y) / 2;
            var shape = runParticles.shape;
            shape.rotation = new Vector3(0, angle, 0);

            var renderer = runParticles.GetComponent<ParticleSystemRenderer>();
            if (angle < 90)
            {
                renderer.flip = new Vector2(1, 0);
            }
            else
            {
                renderer.flip = new Vector2(0, 0);
            }
            runParticles.Play();
        }
    }

    void playAudio(AudioClip sound, float volume)
    {
        audioSource.clip = sound;
        audioSource.volume = volume;
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.Play();
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        isTouchingLeftWall = false;
        isTouchingRightWall = false;
        isGrounded = false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.x > 0.5f)
                isTouchingLeftWall = true;

            if (contact.normal.x < -0.5f)
                isTouchingRightWall = true;

            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                usedLeftBoost = false;
                usedUpBoost = false;
                usedRightBoost = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                playAudio(landSound, 0.2f);
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
