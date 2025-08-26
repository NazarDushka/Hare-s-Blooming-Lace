using UnityEngine;

public class SceneDataCarrier : MonoBehaviour
{
    // ����������� ���������, ��������� �� ������ �����
    public static SceneDataCarrier Instance;

    // ����������, ������� �� ����� ��������
    public bool isWhiteIn = false;
    public bool isPlayedBeforeSovidze = false;
    public bool isPlayedBeforeEjidze = false;


    private void Awake()
    {
        // ���� ��������� ��� �� ����������, �� ��������� ����
        if (Instance == null)
        {
            Instance = this;
            // ��������� ���� ������ ��� �������� ����� �����
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���� ��������� ��� ����, ���������� ���� ��������
            Destroy(gameObject);
        }
    }
}