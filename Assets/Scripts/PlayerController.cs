using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Look Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float maxLookAngle = 90f;

    [Header("Head Bob")]
    [SerializeField] private float bobSpeed = 14f;
    [SerializeField] private float bobAmount = 0.05f;
    private float defaultYPos;
    private float timer = 0;

    private CharacterController characterController;
    private Vector3 velocity;
    private float xRotation = 0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        defaultYPos = cameraTransform.localPosition.y;
        
        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleHeadBob();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Get movement speed
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        // Calculate movement direction
        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * currentSpeed * Time.deltaTime);

        // Apply gravity
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Jump (optional - remove if not needed)
        if (Input.GetButtonDown("Jump") && characterController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical rotation (up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (left/right)
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleHeadBob()
    {
        if (characterController.velocity.magnitude > 0.1f && characterController.isGrounded)
        {
            timer += Time.deltaTime * bobSpeed;
            float newY = defaultYPos + Mathf.Sin(timer) * bobAmount;
            cameraTransform.localPosition = new Vector3(
                cameraTransform.localPosition.x,
                newY,
                cameraTransform.localPosition.z
            );
        }
        else
        {
            // Reset camera position
            timer = 0;
            cameraTransform.localPosition = Vector3.Lerp(
                cameraTransform.localPosition,
                new Vector3(
                    cameraTransform.localPosition.x,
                    defaultYPos,
                    cameraTransform.localPosition.z
                ),
                Time.deltaTime * bobSpeed
            );
        }
    }
}