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
            // ������������ ���������� �����, ���� �� ����
            if (activeQuest != null)
            {
                activeQuest.isActive = false;
            }

            activeQuest = newQuest;
            activeQuest.Activate();
            Debug.Log($"����� �����������: {activeQuest.title}");
        }
        else if (newQuest != null && newQuest.isCompleted)
        {
            Debug.LogWarning($"����� � ID {questId} ��� ��������.");
        }
        else
        {
            Debug.LogError($"����� � ID {questId} �� ������!");
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