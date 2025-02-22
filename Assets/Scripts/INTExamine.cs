using UnityEngine;

public class INTExamine : MonoBehaviour, IInteractable
{
    [Header("Examine Settings")]
    [SerializeField] private string objectName = "Object";
    [TextArea][SerializeField] private string examineText = "You see...";

    public string GetObjectName() => objectName;
    public InteractionType GetInteractionType() => InteractionType.Examine;

    public void OnInteract()
    {
        // Show text in UI (replace with your UI system later)
        Debug.Log(examineText);
    }
}