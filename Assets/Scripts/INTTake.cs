using UnityEngine;

public class INTTake : MonoBehaviour, IInteractable
{
    [SerializeField] private string itemName = "Book";

    public string GetObjectName() => itemName;
    public InteractionType GetInteractionType() => InteractionType.Take;

    public void OnInteract()
    {
        Debug.Log($"Took {itemName}");
        // Add to inventory & destroy object
        Destroy(gameObject);
    }
}