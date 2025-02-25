using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DLG_New", menuName = "Dialogue/New Dialogue")]
public class Dialogue : ScriptableObject
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string speakerName;
        public Color textColor;
        public float charactersPerSecond;

        [TextArea(3, 10)]
        public string line;
        
    }

    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}