using UnityEngine;

public class INTTalk : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcName = "Nick";

    public string GetObjectName() => npcName;
    public InteractionType GetInteractionType() => InteractionType.Talk;

    public void OnInteract()
    {
        Debug.Log($"Started conversation with {npcName}");
        // Trigger dialogue system here
    }
}