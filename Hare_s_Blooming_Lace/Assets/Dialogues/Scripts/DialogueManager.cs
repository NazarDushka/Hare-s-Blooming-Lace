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

    // Ссылка на Rect Transform диалогового окна
    public RectTransform dialogueBoxRect;
    // Ссылка на объект игрока
    public Transform player;

    // Коэффициент масштабирования
    // Если 1 метр в игре = 100 пикселям на Canvas
    private float scaleFactor = 100f;

    // Новые переменные для ограничения движения по X
    public float minX = -400f; // Установи нужные значения в инспекторе
    public float maxX = 400f; // Установи нужные значения в инспекторе


    void LateUpdate()
    {
        if (player != null)
        {
            // Получаем текущую позицию Rect Transform
            Vector2 currentPosition = dialogueBoxRect.anchoredPosition;

            // Обновляем только координату X, масштабируя её
            float scaledX = player.position.x * scaleFactor;

            // Ограничиваем X-координату, чтобы она не выходила за пределы
            float clampedX = Mathf.Clamp(scaledX, minX, maxX);

            currentPosition.x = clampedX;

            // Присваиваем обновлённую позицию, сохраняя Y
            dialogueBoxRect.anchoredPosition = currentPosition;
        }
    }

    private void Awake()
    {
        // Проверяем, существует ли уже экземпляр
        if (instance == null)
        {
            // Если нет, назначаем себя как экземпляр
            instance = this;
            // Запрещаем уничтожение при загрузке новых сцен
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Если экземпляр уже существует, уничтожаем этот объект
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