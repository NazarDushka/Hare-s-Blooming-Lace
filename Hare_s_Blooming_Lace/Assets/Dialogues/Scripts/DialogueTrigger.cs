
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



[System.Serializable]
public class DialogueCharacter
{ 
 public Sprite icon;
public Sprite NpcIcon;
}

[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    public bool isPlayer;
    [TextArea(3,10)]
    public string line;
}
[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialoguelines= new List<DialogueLine> ();
    public GameObject Player;
    public GameObject Npc; 
}

public class DialogueTrigger : MonoBehaviour
{
    public List<Dialogue> dialogues = new List<Dialogue>();
    private int currentDialogueIndex = 0;

    private bool isInTrigger = false;


    public KeyCode interactKey = KeyCode.E;


    void Update()
    {
       
        if (isInTrigger && Input.GetKeyDown(interactKey))
        {
           
            if (DialogueManager.instance != null && !DialogueManager.instance.isDialogueActive)
            {
                TriggerDialogue();
            }
        }
    }

    public void TriggerDialogue()
    {
        if (DialogueManager.instance != null)
        {
            
            if (currentDialogueIndex < dialogues.Count)
            {
                // Запускаем диалог по текущему индексу
                DialogueManager.instance.StartDialogue(dialogues[currentDialogueIndex], this);
            }
        }
    }

    public void AdvanceDialogueState()
    {
        if (currentDialogueIndex < dialogues.Count - 1)
        {
            currentDialogueIndex++;
            Debug.Log("Диалог переключен на состояние: " + currentDialogueIndex);
        }
        else
        {
            Debug.Log("Достигнуто последнее состояние диалога.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInTrigger = true;
            Debug.Log("Игрок вошел в зону триггера. Нажмите " + interactKey.ToString() + " для начала диалога.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInTrigger = false;
            Debug.Log("Игрок вышел из зоны триггера.");
        }
    }
}


