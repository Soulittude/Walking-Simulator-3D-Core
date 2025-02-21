public enum InteractionType { Examine, Take, Use, Talk }

public interface IInteractable
{
    string GetObjectName();
    InteractionType GetInteractionType();
    void OnInteract();
}