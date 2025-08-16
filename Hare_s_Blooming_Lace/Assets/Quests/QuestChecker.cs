using UnityEngine;

public class QuestChecker : MonoBehaviour
{
    // ID квеста, который необходим для успеха
    public int requiredQuestId;

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
        Debug.Log("QuestChecker: Начинаем проверку текущего квеста...");

        if (QuestManager.instance == null)
        {
            Debug.LogError("QuestChecker: QuestManager не найден в сцене!");
            return;
        }

        Quest currentQuest = QuestManager.instance.activeQuest;

        // Если активного квеста нет, или его ID меньше требуемого
        if (currentQuest == null || currentQuest.id < requiredQuestId)
        {
            Debug.Log($"QuestChecker: Неудача! Текущий квест (ID: {(currentQuest != null ? currentQuest.id : 0)}) меньше требуемого (ID: {requiredQuestId}).");

            SetObjectsActive(OnSuccess, false);
            SetObjectsActive(OnFailure, true);
        }
        // Если ID активного квеста равен или больше требуемого
        else
        {
            Debug.Log($"QuestChecker: Успех! Текущий квест (ID: {currentQuest.id}) соответствует или превышает требуемый (ID: {requiredQuestId}).");

            SetObjectsActive(OnSuccess, true);
            SetObjectsActive(OnFailure, false);
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