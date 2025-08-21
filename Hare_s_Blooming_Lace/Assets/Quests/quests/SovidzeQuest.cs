using UnityEngine;

public class SovidzeQuest : MonoBehaviour
{
    // ���������� ID ��� ���������� ��������� ������� ��������������
    public string uniqueId = "SovaFirstContact";

    [Tooltip("ID ������, ������� ����� ������������.")]
    public int questIdToActivate = 3;

    public Animator animator;

    [Tooltip("�������� �������� ���������.")]
    public string appearingAnimName = "Appearing";

    [Tooltip("�������� �������� �����������.")]
    public string idleAnimName = "Idle";

    private bool playerInRange = false;
    private bool isFirstInteraction;
    private bool hasAppeared = false;


    private void Awake()
    {
        if (animator == null)
        {
            Debug.LogError("Animator �� �������� � ����������. ����������, ��������� ���!");
            return;
        }

        if (QuestManager.instance != null)
        {
            // ��������� ��������� ������� ��������������
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
            Debug.Log("����� ����� � ���� �������������� � �����.");

            if (animator != null)
            {
                // ��������� �������� ���������
                animator.Play(appearingAnimName);

                // ������������� ����, ����� �������� ��������� ������ ���� ���
                hasAppeared = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("����� ����� �� ���� �������������� � �����.");
        }
    }

    private void Update()
    {
        // ���������, ��������� �� ����� � ���� � ����� �� ������� E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (QuestManager.instance == null)
            {
                Debug.LogError("QuestManager �� ������ � �����!");
                return;
            }

            if (isFirstInteraction)
            {
                // ���� ��� ������ ��������������, ���������� �����
                QuestManager.instance.SetActiveQuest(questIdToActivate);
                Debug.Log($"����������� ����� � ID {questIdToActivate}.");

                // ��������� ���������, ����� �������� ��������� ���������
                isFirstInteraction = false;
                QuestManager.instance.SaveQuestState(uniqueId, isFirstInteraction);
            }
            // ����� �������� else-���� ��� �������, ���� ����� ��� �����������
        }
    }
}
