using UnityEngine;

public class Asksomebodyaboutbrother : MonoBehaviour
{
    // ���������� ID ��� ����� ���������� ��������
    public string uniqueId;

    [Tooltip("ID ������, ������� ����� ���������.")]
    public int questIdToComplete = 0;

    [Tooltip("ID ������, ������� ����� ������������.")]
    public int questIdToActivate = 1;

    private bool playerInRange = false;

    // ����� Awake ���������� ��� �������� �������
    private void Awake()
    {
        // ��������������� ��������� hasActivated �� ��������� �������
        if (QuestManager.instance != null)
        {
            // QuestManager.GetQuestState ���������� false, ���� ��������� ��� ���������
            // isFirstInteraction = QuestManager.instance.GetQuestState(uniqueId);
            // ��� ������ ��������, ������� �� ����� ������������ �� ��������.
            // ���� GetQuestState ������ true (������ �������), ������, �� ��� �� ������������.
            bool hasNotActivated = QuestManager.instance.GetQuestState(uniqueId);
            if (!hasNotActivated)
            {
                // ���� ��������� "��� ������������" (false) ���������, ��������� ������
                this.enabled = false;
            }
        }
    }

    // ����� ����������, ����� ������ ��������� ������ � �������
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("����� ����� � ���� ����� ��������.");
        }
    }

    // ����� ����������, ����� ������ ��������� ������� �� ��������
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("����� ����� �� ���� ����� ��������.");
        }
    }

    private void Update()
    {
        // ���������, ��������� �� ����� � ���� � ����� �� ������
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && QuestManager.instance != null)
        {
            Debug.Log("������ E ������, ���������� �����.");

            // ��������� ����� � ID 0
            QuestManager.instance.CompleteQuest(questIdToComplete);

            // ���������� ����� � ID 1
            QuestManager.instance.SetActiveQuest(questIdToActivate);

            // ��������� ��������� � QuestManager, ����� ����� ������ �� �������������
            QuestManager.instance.SaveQuestState(uniqueId, false);

            // ��������� ���� ������, ����� ������������� ��������� ���������
            this.enabled = false;
        }
        else if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogError("QuestManager �� ������ � �����.");
        }
    }
}