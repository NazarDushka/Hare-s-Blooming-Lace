using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    // ���� ����� ����� ���������� �����, ��� ������ �������� �������� � �����
    void Start()
    {
        // ���������, ����� �� ��� �����������������
        if (LocationLoader.shouldTeleport)
        {
            // ���������� ��������� � ������ �����
            transform.position = LocationLoader.teleportPosition;

            // ���������� ����, ����� ��� ��������� �������� �� ���� ������������
            LocationLoader.shouldTeleport = false;
        }
    }
}