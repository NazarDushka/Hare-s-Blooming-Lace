using System.Collections;
using UnityEngine;

public class QuestCompleter : MonoBehaviour
{
    [Tooltip("ID квеста, который нужно завершить при взаимодействии.")]
    public int questIdToComplete = 3;

    [Tooltip("ID квестов, которые нужно проверить на завершение перед активацией следующего.")]
    public int questIdToVerify_1 = 2;
    public int questIdToVerify_2 = 3;

    [Tooltip("ID квеста, который нужно запустить после выполнения всех проверок.")]
    public int nextQuestId = 4;

    public Animator animator;
    public float destroyDelay = 2f; // Задержка в секундах перед уничтожением

    public GameObject player; // Ссылка на игрока
    public Animator blackoutAnimator; // Аниматор для затемнения экрана

    public DialogueTrigger dialogueToPlay; // Триггер диалога, который должен проиграться первым
    public bool playDialogueBeforeCutscene = true; // Флаг, включающий проигрывание диалога

    private bool playerInRange = false;
    private bool dialogueFinished = false; // Флаг для отслеживания завершения диалога

    private void Awake()
    {
        Quest quest = QuestManager.instance.GetQuestById(questIdToComplete);
        if (quest != null && quest.isCompleted)
        {
            
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Игрок вошел в зону завершения квеста.");
            if ( QuestManager.instance.GetQuestById(questIdToComplete).isCompleted)
            {
               
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Игрок вышел из зоны завершения квеста.");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (QuestManager.instance == null)
            {
                Debug.LogError("QuestManager не найден в сцене!");
                return;
            }

            // Завершаем текущий квест
            QuestManager.instance.CompleteQuest(questIdToComplete);
            Debug.Log($"Квест с ID {questIdToComplete} успешно завершен.");

            // Запускаем корутину, которая сначала проиграет диалог (если нужно), а потом катсцену
            StartCoroutine(HandleQuestCompletionSequence());
        }
    }

   private IEnumerator HandleQuestCompletionSequence()
    {
        Debug.Log("Запуск последовательности завершения квеста.");

        // 1. Проверяем, нужно ли проигрывать диалог
        if (playDialogueBeforeCutscene && dialogueToPlay != null && DialogueManager.instance != null)
        {
            // Если диалог должен проиграться, запускаем его
            Debug.Log("Запуск диалога...");
            DialogueManager.instance.StartDialogue(dialogueToPlay.dialogues[0], dialogueToPlay); // Предполагаем, что первый диалог в списке - это тот, который нужно проиграть. Вам может понадобиться более сложная логика выбора диалога.

            // Ждем, пока диалог не будет активен
            while (DialogueManager.instance.isDialogueActive)
            {
                yield return null; // Ждем следующий кадр
            }
            Debug.Log("Диалог завершен.");
        }
        else if (playDialogueBeforeCutscene && dialogueToPlay == null)
        {
            Debug.LogWarning("Флаг 'playDialogueBeforeCutscene' включен, но 'dialogueToPlay' не назначен.");
        }
        else if (playDialogueBeforeCutscene && DialogueManager.instance == null)
        {
            Debug.LogError("DialogueManager не найден в сцене. Диалог не будет проигран.");
        }

        // 2. Проигрываем катсцену (ваша существующая логика)
        Debug.Log("Запуск катсцены...");
        yield return StartCoroutine(PlayAnimationsInSequence()); // Запускаем вашу текущую корутину для катсцены
        Debug.Log("Катсцена завершена.");

        // 3. Уничтожаем объект и активируем следующий квест
        if (transform.parent != null)
        {
            Debug.Log("Уничтожение родительского объекта.");
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Debug.Log("Уничтожение текущего объекта.");
            Destroy(gameObject);
        }
    }

    // Ваша существующая корутина для проигрывания анимаций катсцены
    private IEnumerator PlayAnimationsInSequence()
    {
        Debug.Log("Корутина PlayAnimationsInSequence запущена.");

        var rb2d = player.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;
            Debug.Log("Скорость игрока сброшена.");
        }

        // --- Здесь ваша логика для катсцены ---
        // Пример:
        //if (blackoutAnimator != null)
        //{
        //    Debug.Log("Запуск анимации BlackIn.");
        //    blackoutAnimator.Play("BlackIn");
        //    yield return new WaitForSeconds(0.5f); // Примерная задержка, можете заменить на ожидание завершения анимации
        //}
        yield return new WaitForSeconds(0.5f);
        player.transform.position = new Vector2(63.56f, -1.674056f); // Перемещение игрока
        Debug.Log("Позиция игрока перемещена.");
        yield return new WaitForSeconds(0.5f);

        if (animator != null)
        {
            Debug.Log("Запуск анимации owlbox.");
            animator.Play("owlbox");
            // Если нужно дождаться завершения анимации "owlbox":
            // yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            yield return new WaitForSeconds(1.0f); // Задержка, пока анимация проигрывается
            Debug.Log("Анимация owlbox завершена.");
        }
        else
        {
            Debug.LogWarning("Animator не назначен, анимация 'owlBox' не будет проиграна.");
        }
        // --- Конец логики катсцены ---

        Debug.Log("Ожидание финальной задержки.");
        yield return new WaitForSeconds(destroyDelay);
        Debug.Log("Финальная задержка завершена.");

        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;
            Debug.Log("Скорость игрока сброшена повторно.");
        }

        //// Логика активации квеста (оставлена как есть)
        //Quest quest2 = QuestManager.instance.GetQuestById(questIdToVerify_1);
        //Quest quest3 = QuestManager.instance.GetQuestById(questIdToVerify_2);

        //// Проверяем, выполнены ли оба квеста
        //if (quest2 != null && quest2.isCompleted && quest3 != null && quest3.isCompleted)
        //{
        QuestManager.instance.SetActiveQuest(nextQuestId);
        //    Debug.Log($"Квесты {questIdToVerify_1} и {questIdToVerify_2} выполнены. Активирован квест {nextQuestId}.");
        //}
        //else
        //{
        //    Debug.Log("Один или оба квеста-предшественника еще не завершены. Следующий квест не активирован.");
        //}
    }
}
