using UnityEngine;

public class INTExamine : MonoBehaviour, IInteractable
{
    [Header("Examine Settings")]
    [Tooltip("Name shown in interaction prompt")]
    public string objectName = "Object";

    [Tooltip("Offset from camera during examination")]
    public Vector3 examineOffset = new Vector3(0, 0, 1.5f);

    [Tooltip("Rotation speed while examining")]
    public float rotateSpeed = 100f;

    [Header("References")]
    [SerializeField] private Transform playerCamera;

    [Header("Player Control")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CameraController cameraController;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Transform _originalParent;
    private bool _isExamining;

    public string GetObjectName() => objectName;
    public InteractionType GetInteractionType() => InteractionType.Examine;

    void Update()
    {
        if (!_isExamining) 
            return;

            // Freeze player and camera
            //playerMovement.enabled = false;
            //cameraController.enabled = false;

            // Get mouse input
            float x = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            float y = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;

            // Rotate around both axes
            transform.Rotate(Vector3.up, -x, Space.World);
            transform.Rotate(Vector3.right, y, Space.World);

            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;

        if (Input.GetKeyDown(KeyCode.Escape))
            StopExamine();
    }

    public void OnInteract()
    {
        if (!_isExamining)
            StartExamine();
        else
            StopExamine();
    }

    private void StartExamine()
    {
        GameManager.Instance.SetControlState(false, false, false);
        _isExamining = true;

        // Save original state
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;
        _originalParent = transform.parent;

        // Move object to camera view
        transform.SetParent(playerCamera);
        transform.localPosition = examineOffset;
        transform.localRotation = Quaternion.identity;

        // Disable physics/collisions
        GetComponent<Collider>().enabled = false;
        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;
    }

    private void StopExamine()
    {
        GameManager.Instance.SetControlState(true, true, true);

        // Restore controls
        playerMovement.enabled = true;
        cameraController.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _isExamining = false;

        // Restore original state
        transform.SetParent(_originalParent);
        transform.position = _originalPosition;
        transform.rotation = _originalRotation;

        // Re-enable physics/collisions
        GetComponent<Collider>().enabled = true;
        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = false;
    }
}