using UnityEngine;

public class QuestChecker : MonoBehaviour
{
    // ID ������, ������� ��������� ��� ������
    public int requiredQuestId;

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
        Debug.Log("QuestChecker: �������� �������� �������� ������...");

        if (QuestManager.instance == null)
        {
            Debug.LogError("QuestChecker: QuestManager �� ������ � �����!");
            return;
        }

        Quest currentQuest = QuestManager.instance.activeQuest;

        // ���� ��������� ������ ���, ��� ��� ID ������ ����������
        if (currentQuest == null || currentQuest.id < requiredQuestId)
        {
            Debug.Log($"QuestChecker: �������! ������� ����� (ID: {(currentQuest != null ? currentQuest.id : 0)}) ������ ���������� (ID: {requiredQuestId}).");

            SetObjectsActive(OnSuccess, false);
            SetObjectsActive(OnFailure, true);
        }
        // ���� ID ��������� ������ ����� ��� ������ ����������
        else
        {
            Debug.Log($"QuestChecker: �����! ������� ����� (ID: {currentQuest.id}) ������������� ��� ��������� ��������� (ID: {requiredQuestId}).");

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