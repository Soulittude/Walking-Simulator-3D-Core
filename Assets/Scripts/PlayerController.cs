using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Reference to the CharacterController component
    private CharacterController controller;
    // Cache for camera rotation.t
    private float xRotation = 0f;

    [Header("Movement Settings")]
    public float baseSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float walkMultiplier = 0.5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    private Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerCamera;

    [Header("Flashlight Settings")]
    public Light flashlight;
    private bool flashlightOn = false;

    [Header("Input Actions")]
    // These InputActions should be assigned via the inspector (from your Input Action asset)
    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction lookAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction sprintAction;
    [SerializeField] private InputAction walkAction;
    [SerializeField] private InputAction crouchAction;
    [SerializeField] private InputAction interactAction;
    [SerializeField] private InputAction flashlightAction;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Enable input actions
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
        walkAction.Enable();
        crouchAction.Enable();
        interactAction.Enable();
        flashlightAction.Enable();

        // Bind flashlight toggle
        flashlightAction.performed += ctx => ToggleFlashlight();
        // Bind interact action
        interactAction.performed += ctx => Interact();
    }

    void Update()
    {
        HandleMovement();
        HandleLook();
        HandleJump();
        HandleCrouch();   // You can expand this as needed.
        // Other mechanics (sprint, walk modifiers) are handled in movement.
    }

    void HandleMovement()
    {
        // Read movement input (WASD)
        Vector2 input = moveAction.ReadValue<Vector2>();

        // Determine movement multiplier based on sprint/walk keys
        float multiplier = 1f;
        if (sprintAction.IsPressed())
            multiplier = sprintMultiplier;
        else if (walkAction.IsPressed())
            multiplier = walkMultiplier;

        // Calculate movement direction in local space
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        controller.Move(move * baseSpeed * multiplier * Time.deltaTime);

        // Ground check for gravity and jumping
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        // Read mouse movement input
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // Adjust vertical rotation and clamp it
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate camera (vertical)
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        // Rotate player (horizontal)
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleJump()
    {
        // Check for jump input (Spacebar) and if grounded
        if (jumpAction.triggered && Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void HandleCrouch()
    {
        // Example: when left Ctrl is held, reduce height.
        if (crouchAction.IsPressed())
        {
            // Adjust CharacterController height (ensure you save original values elsewhere)
            controller.height = 1.0f;
        }
        else
        {
            controller.height = 2.0f;
        }
    }

    void ToggleFlashlight()
    {
        // Toggle flashlight on/off when F is pressed
        flashlightOn = !flashlightOn;
        if (flashlight != null)
            flashlight.enabled = flashlightOn;
    }

    void Interact()
    {
        // Handle interaction logic when E is pressed.
        // For example: cast a ray and call an Interact() method on the hit object.
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f))
        {
            hit.collider.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
        }
    }
}
