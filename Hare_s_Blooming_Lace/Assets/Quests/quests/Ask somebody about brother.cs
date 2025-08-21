using UnityEngine;

public class Asksomebodyaboutbrother : MonoBehaviour
{
    // Уникальный ID для этого квестового триггера
    public string uniqueId;

    [Tooltip("ID квеста, который нужно завершить.")]
    public int questIdToComplete = 0;

    [Tooltip("ID квеста, который нужно активировать.")]
    public int questIdToActivate = 1;

    private bool playerInRange = false;

    // Метод Awake вызывается при загрузке объекта
    private void Awake()
    {
        // Восстанавливаем состояние hasActivated из менеджера квестов
        if (QuestManager.instance != null)
        {
            // QuestManager.GetQuestState возвращает false, если состояние уже сохранено
            // isFirstInteraction = QuestManager.instance.GetQuestState(uniqueId);
            // Эта логика обратная, поэтому мы будем использовать ее напрямую.
            // Если GetQuestState вернет true (первая встреча), значит, мы еще не активировали.
            bool hasNotActivated = QuestManager.instance.GetQuestState(uniqueId);
            if (!hasNotActivated)
            {
                // Если состояние "уже активировано" (false) сохранено, отключаем скрипт
                this.enabled = false;
            }
        }
    }

    // Метод вызывается, когда другой коллайдер входит в триггер
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Игрок вошел в зону квест триггера.");
        }
    }

    // Метод вызывается, когда другой коллайдер выходит из триггера
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Игрок вышел из зоны квест триггера.");
        }
    }

    private void Update()
    {
        // Проверяем, находится ли игрок в зоне и нажал ли кнопку
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && QuestManager.instance != null)
        {
            Debug.Log("Кнопка E нажата, активируем квест.");

            // Завершаем квест с ID 0
            QuestManager.instance.CompleteQuest(questIdToComplete);

            // Активируем квест с ID 1
            QuestManager.instance.SetActiveQuest(questIdToActivate);

            // Сохраняем состояние в QuestManager, чтобы квест больше не активировался
            QuestManager.instance.SaveQuestState(uniqueId, false);

            // Отключаем этот скрипт, чтобы предотвратить повторную активацию
            this.enabled = false;
        }
        else if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogError("QuestManager не найден в сцене.");
        }
    }
}