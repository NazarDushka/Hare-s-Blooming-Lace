using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [Header("��������� ��������")]
    [Tooltip("���� �����������")]
    public string targetSceneName;
    [Tooltip("������������� ����� �������� (�������� ������, ���� �� �����)")]
    public string loadingSceneName;
    [Tooltip("��������������� ������ �� ������� ������� ����� �������� �����")]
    public bool shouldTeleport = false;
    [Tooltip("������� ������� ��� ������������")]
    public Vector2 targetPosition;

    [Header("��������")]
    [Tooltip("�������� ��� �������� ����� ������� (�������� ������, ���� �� ����� ��������)")]
    public Animator transitionAnimator;
    [Tooltip("������������ �������� ��������")]
    public float transitionDuration = 2.5f;
    [Tooltip("��� �������� ��� �������� 'In'")]
    public string transitionInClipName = "In";

    [Header("�������������")]
    public bool sendWhiteIn = false;
    private bool playerInTrigger = false;

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (transitionAnimator != null)
            {
                // ��������� �������� "In" � ������� ��������
                transitionAnimator.SetTrigger(transitionInClipName);
                StartCoroutine(LoadSceneAfterAnimation());
            }
            else
            {
                if (SceneDataCarrier.Instance != null)
                {
                    SceneDataCarrier.Instance.isWhiteIn = sendWhiteIn;
                }
                LocationLoader.Load(loadingSceneName, targetSceneName, shouldTeleport, targetPosition);
            }
        }
    }

    public IEnumerator LoadSceneAfterAnimation()
    {
        yield return new WaitForSeconds(transitionDuration);
        LocationLoader.Load(loadingSceneName, targetSceneName, shouldTeleport, targetPosition);
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