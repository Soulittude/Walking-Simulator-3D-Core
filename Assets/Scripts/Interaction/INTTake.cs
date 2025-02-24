using UnityEngine;

public class INTTake : MonoBehaviour, IInteractable
{
    [Header("Take Settings")]
    [Tooltip("Name shown in interaction prompt")]
    public string objectName = "Item";

    [Tooltip("Offset in player's left hand")]
    public Vector3 holdOffset = new Vector3(-0.3f, -0.2f, 0.3f);

    [Header("References")]
    [SerializeField] private Transform leftHand;
    [SerializeField] private PlayerMovement playerMovement; // Reference to PlayerMovement

    private bool _isHeld;
    private Vector3 _originalScale;
    private Transform _originalParent;
    private Rigidbody _rb;

    public string GetObjectName() => objectName;
    public InteractionType GetInteractionType() => InteractionType.Take;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _originalScale = transform.localScale;
        _originalParent = transform.parent;
    }

    void Update()
    {
        if (_isHeld && Input.GetKeyDown(KeyCode.Q))
            Drop();
    }

    public void OnInteract()
    {
        if (!_isHeld)
            Take();
        else
            Drop();
    }

    private void Take()
    {
        _isHeld = true;

        // Save original state
        _originalParent = transform.parent;
        _originalScale = transform.localScale;

        // Attach to hand
        transform.SetParent(leftHand);
        transform.localPosition = holdOffset;
        transform.localRotation = Quaternion.identity;
        transform.localScale = _originalScale;

        // Disable physics
        GetComponent<Collider>().enabled = false;
        if (_rb != null)
            _rb.isKinematic = true;
    }

    private void Drop()
    {
        _isHeld = false;

        // Restore original state
        transform.SetParent(_originalParent);
        transform.localScale = _originalScale;

        // Enable physics
        GetComponent<Collider>().enabled = true;
        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.linearVelocity = playerMovement.Velocity; // Use player's velocity
            _rb.AddForce(leftHand.forward * 5f, ForceMode.Impulse);
        }
    }
}