using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("��������� ������")]
    public Transform Player; // ����, �� ������� ����� ������� ������
    public float smoothSpeed = 1.5f; // �������� ����������� �������� ������

    [Header("����������� �����")]
    public float minX; // ����������� ������� ������ �� X
    public float maxX; // ������������ ������� ������ �� X

    private float yOffset;
    private float zOffset;

    void Start()
    {
        yOffset = transform.position.y;
        zOffset = transform.position.z;
    }

    void FixedUpdate()
    {
        if (Player != null)
        {
            // ������� ����� ������� ��� ������, ��������� X ������
            Vector3 targetPosition = new Vector3(Player.position.x, yOffset, zOffset);

            // ������������ X-����������, ����� ��� �� �������� �� ������� minX � maxX
            float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);

            // ��������� ������������ X-���������� � ������� �������
            targetPosition = new Vector3(clampedX, yOffset, zOffset);

            // ���������� �������� ������
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.fixedDeltaTime);

            // ��������� ����� ������� � ������
            transform.position = smoothedPosition;
        }
        else
        {
            Debug.LogWarning("Player Transform is not assigned in CameraScript.");
        }
    }
}