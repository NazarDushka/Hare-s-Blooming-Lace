using UnityEngine;
using System.Linq;
using System.Collections.Generic; // Добавлена директива для IEnumerable<T>

public class QuestChecker : MonoBehaviour
{
    // ID квеста, который необходим для успеха
    public int requiredQuestId;

    [Tooltip("Если true, условие будет '>= requiredQuestId'. Если false, '<= requiredQuestId'.")]
    public bool ifEqualsOrGreater = true;

    [Tooltip("Проверять только активные квесты? (Если false, будут проверяться завершенные).")]
    public bool checkActiveQuests = true;

    [Tooltip("Проверять завершенные квесты?")]
    public bool checkCompletedQuests = false;

    // Массив объектов, которые будут активированы при успешной проверке
    public GameObject[] OnSuccess;
    // Массив объектов, которые будут активированы при неудачной проверке
    public GameObject[] OnFailure;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckQuestStatus();
        }
    }

    public void CheckQuestStatus()
    {
        Debug.Log("QuestChecker: Начинаем проверку квестов...");

        if (QuestManager.instance == null)
        {
            Debug.LogError("QuestChecker: QuestManager не найден в сцене!");
            return;
        }

        bool checkSucceeded = false;

        // Инициализируем questsToCheck как пустую коллекцию типа Quest
        IEnumerable<Quest> questsToCheck = new List<Quest>();

        if (checkActiveQuests)
        {
            questsToCheck = questsToCheck.Concat(QuestManager.instance.activeQuests);
        }

        if (checkCompletedQuests)
        {
            // Здесь будет ошибка, пока вы не добавите completedQuests в QuestManager
            questsToCheck = questsToCheck.Concat(QuestManager.instance.completedQuests);
        }

        // Проверяем, есть ли хотя бы один квест, соответствующий условию
        if (ifEqualsOrGreater)
        {
            checkSucceeded = questsToCheck.Any(q => q.id >= requiredQuestId);
        }
        else
        {
            checkSucceeded = questsToCheck.Any(q => q.id <= requiredQuestId);
        }

        if (checkSucceeded)
        {
            Debug.Log($"QuestChecker: Успех! Найден квест с ID, соответствующим условию.");
            SetObjectsActive(OnSuccess, true);
            SetObjectsActive(OnFailure, false);
        }
        else
        {
            Debug.Log($"QuestChecker: Неудача! Не найдено квестов, соответствующих условию.");
            SetObjectsActive(OnSuccess, false);
            SetObjectsActive(OnFailure, true);
        }
    }

    private void SetObjectsActive(GameObject[] objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }
        }
    }
}