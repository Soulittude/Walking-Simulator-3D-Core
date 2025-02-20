using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private Transform head;          // Reference to Head transform.
    [SerializeField] private Transform fpsCamera;     // Reference to FPCameraMain.
    [SerializeField] private Transform groundCheck;   // Reference to GroundCheck.

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float verticalLookLimit = 80f;

    private CharacterController controller;
    private PlayerInputActions inputActions;
    private Vector2 movementInput;
    private Vector2 lookInput;
    private Vector3 verticalVelocity;
    private float xRotation;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions();

        // Optional Rigidbody setup
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void Start() => Cursor.lockState = CursorLockMode.Locked;

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        HandleMovementInput();
        HandleLookInput();
        HandleGravity();
    }

    private void FixedUpdate() => ApplyMovement();

    private void HandleMovementInput() => movementInput = inputActions.Player.Move.ReadValue<Vector2>();

    private void HandleLookInput()
    {
        lookInput = inputActions.Player.Look.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        // Vertical rotation (Head)
        xRotation = Mathf.Clamp(xRotation - mouseY, -verticalLookLimit, verticalLookLimit);
        head.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Horizontal rotation (PlayerRoot)
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleGravity()
    {
        verticalVelocity.y = controller.isGrounded ?
            Mathf.Max(verticalVelocity.y, -2f) :
            verticalVelocity.y + gravity * Time.deltaTime;
    }

    private void ApplyMovement()
    {
        Vector3 move = transform.TransformDirection(
            new Vector3(movementInput.x, 0, movementInput.y)
        ) * movementSpeed;

        controller.Move((move + verticalVelocity) * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.TryGetComponent(out Rigidbody rb) && !rb.isKinematic)
        {
            rb.AddForce(controller.velocity * 0.5f, ForceMode.Impulse);
        }
    }
}