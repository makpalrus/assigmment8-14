using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public PlayerStats stats;
    public Transform groundCheck;
    public LayerMask groundLayer;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sprite;
    bool isGrounded;
    bool jumpQueued;
    float horizontal;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        horizontal = move.x;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && Time.timeScale > 0 && (!GameUI.Instance || GameUI.Instance.IsPlaying))
            jumpQueued = true;
    }

    public void OnPause(InputValue value)
    {
        GameUI ui = GameUI.Instance;
        if (!value.isPressed || ui.gameOver.activeSelf) return;
        if (ui.IsPlaying) ui.Pause();
        else ui.Resume();
    }
    void OnDisable()
    {
        horizontal = 0;
        ClearInput();
    }

    public void ClearInput() => jumpQueued = false;

    void Update()
    {
        if (Time.timeScale == 0 || (GameUI.Instance && !GameUI.Instance.IsPlaying)) return;
        if (sprite && Mathf.Abs(horizontal) > 0.01f)
            sprite.flipX = horizontal < 0;

        if (!animator) return;
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
        animator.SetBool("Grounded", isGrounded);
        animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);
    }
    void FixedUpdate()
    {
        Collider2D ground = null;
        if (groundCheck)
            ground = Physics2D.OverlapCircle(groundCheck.position, stats.groundCheckRadius, groundLayer);

        bool wasGrounded = isGrounded;
        isGrounded = ground;
        if (!wasGrounded && isGrounded && rb.linearVelocity.y < -1f)
            GameAudio.Instance.PlayLand();
        float platformSpeed = 0f;
        if (ground && ground.TryGetComponent(out MovingPlatform platform))
            platformSpeed = platform.HorizontalVelocity;

        rb.linearVelocity = new Vector2(horizontal * stats.moveSpeed + platformSpeed, rb.linearVelocity.y);
        if (jumpQueued && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpForce);
            isGrounded = false;
            GameAudio.Instance.PlayJump();
        }
        jumpQueued = false;
    }
}
