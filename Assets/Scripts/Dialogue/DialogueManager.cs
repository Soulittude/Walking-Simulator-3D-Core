using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel; // The dialogue UI panel.
    [SerializeField] private TMP_Text speakerNameText;   // TextMeshPro for the speaker's name.
    [SerializeField] private TMP_Text dialogueText;      // TextMeshPro for the dialogue content.

    [Header("Typewriter Settings")]
    [SerializeField] private float lettersPerSecond = 30f; // Speed for the typewriter effect.
    [SerializeField] private int maxCharactersPerLine = 30; // Approximate max characters per line.

    // Queue to hold all dialogue pages.
    private Queue<DialoguePage> dialoguePages = new Queue<DialoguePage>();
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    // Public property to check if dialogue is active.
    public bool IsDialogueActive => dialoguePanel.activeSelf;

    private void Awake()
    {
        // Singleton pattern.
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Begins a dialogue using the provided Dialogue asset.
    /// If a dialogue is already active, the call is ignored.
    /// </summary>
    public void StartDialogue(string overrideSpeakerName, Dialogue dialogue)
    {
        // If a dialogue is already active, ignore new calls.
        if (IsDialogueActive)
        {
            Debug.Log("Dialogue already active; ignoring new start call.");
            return;
        }

        // Check that the Dialogue asset has at least one dialogue line.
        if (dialogue == null || dialogue.dialogueLines == null || dialogue.dialogueLines.Count == 0)
        {
            Debug.Log("Dialogue asset has no dialogue lines.");
            return;
        }

        // Disable player controls.
        GameManager.Instance.SetControlState(false, false, false);

        // Activate the dialogue UI panel.
        dialoguePanel.SetActive(true);
        dialoguePages.Clear();

        // For each dialogue line in the asset...
        foreach (var dialogueLine in dialogue.dialogueLines)
        {
            // Use the override speaker name if provided; otherwise, use the dialogue line’s speaker.
            string speaker = string.IsNullOrEmpty(overrideSpeakerName) ? dialogueLine.speakerName : overrideSpeakerName;
            // Calculate maximum characters per page (here approximating 2 lines).
            int maxCharsPerPage = maxCharactersPerLine * 2;
            // Split the dialogue line into pages.
            List<string> pages = PaginateText(dialogueLine.line, maxCharsPerPage);
            // Enqueue each page along with its speaker.
            foreach (var page in pages)
            {
                DialoguePage dp = new DialoguePage
                {
                    speakerName = speaker,
                    pageText = page
                };
                dialoguePages.Enqueue(dp);
            }
        }
        DisplayNextPage();
    }

    /// <summary>
    /// Splits a block of text into pages based on the maxChars limit.
    /// This method avoids splitting words.
    /// </summary>
    private List<string> PaginateText(string text, int maxChars)
    {
        List<string> pages = new List<string>();
        if (string.IsNullOrEmpty(text))
        {
            pages.Add("");
            return pages;
        }

        string[] words = text.Split(' ');
        string currentPage = "";

        foreach (string word in words)
        {
            if (currentPage.Length + word.Length + 1 > maxChars)
            {
                pages.Add(currentPage.Trim());
                currentPage = word + " ";
            }
            else
            {
                currentPage += word + " ";
            }
        }
        if (!string.IsNullOrEmpty(currentPage))
            pages.Add(currentPage.Trim());

        return pages;
    }

    /// <summary>
    /// Displays the next page from the queue with the typewriter effect.
    /// </summary>
    private void DisplayNextPage()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (dialoguePages.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialoguePage currentPage = dialoguePages.Peek();
        speakerNameText.text = currentPage.speakerName;
        typingCoroutine = StartCoroutine(TypeSentence(currentPage.pageText));
    }

    /// <summary>
    /// Reveals the sentence letter by letter.
    /// </summary>
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        isTyping = false;
    }

    private void Update()
    {
        if (!IsDialogueActive)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isTyping)
            {
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);
                DialoguePage currentPage = dialoguePages.Peek();
                dialogueText.text = currentPage.pageText;
                isTyping = false;
            }
            else
            {
                dialoguePages.Dequeue();
                if (dialoguePages.Count > 0)
                {
                    DisplayNextPage();
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    /// <summary>
    /// Ends the dialogue, hides the dialogue UI, and re-enables player controls.
    /// </summary>
    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        GameManager.Instance.SetControlState(true, true, true);
        INTTalk.SetJustEndedDialogue();
    }

    // Struct for holding dialogue page info.
    private struct DialoguePage
    {
        public string speakerName;
        public string pageText;
    }
}
