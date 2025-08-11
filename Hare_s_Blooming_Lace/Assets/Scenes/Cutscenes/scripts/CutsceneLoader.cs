using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Cutscene Settings")]
    [Tooltip("Animator, ����������� ���������. �������� �������� ����� � ����������.")]
    public Animator cutsceneAnimator;
    [Tooltip("��� ��������� ��������, ������� ������������� � ���������� ��������.")]
    public string cutsceneEndState = "CutsceneEnd";

    [Header("Loading Screen Settings")]
    [Tooltip("Animator, ����������� ��������� ������������.")]
    public Animator loadingAnimator;
    [Tooltip("��� ��������, ������� ����� ��������� ����� ���������.")]
    public string disappearAnimationName = "gamenameDisappear";
    [Tooltip("������������� �������� � ��������.")]
    public float artificialLatency = 1.0f;

    private float animationDuration;
    private AsyncOperation asyncOperation;

    void Start()
    {
        // ���������, ��� ��������� ���������
        if (cutsceneAnimator == null || loadingAnimator == null)
        {
            Debug.LogError("Animators are not assigned! Please assign them in the Inspector.");
            return;
        }

        // �������� ������������ �������� ������������
        AnimationClip[] clips = loadingAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == disappearAnimationName)
            {
                animationDuration = clip.length;
                break;
            }
        }

        StartCoroutine(WaitForCutsceneAndLoad());
    }

    IEnumerator WaitForCutsceneAndLoad()
    {
        // ���, ���� �������� ����������
        // ���������, ��� �������� ��������� � ��������� cutsceneEndState
        while (!cutsceneAnimator.GetCurrentAnimatorStateInfo(0).IsName(cutsceneEndState))
        {
            yield return null;
        }

        // �������� ����������� �������� ��������� �����
        asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncOperation.allowSceneActivation = false;

        float startTime = Time.realtimeSinceStartup;

        // ���, ���� ����� ����� ���������� � ������� ������������� ��������
        while (asyncOperation.progress < 0.9f || (Time.realtimeSinceStartup - startTime < artificialLatency))
        {
            yield return null;
        }

        // ������������� �������� ������������ � ���� � ����������
        loadingAnimator.Play(disappearAnimationName);
        yield return new WaitForSeconds(animationDuration);

        // ���������� ��������� �����
        asyncOperation.allowSceneActivation = true;
    }
}
