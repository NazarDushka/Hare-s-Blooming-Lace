using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Cutscene Settings")]
    [Tooltip("Animator, управляющий катсценой.")]
    public Animator cutsceneAnimator;
    [Tooltip("Имя состояния анимации, которое сигнализирует о завершении катсцены.")]
    public string cutsceneEndState = "CutsceneEnd";

    [Header("Loading Screen Settings")]
    [Tooltip("Animator, управляющий анимацией исчезновения.")]
    public Animator loadingAnimator;
    [Tooltip("Имя анимации, которая будет проиграна перед переходом.")]
    public string disappearAnimationName = "gamenameDisappear";
    [Tooltip("Искусственная задержка в секундах.")]
    public float artificialLatency = 1.0f;

    [Tooltip("Сцена на которую переходим")]
    public string TargetScene;

    // ✅ Добавлена новая переменная, которую будем передавать
    public bool sendWhiteIn = false;

    private float animationDuration;
    private AsyncOperation asyncOperation;

    void Start()
    {
        if (cutsceneAnimator == null || loadingAnimator == null)
        {
            Debug.LogError("Animators are not assigned!");
            return;
        }

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
        while (!cutsceneAnimator.GetCurrentAnimatorStateInfo(0).IsName(cutsceneEndState))
        {
            yield return null;
        }

        
        if (SceneDataCarrier.Instance != null)
        {
            SceneDataCarrier.Instance.isWhiteIn = sendWhiteIn;
        }

        asyncOperation = SceneManager.LoadSceneAsync(TargetScene);
        asyncOperation.allowSceneActivation = false;

        float startTime = Time.realtimeSinceStartup;

        while (asyncOperation.progress < 0.9f || (Time.realtimeSinceStartup - startTime < artificialLatency))
        {
            yield return null;
        }

        loadingAnimator.Play(disappearAnimationName);
        yield return new WaitForSeconds(animationDuration);

        asyncOperation.allowSceneActivation = true;
    }
}