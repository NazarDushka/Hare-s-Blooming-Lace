
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
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialoguelines = new List<DialogueLine>();
    public GameObject Player;
    public GameObject Npc;

    // ����� ���������� ��� �������� ������ � ����������� �������
    public bool isQuestSpecific = false;
    public int requiredQuestId; // ID ������ ��� ��������
    public bool requiredIsCompleted; // ������ ���� �������� ��� ���
}


public class DialogueTrigger : MonoBehaviour
{
    // ���������� ID ��� ����� ����������� ��������
    public string uniqueId;

    public List<Dialogue> dialogues = new List<Dialogue>();
    public Dialogue fallbackDialogue;
    private int currentDialogueIndex = 0;
    private bool isInTrigger = false;
    public KeyCode interactKey = KeyCode.E;

    // ����� Awake ���������� ��� �������� �������
    private void Awake()
    {
        if (QuestManager.instance != null)
        {
            // ��������������� ����������� ���������
            currentDialogueIndex = QuestManager.instance.GetDialogueState(uniqueId);
        }
    }

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

        Dialogue currentDialogue = null;
        if (currentDialogueIndex < dialogues.Count)
        {
            currentDialogue = dialogues[currentDialogueIndex];
        }

        if (currentDialogue != null && currentDialogue.isQuestSpecific)
        {
            if (CheckQuestStatus(currentDialogue))
            {
                DialogueManager.instance.StartDialogue(currentDialogue, this);
            }
            else
            {
                if (fallbackDialogue != null)
                {
                    DialogueManager.instance.StartDialogue(fallbackDialogue, this);
                }
                else
                {
                    Debug.Log("������� ������ �� ���������, � �������� ������ �� ��������.");
                }
            }
        }
        else if (currentDialogue != null)
        {
            DialogueManager.instance.StartDialogue(currentDialogue, this);
        }
    }

    private bool CheckQuestStatus(Dialogue dialogue)
    {
        if (QuestManager.instance == null)
        {
            Debug.LogError("QuestManager �� ������ � �����!");
            return false;
        }

        Quest questToCheck = QuestManager.instance.GetQuestById(dialogue.requiredQuestId);

        return questToCheck != null && questToCheck.isCompleted == dialogue.requiredIsCompleted;
    }

    public void AdvanceDialogueState()
    {
        if (currentDialogueIndex < dialogues.Count - 1)
        {
            currentDialogueIndex++;
            Debug.Log("������ ���������� �� ���������: " + currentDialogueIndex);

            // ��������� ����� ��������� � ���������
            if (QuestManager.instance != null)
            {
                QuestManager.instance.SaveDialogueState(uniqueId, currentDialogueIndex);
            }
        }
        else
        {
            Debug.Log("���������� ��������� ��������� �������.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInTrigger = false;
        }
    }
}


