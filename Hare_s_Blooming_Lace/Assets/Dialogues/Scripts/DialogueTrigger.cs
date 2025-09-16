
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    public Sprite background;

    // ����� ���������� ��� �������� ������ � ����������� �������
    public bool isQuestSpecific = false;
    public int requiredQuestId; // ID ������ ��� ��������
    public bool requiredIsCompleted; // ������ ���� �������� ��� ���

    [Tooltip("Is there any need in E pressing")]
    public bool needPressE = true;
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

    [Tooltip("Should the dialogue be chosen randomly from the list?")]
    public bool isRandomDialogue = false;



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
        if (currentDialogueIndex >= dialogues.Count)
        {
           
            currentDialogueIndex = dialogues.Count > 0 ? dialogues.Count - 1 : 0;
        }

        if (dialogues[currentDialogueIndex].needPressE) 
        {
            if (isInTrigger && Input.GetKeyDown(interactKey))
            {
                if (DialogueManager.instance != null && !DialogueManager.instance.isDialogueActive)
                {
                    TriggerDialogue();
                }
            }
        }
        else
        {
            if (isInTrigger)
            {
                if (DialogueManager.instance != null && !DialogueManager.instance.isDialogueActive)
                {
                    TriggerDialogue();
                }
            }
        }
    }

    public void TriggerDialogue()
    {
        Dialogue currentDialogue = null;

        if (isRandomDialogue)
        {
            if (dialogues.Count > 0)
            {
                int randomIndex = Random.Range(0, dialogues.Count);
                currentDialogue = dialogues[randomIndex];
                Debug.Log($"��������������� ��������� ������ ��� �������: {randomIndex}");
            }
            else
            {
                Debug.LogWarning("������ �������� ����, ������ ��������������.");
                return;
            }
        }
        else
        {
            if (currentDialogueIndex < dialogues.Count)
            {
                currentDialogue = dialogues[currentDialogueIndex];
            }
            else
            {
                if (fallbackDialogue != null)
                {
                    DialogueManager.instance.StartDialogue(fallbackDialogue, this);
                }
                Debug.Log("��� ������� � ������ ��� ��������.");
                return;
            }
        }

        if (currentDialogue != null)
        {
            if (currentDialogue.isQuestSpecific)
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
            else
            {
                DialogueManager.instance.StartDialogue(currentDialogue, this);
            }
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


