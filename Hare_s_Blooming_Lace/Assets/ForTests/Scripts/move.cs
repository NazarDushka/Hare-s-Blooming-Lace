// ��� ����������� ����������, ������� ����� ��� ������ � Unity.
using UnityEngine;

// 'public' ��������, ��� ���� ����� ����� � Unity.
// 'MonoBehaviour' - ��� ������� ����� ��� ���� ��������, ������� ����� ����������� � ������� ��������.
public class PlayerMovement : MonoBehaviour
{
    // [SerializeField] ��������� ��� ������ � �������� ��� ���������� � ���� Inspector
    // ���� ���� ��� 'private'. ��� ������� ��������.
    [SerializeField] private float moveSpeed = 5f;

    // ������ �� ��������� Rigidbody2D. �� ����� ������������ ��� ��� ����������� ��������.
    private Rigidbody2D rb;

    // ��� ���������� ����� ������� ����������� ��������.
    private float horizontalInput;

    // Start ���������� ���� ���, ����� ������ ���������� � �����.
    void Start()
    {
        // �������� ��������� Rigidbody2D, ������������� � ����� �� �������� �������.
        // ���� ��� ���, ������ ������ ������.
        rb = GetComponent<Rigidbody2D>();
    }

    // Update ���������� ������ ����.
    void Update()
    {
        // �������� ���� � �������������� ���.
        // ������� 'A' ��� ������� ����� ������ -1.
        // ������� 'D' ��� ������� ������ ������ 1.
        // ������ �� ������ - 0.
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    // FixedUpdate ���������� ����� ������ ���������� �������,
    // ��� ������ ��� ��������� ��� ������ � ������� (Rigidbody).
    void FixedUpdate()
    {
        // ������� ������ ��������, ������� ����� ��������������.
        // ��������� ������� ������������ �������� (rb.linearVelocity.y), ����� �������� �� ���������� ������/�������.
        // *** �����������: ���������� 'linearVelocity' ������ 'velocity'
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // ����������� ���� ����� ������ �������� Rigidbody.
        // *** �����������: ���������� 'linearVelocity' ������ 'velocity'
        rb.linearVelocity = movement;

        // �������� ������� ��� �������� ���������, ����� �� ������� � ������� ��������.
        FlipCharacter();
    }

    // ����� ��� �������� ���������.
    private void FlipCharacter()
    {
        // ���� �������� �������� ������ � �� �� �������� ������.
        if (horizontalInput > 0)
        {
            // ������������ ��� ������. '1' - ��� ���������� �������.
            transform.localScale = new Vector3(1, 1, 1);
        }
        // ���� �������� �������� �����.
        else if (horizontalInput < 0)
        {
            // ������������ ��� �����. '-1' �� X �������������� ������.
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}