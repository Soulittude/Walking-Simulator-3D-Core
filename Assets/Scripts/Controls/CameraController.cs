using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float clampAngle = 85f;

    [Header("Head Bob")]
    [SerializeField] private float bobAmount = 0.05f;
    [SerializeField] private float bobSpeed = 14f;

    private PlayerMovement playerMovement;
    private InputAction lookAction;
    private float xRotation = 0f;
    private float defaultYPos;
    private float timer = 0f;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>(); // Cache reference
        defaultYPos = transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;

        var playerInput = GetComponentInParent<PlayerInput>();
        lookAction = playerInput.actions["Look"];
    }

    private void Update()
    {
        if (GameManager.Instance.IsPaused || !GameManager.Instance.CanLook) return;

        HandleMouseLook();
        HandleHeadBob();
    }

    private void HandleMouseLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;

        xRotation -= lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -clampAngle, clampAngle);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.parent.Rotate(Vector3.up * lookInput.x);
    }

    private void HandleHeadBob()
    {
        // Use the cached playerMovement reference
        if (playerMovement.IsMoving && playerMovement.IsGrounded)
        {
            timer += Time.deltaTime * bobSpeed;
            float newY = defaultYPos + Mathf.Sin(timer) * bobAmount;
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                newY,
                transform.localPosition.z
            );
        }
        else
        {
            timer = 0f;
            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                new Vector3(transform.localPosition.x, defaultYPos, transform.localPosition.z),
                Time.deltaTime * bobSpeed
            );
        }
    }
}