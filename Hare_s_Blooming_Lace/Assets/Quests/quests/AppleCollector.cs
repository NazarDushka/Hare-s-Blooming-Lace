using UnityEngine;

public class AppleCollector : MonoBehaviour
{
    // ���������� ID ��� ����� ������ �� �����
    // ���� ID ������ ���� � ������������� �������, ��� ��� ��� �� ����� ����������
    public string uniqueId;

    public static int applesCount = 0;
    private bool playerInRange = false;

    // ����� Awake ���������� ��� �������� �������
    private void Awake()
    {
        if (QuestManager.instance != null && QuestManager.instance.IsObjectCollected(uniqueId))
        {
            // ���� ������ ��� �������, ���������� ������������ ������
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject); // ���� �������� ���, ���������� ����
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            applesCount++;
            Debug.Log($"������ �������! ����� �����: {applesCount}");

            // ��������� ���������� � ���, ��� ������ �������
            if (QuestManager.instance != null)
            {
                QuestManager.instance.AddCollectedObject(uniqueId);
            }

            // ���������� ������������ ������
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                // ���� � ������� ��� ��������, ���������� ��� ������
                Destroy(gameObject);
            }
        }
    }
}