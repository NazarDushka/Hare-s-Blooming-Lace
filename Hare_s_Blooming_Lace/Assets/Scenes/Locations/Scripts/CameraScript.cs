using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("��������� ������")]
    public Transform Player;
    public float smoothSpeed = 1.5f;
    public float teleportDistance = 20f; // ��������� ��� ������������

    [Header("����������� �����")]
    public float minX;
    public float maxX;

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
            // ������� ����� ������� ��� ������
            Vector3 targetPosition = new Vector3(Player.position.x, yOffset, zOffset);

            // --- ����� ������ ---
            // ���������, ���� ���������� ������� �������
            if (Vector3.Distance(transform.position, targetPosition) > teleportDistance)
            {
                // ��������� ���������� ������ � ������
                transform.position = targetPosition;
            }
            else
            {
                // ����� ���������� ������� ��������
                Vector3 clampedTargetPosition = new Vector3(
                    Mathf.Clamp(targetPosition.x, minX, maxX),
                    yOffset,
                    zOffset
                );

                Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedTargetPosition, smoothSpeed * Time.fixedDeltaTime);
                transform.position = smoothedPosition;
            }
        }
        else
        {
            Debug.LogWarning("Player Transform is not assigned in CameraScript.");
        }
    }
}