using UnityEngine;

public class INTTake : MonoBehaviour, IInteractable
{
    [Header("Take Settings")]
    [SerializeField] private string itemName = "Item";
    [Tooltip("Optional sound when picked up")]
    [SerializeField] private AudioClip pickupSound;

    public string GetObjectName() => itemName;
    public InteractionType GetInteractionType() => InteractionType.Take;

    public void OnInteract()
    {
        // Add to inventory system later
        Debug.Log($"Took {itemName}");
        if (pickupSound) AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        Destroy(gameObject);
    }
}