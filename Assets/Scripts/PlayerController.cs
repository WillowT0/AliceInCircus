using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//na przysz³oœæ - dodaæ unityengine.inputsytem albo nic ne bêdzie dzia³aæ i zmieniæ preferencje z default na visual studio 

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpImpulse = 12f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    private Vector2 moveInput;
    private bool isJumpHeld;

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
            {
                //w prawo
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
            _isFacingRight = value;
        }
    }

    public float CurrentMoveSpeed
    {
        get
        {
            if (touchingDirections.IsGrounded)
            {
                return IsMoving ? (IsRunning ? runSpeed : walkSpeed) : 0f;
            }
            else
            {
                return airWalkSpeed;
            }
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.linearVelocity;

        // Gravity adjustment for better jump feel
        if (velocity.y < 0f)
        {
            // Falling
            velocity.y += Physics2D.gravity.y * (fallMultiplier - 1f) * Time.fixedDeltaTime;
        }
        else if (velocity.y > 0f && !isJumpHeld)
        {
            // Released jump early
            velocity.y += Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime;
        }

        // Apply horizontal movement
        velocity.x = moveInput.x * CurrentMoveSpeed;
        rb.linearVelocity = velocity;

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded)
        {
            animator.SetTrigger("jump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
            isJumpHeld = true;
        }
        else if (context.canceled)
        {
            isJumpHeld = false;
        }
    }
}
