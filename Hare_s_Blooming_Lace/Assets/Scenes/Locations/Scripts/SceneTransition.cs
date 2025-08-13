using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [Header("��������� ��������")]
    public string targetSceneName;

    // ��������� ����� ���������� ��� ���������
    public bool shouldTeleport = false;
    public Vector2 targetPosition;

    private bool playerInTrigger = false;

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            // ������ �� �������� ���������� ������ � ��������� �����
            LocationLoader.Load(targetSceneName, shouldTeleport, targetPosition);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            Debug.Log("������� E, ����� ������� �� ��������� �������.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}