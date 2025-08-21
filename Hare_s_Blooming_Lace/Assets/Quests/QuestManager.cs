using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CollectedObject
{
    public string objectId;
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public List<Quest> allQuests;

    // Переменная для активного квеста заменена на список
    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();

    public List<CollectedObject> collectedObjects = new List<CollectedObject>();
    public Dictionary<string, int> dialogueTriggerStates = new Dictionary<string, int>();
    public Dictionary<string, bool> questTriggerStates = new Dictionary<string, bool>();

    public Dictionary<string, bool> animationTriggerStates = new Dictionary<string, bool>();
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
    }

    public void SetActiveQuest(int questId)
    {
        Quest newQuest = GetQuestById(questId);
        if (newQuest != null && !newQuest.isCompleted && !newQuest.isActive)
        {
            newQuest.isActive = true;
            activeQuests.Add(newQuest); // Добавляем квест в список
            newQuest.Activate();
            Debug.Log($"Квест активирован: {newQuest.title}");
        }
        else if (newQuest != null && newQuest.isCompleted)
        {
            Debug.LogWarning($"Квест с ID {questId} уже завершен.");
        }
        else
        {
            Debug.LogError($"Квест с ID {questId} не найден или уже активен!");
        }
    }

    public Quest GetQuestById(int questId)
    {
        foreach (Quest quest in allQuests)
        {
            if (quest.id == questId)
            {
                return quest;
            }
        }
        return null;
    }

    /// <summary>
    /// Завершает квест по его ID.
    /// </summary>
    public void CompleteQuest(int questId)
    {
        Quest questToComplete = GetQuestById(questId);
        if (questToComplete != null)
        {
            questToComplete.isCompleted = true;
            questToComplete.isActive = false;
            activeQuests.Remove(questToComplete); // Удаляем квест из списка активных
            completedQuests.Add(questToComplete);
            Debug.Log($"Квест '{questToComplete.title}' завершен.");
        }
        else
        {
            Debug.LogWarning($"Квест с ID {questId} не найден.");
        }
    }

    // Остальные методы остаются без изменений
    public void AddCollectedObject(string objectId)
    {
        if (!IsObjectCollected(objectId))
        {
            collectedObjects.Add(new CollectedObject { objectId = objectId });
        }
    }

    public bool IsObjectCollected(string objectId)
    {
        return collectedObjects.Exists(obj => obj.objectId == objectId);
    }

    public void SaveDialogueState(string triggerId, int stateIndex)
    {
        if (dialogueTriggerStates.ContainsKey(triggerId))
        {
            dialogueTriggerStates[triggerId] = stateIndex;
        }
        else
        {
            dialogueTriggerStates.Add(triggerId, stateIndex);
        }
    }

    public int GetDialogueState(string triggerId)
    {
        if (dialogueTriggerStates.ContainsKey(triggerId))
        {
            return dialogueTriggerStates[triggerId];
        }
        return 0;
    }

    public void SaveQuestState(string triggerId, bool state)
    {
        if (questTriggerStates.ContainsKey(triggerId))
        {
            questTriggerStates[triggerId] = state;
        }
        else
        {
            questTriggerStates.Add(triggerId, state);
        }
    }

    public bool GetQuestState(string triggerId)
    {
        if (questTriggerStates.ContainsKey(triggerId))
        {
            return questTriggerStates[triggerId];
        }
        return true;
    }

    /// <summary>
    /// Сохраняет состояние анимационного триггера.
    /// </summary>
    public void SaveAnimationState(string triggerId, bool hasTriggered)
    {
        if (animationTriggerStates.ContainsKey(triggerId))
        {
            animationTriggerStates[triggerId] = hasTriggered;
        }
        else
        {
            animationTriggerStates.Add(triggerId, hasTriggered);
        }
    }

    /// <summary>
    /// Получает сохраненное состояние анимационного триггера.
    /// </summary>
    public bool GetAnimationState(string triggerId)
    {
        if (animationTriggerStates.ContainsKey(triggerId))
        {
            return animationTriggerStates[triggerId];
        }
        return false; // По умолчанию анимация не была запущена
    }
}