using UnityEngine;

public class HintScript : MonoBehaviour
{
    public GameObject GameObject;

    void Start()
    {

        GameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ���������, ��� ����� ������ ����� �� ���� "Player"
        if (other.CompareTag("Player"))
        {
            // �������� ���� ������� ������
            GameObject.SetActive(true);
            Debug.Log("��������� ��������.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // ���������, ��� ����� ������ �����
        if (other.CompareTag("Player"))
        {
            // ��������� ���� ������� ������
            GameObject.SetActive(false);
            Debug.Log("��������� ���������.");
        }
    }
}