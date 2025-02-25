using UnityEngine;

public class INTTalk : MonoBehaviour, IInteractable
{
    [Header("Character Settings")]
    [SerializeField] private string characterName = "Name";
    [SerializeField] private Color characterColor = Color.white;
    [SerializeField] private float talkingSpeed = 30f;

    [Header("Dialogue")]
    [SerializeField] private Dialogue dialogue;

    // This flag prevents starting dialogue immediately after one just ended.
    private static bool justEndedDialogue = false;

    public string GetObjectName() => characterName;
    public InteractionType GetInteractionType() => InteractionType.Talk;

    public void OnInteract()
    {
        // Only start dialogue if one is not active and we didn't just end one.
        if (DialogueManager.Instance != null &&
            !DialogueManager.Instance.IsDialogueActive &&
            !justEndedDialogue &&
            dialogue != null)
        {
            DialogueManager.Instance.StartDialogue(characterName, dialogue);
        }
    }

    // You could reset the justEndedDialogue flag after a short delay.
    private void LateUpdate()
    {
        // Reset flag at the end of frame.
        if (justEndedDialogue)
            justEndedDialogue = false;
    }

    // This method can be called from DialogueManager.EndDialogue() to prevent immediate re‑triggering.
    public static void SetJustEndedDialogue()
    {
        justEndedDialogue = true;
    }
}
