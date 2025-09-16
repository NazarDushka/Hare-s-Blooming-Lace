using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public string LoadingScene;
    public static string NextScene = "Under the tree (start)";
    public bool shouldTeleport = false;
    public Vector2 targetPosition;
    public float animationDuration = 3.0f; // ����������������� �������� ��������

    Animator anim;


    private void Awake()
    {
        Cursor.visible = true;
        anim = GameObject.Find("Canvas").GetComponent<Animator>();
    }


    public void StartGame()
    {
        // ��������� �������� ��������
        anim.SetTrigger("Start");
        QuestManager.instance.SetActiveQuest(0); // ���������� ����� � ID 0
        Cursor.visible = false;
        // ��������� ����� � ���������, ����� �������� ������ �����������
        Invoke("LoadNextScene", animationDuration);
    }

    private void LoadNextScene()
    {
        // �������� ����������� �������� transitionOut (��������� ������ ��������, ��������, ������ ������ ��� ��� �����)
        LocationLoader.Load(LoadingScene, NextScene, shouldTeleport, targetPosition);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}