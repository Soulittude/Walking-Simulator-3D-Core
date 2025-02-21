using UnityEngine;

public class INTUse : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private string doorName = "Basement Door";
    [SerializeField] private Animator doorAnimator;

    public string GetObjectName() => doorName;
    public InteractionType GetInteractionType() => InteractionType.Use;

    public void OnInteract()
    {
        doorAnimator.SetTrigger("Open");
        // Play creaking sound here
    }
}