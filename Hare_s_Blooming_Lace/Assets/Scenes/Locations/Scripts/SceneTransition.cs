using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [Header("Настройки перехода")]
    [Tooltip("Куда переносимся")]
    public string targetSceneName;
    [Tooltip("Промежуточная сцена загрузки (оставьте пустым, если не нужна)")]
    public string loadingSceneName;
    [Tooltip("Телепортировать игрока на целевую позицию после загрузки сцены")]
    public bool shouldTeleport = false;
    [Tooltip("Целевая позиция для телепортации")]
    public Vector2 targetPosition;

    [Header("Анимация")]
    [Tooltip("Аниматор для перехода между сценами (оставьте пустым, если не нужна анимация)")]
    public Animator transitionAnimator;
    [Tooltip("Длительность анимации перехода")]
    public float transitionDuration = 2.5f;
    [Tooltip("Имя триггера для анимации 'In'")]
    public string transitionInClipName = "In";

    [Header("Дополнительно")]
    public bool sendWhiteIn = false;
    private bool playerInTrigger = false;

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (transitionAnimator != null)
            {
                // Запускаем анимацию "In" с помощью триггера
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
            Debug.Log("Нажмите E, чтобы перейти на следующую локацию.");
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