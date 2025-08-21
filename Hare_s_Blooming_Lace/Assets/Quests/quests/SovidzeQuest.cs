using UnityEngine;

public class SovidzeQuest : MonoBehaviour
{
    // Уникальный ID для сохранения состояния первого взаимодействия
    public string uniqueId = "SovaFirstContact";

    [Tooltip("ID квеста, который нужно активировать.")]
    public int questIdToActivate = 3;

    public Animator animator;

    [Tooltip("Название анимации появления.")]
    public string appearingAnimName = "Appearing";

    [Tooltip("Название анимации бездействия.")]
    public string idleAnimName = "Idle";

    private bool playerInRange = false;
    private bool isFirstInteraction;
    private bool hasAppeared = false;


    private void Awake()
    {
        if (animator == null)
        {
            Debug.LogError("Animator не назначен в инспекторе. Пожалуйста, привяжите его!");
            return;
        }

        if (QuestManager.instance != null)
        {
            // Загружаем состояние первого взаимодействия
            isFirstInteraction = QuestManager.instance.GetQuestState(uniqueId);
        }
        Quest quest = QuestManager.instance.GetQuestById(questIdToActivate);
        if (quest.isCompleted)
        {
            animator.Play("Pointer");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Quest quest = QuestManager.instance.GetQuestById(questIdToActivate);

        if (other.CompareTag("Player") && !hasAppeared && !quest.isCompleted)
        {
            playerInRange = true;
            Debug.Log("Игрок вошел в зону взаимодействия с Совой.");

            if (animator != null)
            {
                // Запускаем анимацию появления
                animator.Play(appearingAnimName);

                // Устанавливаем флаг, чтобы анимация сработала только один раз
                hasAppeared = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Игрок вышел из зоны взаимодействия с Совой.");
        }
    }

    private void Update()
    {
        // Проверяем, находится ли игрок в зоне и нажал ли клавишу E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (QuestManager.instance == null)
            {
                Debug.LogError("QuestManager не найден в сцене!");
                return;
            }

            if (isFirstInteraction)
            {
                // Если это первое взаимодействие, активируем квест
                QuestManager.instance.SetActiveQuest(questIdToActivate);
                Debug.Log($"Активирован квест с ID {questIdToActivate}.");

                // Сохраняем состояние, чтобы избежать повторной активации
                isFirstInteraction = false;
                QuestManager.instance.SaveQuestState(uniqueId, isFirstInteraction);
            }
            // Можно добавить else-блок для диалога, если квест уже активирован
        }
    }
}
