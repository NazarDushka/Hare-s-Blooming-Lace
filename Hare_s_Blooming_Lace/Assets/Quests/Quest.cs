using UnityEngine;

[System.Serializable]
public class Quest
{
    public int id;
    public string title;
    [TextArea(3, 10)]
    public string description;

    public bool isActive = false;
    public bool isCompleted = false;

    // ��� �����, �� ������� ������ ���� ������� �����
    public string SceneName;
    // �������, ���� ������ ���� ��������� ����� ��� ��������� ������
    public Vector2 TargetPosition;

    public void Activate()
    {
        isActive = true;
    }

    public void Complete()
    {
        isCompleted = true;
        isActive = false;
        // ��� ����� �������� ������, ��������, ������ �������
    }
}