using UnityEngine;

public class INTExamine : MonoBehaviour, IInteractable
{
    [SerializeField] private string objectName = "Painting";
    [TextArea][SerializeField] private string description;

    public string GetObjectName() => objectName;
    public InteractionType GetInteractionType() => InteractionType.Examine;

    public void OnInteract()
    {
        Debug.Log(description); // Show in UI later
    }
}