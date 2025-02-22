/*using UnityEngine;

public class INTUse : MonoBehaviour, IInteractable
{
    [Header("Use Settings")]
    [SerializeField] private string useName = "Door";
    [SerializeField] private Animator objectAnimator;

    public string GetObjectName() => useName;
    public InteractionType GetInteractionType() => InteractionType.Use;

    public void OnInteract()
    {
        objectAnimator.SetTrigger("Open");
        // Example: objectAnimator.SetBool("IsOpen", !objectAnimator.GetBool("IsOpen"));
    }
}*/

using UnityEngine;

public class INTUse : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    [SerializeField] private string useName = "Door";
    /*[SerializeField]*/ private Animator doorAnimator;
    [SerializeField] private AudioClip openSound;

    private bool _isOpen;

    public string GetObjectName() => useName;
    public InteractionType GetInteractionType() => InteractionType.Use;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    public void OnInteract()
    {
        _isOpen = !_isOpen; // Toggle state
        doorAnimator.SetTrigger("Activate");

        if (openSound != null)
        {
            AudioSource.PlayClipAtPoint(openSound, transform.position);
        }
    }
}