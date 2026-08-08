using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueData", menuName = "Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] dialogueLines;
}
