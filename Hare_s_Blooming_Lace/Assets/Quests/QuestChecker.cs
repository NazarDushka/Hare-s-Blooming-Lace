using UnityEngine;
using System.Linq;
using System.Collections.Generic; // ��������� ��������� ��� IEnumerable<T>

public class QuestChecker : MonoBehaviour
{
    // ID ������, ������� ��������� ��� ������
    public int requiredQuestId;

    [Tooltip("���� true, ������� ����� '>= requiredQuestId'. ���� false, '<= requiredQuestId'.")]
    public bool ifEqualsOrGreater = true;

    [Tooltip("��������� ������ �������� ������? (���� false, ����� ����������� �����������).")]
    public bool checkActiveQuests = true;

    [Tooltip("��������� ����������� ������?")]
    public bool checkCompletedQuests = false;

    // ������ ��������, ������� ����� ������������ ��� �������� ��������
    public GameObject[] OnSuccess;
    // ������ ��������, ������� ����� ������������ ��� ��������� ��������
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
        Debug.Log("QuestChecker: �������� �������� �������...");

        if (QuestManager.instance == null)
        {
            Debug.LogError("QuestChecker: QuestManager �� ������ � �����!");
            return;
        }

        bool checkSucceeded = false;

        // �������������� questsToCheck ��� ������ ��������� ���� Quest
        IEnumerable<Quest> questsToCheck = new List<Quest>();

        if (checkActiveQuests)
        {
            questsToCheck = questsToCheck.Concat(QuestManager.instance.activeQuests);
        }

        if (checkCompletedQuests)
        {
            // ����� ����� ������, ���� �� �� �������� completedQuests � QuestManager
            questsToCheck = questsToCheck.Concat(QuestManager.instance.completedQuests);
        }

        // ���������, ���� �� ���� �� ���� �����, ��������������� �������
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
            Debug.Log($"QuestChecker: �����! ������ ����� � ID, ��������������� �������.");
            SetObjectsActive(OnSuccess, true);
            SetObjectsActive(OnFailure, false);
        }
        else
        {
            Debug.Log($"QuestChecker: �������! �� ������� �������, ��������������� �������.");
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