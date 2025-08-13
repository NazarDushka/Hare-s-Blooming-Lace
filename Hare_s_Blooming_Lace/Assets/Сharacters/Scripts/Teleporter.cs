using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [Header("��������� ������������")]
    public float deathYPosition = -10f;

    // �����, ���� ��������������� ��������
    public Vector3 teleportTarget = new Vector3(0, 5, 0);

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // ���������, ��������� �� �������� ���� ����� ������
        if (transform.position.y < deathYPosition)
        {
            // ���������� ��������� � ��������� �����
            transform.position = teleportTarget;

            // ���� Rigidbody ����������, ���������� ��� ��������, ����� �������� �� ��������� ������
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}