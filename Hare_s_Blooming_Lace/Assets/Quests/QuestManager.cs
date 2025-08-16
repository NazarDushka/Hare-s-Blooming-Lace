using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public List<Quest> allQuests;
    public Quest activeQuest;

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

        if (newQuest != null && !newQuest.isCompleted)
        {
            // ƒеактивируем предыдущий квест, если он есть
            if (activeQuest != null)
            {
                activeQuest.isActive = false;
            }

            activeQuest = newQuest;
            activeQuest.Activate();
            Debug.Log($" вест активирован: {activeQuest.title}");
        }
        else if (newQuest != null && newQuest.isCompleted)
        {
            Debug.LogWarning($" вест с ID {questId} уже завершен.");
        }
        else
        {
            Debug.LogError($" вест с ID {questId} не найден!");
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
}