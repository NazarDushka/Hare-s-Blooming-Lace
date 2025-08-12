using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Cutscene Settings")]
    [Tooltip("Animator, управляющий катсценой. Загрузка начнется после её завершения.")]
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

    private float animationDuration;
    private AsyncOperation asyncOperation;

    void Start()
    {
        // Проверяем, что аниматоры назначены
        if (cutsceneAnimator == null || loadingAnimator == null)
        {
            Debug.LogError("Animators are not assigned! Please assign them in the Inspector.");
            return;
        }

        // Получаем длительность анимации исчезновения
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
        // Ждём, пока катсцена завершится
        // Проверяем, что аниматор находится в состоянии cutsceneEndState
        while (!cutsceneAnimator.GetCurrentAnimatorStateInfo(0).IsName(cutsceneEndState))
        {
            yield return null;
        }

        // Начинаем асинхронную загрузку следующей сцены
        asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncOperation.allowSceneActivation = false;

        float startTime = Time.realtimeSinceStartup;

        // Ждём, пока сцена почти загрузится И пройдет искусственная задержка
        while (asyncOperation.progress < 0.9f || (Time.realtimeSinceStartup - startTime < artificialLatency))
        {
            yield return null;
        }

        // Воспроизводим анимацию исчезновения и ждем её завершения
        loadingAnimator.Play(disappearAnimationName);
        yield return new WaitForSeconds(animationDuration);

        // Активируем следующую сцену
        asyncOperation.allowSceneActivation = true;
    }
}
