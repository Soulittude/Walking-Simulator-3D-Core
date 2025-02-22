using UnityEngine;

// Add this script to ANY object you want to be interactable (doors, levers, etc.)
public class INTUse : MonoBehaviour, IInteractable
{
    [Header("Basic Settings")]
    [Tooltip("The text shown when looking at the object")]
    public string objectName = "Object"; // Example: "Creaky Door", "Lever"

    [Tooltip("Time between interactions (prevents spamming)")]
    public float cooldown = 2f; // Set to 0 for instant interactions

    [Header("Animation")]
    [Tooltip("Drag your object's Animator here")]
    public Animator objectAnimator;

    [Tooltip("Name of the TRIGGER parameter in your Animator")]
    public string animationTrigger = "Activate"; // Must match your Animator's trigger name

    [Header("Sound")]
    [Tooltip("Sound played when interacting")]
    public AudioClip interactionSound;

    // Private variables
    private float _lastInteractionTime;
    private AudioSource _audioSource;

    // Interface requirement
    public string GetObjectName() => objectName;
    public InteractionType GetInteractionType() => InteractionType.Use;

    void Start()
    {
        // Auto-setup audio source
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.spatialBlend = 1f; // Makes sound 3D
    }

    public void OnInteract()
    {
        // Prevent spamming interactions during cooldown
        if (Time.time < _lastInteractionTime + cooldown) return;

        // Play animation if available
        if (objectAnimator != null)
        {
            objectAnimator.SetTrigger(animationTrigger);
        }

        // Play sound if available
        if (interactionSound != null)
        {
            _audioSource.pitch = Random.Range(0.95f, 1.05f); // Small pitch variation
            _audioSource.PlayOneShot(interactionSound);
        }

        // Update last interaction time
        _lastInteractionTime = Time.time;
    }

    // Editor-only check to help with setup
#if UNITY_EDITOR
    void OnValidate()
    {
        // Auto-get Animator if not set
        if (objectAnimator == null)
        {
            objectAnimator = GetComponent<Animator>();
        }
    }
#endif
}