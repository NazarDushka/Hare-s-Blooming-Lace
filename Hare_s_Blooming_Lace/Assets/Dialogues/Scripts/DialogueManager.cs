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
    public Image Background;
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

    public RectTransform dialogueBoxRect;
    public Transform player;

    private float scaleFactor = 100f;
    public float minX = -400f;
    public float maxX = 400f;

    void LateUpdate()
    {
        if (player != null)
        {
            Vector2 currentPosition = dialogueBoxRect.anchoredPosition;
            float scaledX = player.position.x * scaleFactor;
            float clampedX = Mathf.Clamp(scaledX, minX, maxX);
            currentPosition.x = clampedX;
            dialogueBoxRect.anchoredPosition = currentPosition;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
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
        if (currentDialogue.background != null)
        {
            Background.sprite = currentDialogue.background;
        }

        StartCoroutine(PrepareCharactersForDialogue(currentDialogue.Player, false));
        SetCharacterControl(currentDialogue.Npc, false); // НПС не нуждается в ожидании

        PlayerIcon.enabled = false;
        NpcIcon.enabled = false;

        foreach (DialogueLine line in dialogue.dialoguelines)
        {
            lines.Enqueue(line);
        }

        StartCoroutine(StartDialogueWithDelay());
    }

 
    IEnumerator PrepareCharactersForDialogue(GameObject playerCharacter, bool isEnabled)
    {

        PlayerController playerController = playerCharacter.GetComponent<PlayerController>();

        // Если мы отключаем игрока и он не на земле, ждём, пока он приземлится
        if (!isEnabled && playerController != null && !playerController.isGrounded)
        {
            // Используем while с 'yield return null', чтобы ждать по одному кадру
            while (!playerController.isGrounded)
            {
                yield return null;
            }
        }

        // Теперь, когда игрок в нужном состоянии (на земле), отключаем его управление
        SetCharacterControl(playerCharacter, isEnabled);
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

            if (Input.GetKeyDown(KeyCode.E) && !isTyping)
            {
                break;
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

        SetCharacterControl(currentDialogue.Player, true);
        SetCharacterControl(currentDialogue.Npc, true);

        if (currentDialogueTrigger != null)
        {
            currentDialogueTrigger.AdvanceDialogueState();
        }

        currentDialogue = null;
        currentDialogueTrigger = null;
        DialogueArea.text = "";
    }

    /// <summary>
    /// Универсальный метод для включения/выключения управления, физики и анимации персонажа.
    /// </summary>
    /// <param name="character">Игровой объект персонажа.</param>
    /// <param name="isEnabled">True для включения, False для выключения.</param>
    private void SetCharacterControl(GameObject character, bool isEnabled)
    {
        if (character == null) return;

        PlayerController playerController = character.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.enabled = isEnabled;
            playerController.currentSpeed = 0; 
        }

        Rigidbody2D rb = character.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.simulated = isEnabled;
        }

        Collider2D[] colliders = character.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (collider != null)
            {
                collider.enabled = isEnabled;
            }
        }

        Animator anim = character.GetComponent<Animator>();
        if (anim != null && !isEnabled)
        {
            anim.SetFloat("Speed", 0);
            anim.Play("HareIdle");
        }
    }
}