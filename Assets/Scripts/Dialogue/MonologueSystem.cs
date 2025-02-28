using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MonologueSystem : MonoBehaviour
{
    public Text dialogueText; // UI Text component to display the dialogue
    public float typingSpeed = 0.05f; // Time interval between each character
    public AudioSource typingSound; // AudioSource for typing sound effect

    private Queue<string> sentences; // Queue to hold dialogue sentences
    private bool isTyping = false; // Flag to check if typing is in progress

    void Start()
    {
        sentences = new Queue<string>();
    }

    // Call this method to start the dialogue
    public void StartDialogue(string[] dialogueLines)
    {
        sentences.Clear();
        foreach (string line in dialogueLines)
        {
            sentences.Enqueue(line);
        }
        DisplayNextSentence();
    }

    // Display the next sentence in the queue
    public void DisplayNextSentence()
    {
        if (isTyping) return; // Prevent skipping during typing

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(sentence));
    }

    // Coroutine to type out each character
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            if (typingSound != null)
            {
                typingSound.Play();
            }
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    // End the dialogue
    void EndDialogue()
    {
        dialogueText.text = "";
        // Additional logic for when dialogue ends
    }

    void Update()
    {
        // Check for player input to display the next sentence
        if (Input.GetMouseButtonDown(0) && !isTyping)
        {
            DisplayNextSentence();
        }
    }
}
