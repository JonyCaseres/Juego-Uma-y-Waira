using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public int dialogueIndex; // Índice de la línea donde aparece la pregunta
    public string[] choices; // Textos de las respuestas
    public int[] nextDialogueIndexes; // A qué línea salta cada respuesta
}

[CreateAssetMenu(fileName = "New NPC Dialogue", menuName = "NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    public string NPCname;
    public Sprite NPCportrait;

    [TextArea(2, 5)]
    public string[] dialogueLines;
    public bool[] autoProgressLines;
    public bool[] endDialogueLines; // Marca en qué líneas debe terminar el diálogo

    public float typingSpeed = 0.05f;
    public float autoProgressDelay = 1.5f;

    public AudioClip voiceSound;
    public float voicePitch = 1f;

    public DialogueChoice[] choices;
}
