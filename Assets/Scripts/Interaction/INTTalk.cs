using UnityEngine;

public class INTTalk : MonoBehaviour, IInteractable
{
    [Header("Dialogue Settings")]
    [SerializeField] private string characterName = "Name";
    [SerializeField] private Color characterColor = Color.white;
    //[SerializeField] private DialogueSequence currentDialogue;
    //I think we should handle; italic, bold, typing speed at created dialogue files. Because its more developer friendly, we can use them at inside of text easily.
    //Also i guess we need default settings for dialogues. Typing speed, typing color, optional typing sound.

    public string GetObjectName() => characterName;
    public InteractionType GetInteractionType() => InteractionType.Talk;

    public void OnInteract()
    {
    }
}