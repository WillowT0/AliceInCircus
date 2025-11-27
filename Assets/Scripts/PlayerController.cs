using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    [Header("Audio Settings")]
    public PlayerSounds playerSounds;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 7f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpImpulse = 14f;
    [SerializeField] private float gravityScale = 3f;
    [SerializeField] private float fallGravityMult = 2.5f;
    [SerializeField] private float jumpCutGravityMult = 3f;
    [SerializeField] private float jumpHangGravityMult = 0.5f;
    [SerializeField] private float jumpHangTimeThreshold = 1f;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float maxFastFallSpeed = 25f;
    [SerializeField] private float fastFallGravityMult = 3.5f;
    [SerializeField] private float coyoteTime = 0.1f;

    private Vector2 moveInput;
    private bool isJumpHeld;
    private bool isJumpCut;
    private float coyoteTimeCounter;

    private Rigidbody2D rb;
    private Animator animator;
    private TouchingDirections touchingDirections;

    [Header("State Flags")]
    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isRunning = false;
    [SerializeField] private bool _isFacingRight = true;

  


    public bool IsMoving
    {
        get => _isMoving;
        private set
        {
            _isMoving = value;
            animator.SetBool("isMoving", value);
        }
    }

    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            _isRunning = value;
            animator.SetBool("isRunning", value);
        }
    }

    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            _isFacingRight = value;
        }
    }

    public float CurrentMoveSpeed =>
        touchingDirections.IsGrounded ? (IsRunning ? runSpeed : walkSpeed) : airWalkSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
            GetComponent<PlayerInput>().enabled = false;
        else
            GetComponent<PlayerInput>().enabled = true;

        if (touchingDirections.IsGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (playerSounds != null)
        {
            playerSounds.isSprinting = IsRunning;
            playerSounds.isMoving = IsMoving;
            playerSounds.isGrounded = touchingDirections.IsGrounded;
        }
    }

    private void FixedUpdate()
    {
        HandleGravity();
        HandleMovement();
    }

    private void HandleMovement()
    {
        float targetX = moveInput.x * CurrentMoveSpeed;
        float newX = Mathf.Lerp(rb.linearVelocity.x, targetX, 0.2f);
        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }


    #region GRAVITY
    private void HandleGravity()
    {
        float gravityMultiplier = 1f;

        if (rb.linearVelocity.y < 0 && moveInput.y < 0)
        {
            gravityMultiplier = fastFallGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFastFallSpeed));
        }
        else if (isJumpCut)
        {
            gravityMultiplier = jumpCutGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else if (rb.linearVelocity.y > 0 && Mathf.Abs(rb.linearVelocity.y) < jumpHangTimeThreshold)
        {
            gravityMultiplier = jumpHangGravityMult;
        }
        else if (rb.linearVelocity.y < 0)
        {
            gravityMultiplier = fallGravityMult;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            gravityMultiplier = 1f;
        }

        SetGravityScale(gravityScale * gravityMultiplier);
    }

    private void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }
    #endregion

    #region INPUT
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight) IsFacingRight = true;
        else if (moveInput.x < 0 && IsFacingRight) IsFacingRight = false;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started) IsRunning = true;
        else if (context.canceled) IsRunning = false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && coyoteTimeCounter > 0f)
        {
            animator.SetTrigger("jump");
            
            if (playerSounds != null) playerSounds.PlayJump();

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
            isJumpHeld = true;
            isJumpCut = false;
            coyoteTimeCounter = 0f;
        }
        else if (context.canceled)
        {
            isJumpHeld = false;
            isJumpCut = rb.linearVelocity.y > 0;
        }
    }

    public void OnJumpBounce()
    {
        if (playerSounds != null) playerSounds.PlayJump();

        isJumpHeld = true;
        isJumpCut = false;
        coyoteTimeCounter = 0f;
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
            Debug.Log("Interact button pressed.");
    }
    #endregion
}