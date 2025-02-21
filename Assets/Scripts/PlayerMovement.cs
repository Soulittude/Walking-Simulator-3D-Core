using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float walkMultiplier = 0.5f;
    [SerializeField] private float crouchMultiplier = 0.5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float airControl = 0.5f;

    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float heightSmoothTime = 0.1f;

    private CharacterController controller;
    private Vector3 velocity;
    private float currentHeight;
    private float heightVelocity;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction crouchAction;
    private InputAction walkAction;

    private bool isSprinting;
    private bool isCrouching;
    private bool isWalking;
    private bool isGrounded;

    public bool IsGrounded { get; private set; }
    public bool IsMoving { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        var playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];
        crouchAction = playerInput.actions["Crouch"];
        walkAction = playerInput.actions["Walk"];
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        HandleCrouch();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        IsGrounded = controller.isGrounded;
        Vector2 input = moveAction.ReadValue<Vector2>();
        IsMoving = input.magnitude > 0.1f;

        float currentSpeed = GetCurrentSpeed();
        Vector3 moveDirection = (transform.right * input.x + transform.forward * input.y).normalized;
        float speedMultiplier = IsGrounded ? 1f : airControl;

        controller.Move(moveDirection * currentSpeed * speedMultiplier * Time.deltaTime);
    }

    private float GetCurrentSpeed()
    {
        float speed = baseSpeed;

        // Priority: Crouch > Sprint > Walk
        if (isCrouching) speed *= crouchMultiplier;
        else if (isSprinting) speed *= sprintMultiplier;
        else if (isWalking) speed *= walkMultiplier;

        return speed;
    }

    private void HandleJump()
    {
        if (jumpAction.triggered && IsGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void HandleCrouch()
    {
        isCrouching = crouchAction.IsPressed();
        isSprinting = sprintAction.IsPressed() && !isCrouching;
        isWalking = walkAction.IsPressed() && !isSprinting && !isCrouching;

        float targetHeight = isCrouching ? crouchHeight : standHeight;
        currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref heightVelocity, heightSmoothTime);
        controller.height = currentHeight;
    }

    private void ApplyGravity()
    {
        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}