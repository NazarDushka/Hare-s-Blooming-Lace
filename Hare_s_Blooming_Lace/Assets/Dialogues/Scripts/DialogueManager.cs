using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public Image PlayerIcon;
    public Image NpcIcon;
    public Vector2 IconPosition;
    public TextMeshProUGUI DialogueArea;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;
    public float typingspeed = 0.2f;
    public Animator animator;

    public float NextDialogueLineTime = 5f;
    private bool isTyping = false;

    private float playAnimTime = 1.5f;
    private float hideAnimTime = 1.5f;
    private Dialogue currentDialogue;
    private DialogueTrigger currentDialogueTrigger;

    // ������ �� Rect Transform ����������� ����
    public RectTransform dialogueBoxRect;
    // ������ �� ������ ������
    public Transform player;

    // ����������� ���������������
    // ���� 1 ���� � ���� = 100 �������� �� Canvas
    private float scaleFactor = 100f;

    // ����� ���������� ��� ����������� �������� �� X
    public float minX = -400f; // �������� ������ �������� � ����������
    public float maxX = 400f; // �������� ������ �������� � ����������


    void LateUpdate()
    {
        if (player != null)
        {
            // �������� ������� ������� Rect Transform
            Vector2 currentPosition = dialogueBoxRect.anchoredPosition;

            // ��������� ������ ���������� X, ����������� �
            float scaledX = player.position.x * scaleFactor;

            // ������������ X-����������, ����� ��� �� �������� �� �������
            float clampedX = Mathf.Clamp(scaledX, minX, maxX);

            currentPosition.x = clampedX;

            // ����������� ���������� �������, �������� Y
            dialogueBoxRect.anchoredPosition = currentPosition;
        }
    }

    private void Awake()
    {
        // ���������, ���������� �� ��� ���������
        if (instance == null)
        {
            // ���� ���, ��������� ���� ��� ���������
            instance = this;
            // ��������� ����������� ��� �������� ����� ����
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���� ��������� ��� ����������, ���������� ���� ������
            Destroy(gameObject);
        }
        lines = new Queue<DialogueLine>();
    }


    public void StartDialogue(Dialogue dialogue, DialogueTrigger trigger)
    {
        isDialogueActive = true;
        lines.Clear();
        currentDialogue = dialogue;
        currentDialogueTrigger = trigger;

        if (currentDialogue.Player != null) currentDialogue.Player.SetActive(false);
        if (currentDialogue.Npc != null) currentDialogue.Npc.SetActive(false);

        PlayerIcon.enabled = false;
        NpcIcon.enabled = false;

        foreach (DialogueLine line in dialogue.dialoguelines)
        {
            lines.Enqueue(line);
        }

        StartCoroutine(StartDialogueWithDelay());
    }

    IEnumerator StartDialogueWithDelay()
    {
        animator.Play("play");
        yield return new WaitForSeconds(playAnimTime);
        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue(currentDialogue);
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        PlayerIcon.enabled = false;
        NpcIcon.enabled = false;
        animator.Play("Line");

        if (currentLine.isPlayer)
        {
            if (PlayerIcon != null && currentLine.character.icon != null)
            {
                PlayerIcon.sprite = currentLine.character.icon;
                PlayerIcon.enabled = true;
            }
        }
        else
        {
            if (NpcIcon != null && currentLine.character.icon != null)
            {
                NpcIcon.sprite = currentLine.character.NpcIcon;
                NpcIcon.enabled = true;
            }
        }


        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        isTyping = true;
        DialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            DialogueArea.text += letter;
            yield return new WaitForSeconds(typingspeed);
        }
        isTyping = false;


        StartCoroutine(WaitForPlayerInputOrAutoAdvance());
    }

    IEnumerator WaitForPlayerInputOrAutoAdvance()
    {
        float timer = 0f;

        while (timer < NextDialogueLineTime)
        {
            timer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.E))
            {

                if (!isTyping)
                {

                    break;
                }
            }

            yield return null;
        }

        DisplayNextDialogueLine();
    }

    void EndDialogue(Dialogue dialogue)
    {
        isDialogueActive = false;
        StartCoroutine(EndDialogueWithDelay(dialogue));
    }

    IEnumerator EndDialogueWithDelay(Dialogue dialogue)
    {
        animator.Play("hide");
        yield return new WaitForSeconds(hideAnimTime);

        if (dialogue.Player != null) dialogue.Player.SetActive(true);
        if (dialogue.Npc != null) dialogue.Npc.SetActive(true);

        if (currentDialogueTrigger != null)
        {
            currentDialogueTrigger.AdvanceDialogueState();
        }

        currentDialogue = null;
        currentDialogueTrigger = null;
        DialogueArea.text = "";
    }

}