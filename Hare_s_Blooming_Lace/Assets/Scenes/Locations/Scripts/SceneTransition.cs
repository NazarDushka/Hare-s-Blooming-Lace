using System.Collections;
using UnityEngine;
using System.Collections.Generic; // ✅ Необходимо для использования HashSet

public class SceneTransition : MonoBehaviour
{
    [Header("Настройки перехода")]
    public string targetSceneName;
    public string loadingSceneName;
    public bool shouldTeleport = false;
    public Vector2 targetPosition;

    [Header("Анимация")]
    public Animator transitionAnimator;
    public float transitionDuration = 2.5f;
    public string transitionInClipName = "In";

    [Header("Дополнительно")]
    public bool sendWhiteIn = false;
    [Tooltip("Если true, сцена загрузки будет проиграна только один раз для этой целевой сцены.")]
    public bool isOncePlayableLoadingScene = false; // ✅ Эта переменная остается

    private bool playerInTrigger = false;

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (SceneDataCarrier.Instance == null)
            {
                // Если SceneDataCarrier не найден, загружаем сцену стандартным способом
                StartCoroutine(LoadAfterAnimation(loadingSceneName));
                return;
            }

            // ✅ Проверяем, должна ли эта загрузка быть одноразовой и уже была ли она проиграна
            if (isOncePlayableLoadingScene && SceneDataCarrier.Instance.scenesWithOncePlayedLoading.Contains(loadingSceneName))
            {
                // Если да, загружаем сцену напрямую
                if (transitionAnimator != null)
                {
                    StartCoroutine(LoadAfterAnimation(null)); // ✅ Передаем null
                }
                else
                {
                    LocationLoader.Load(null, targetSceneName, shouldTeleport, targetPosition);
                }
            }
            else
            {
                // Если это первая загрузка, используем стандартный путь
                if (transitionAnimator != null)
                {
                    StartCoroutine(LoadAfterAnimation(loadingSceneName));
                }
                else
                {
                    LocationLoader.Load(loadingSceneName, targetSceneName, shouldTeleport, targetPosition);
                }
            }
        }
    }

    public IEnumerator LoadAfterAnimation(string loadingScene) // ✅ Метод стал универсальным
    {
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger(transitionInClipName);
            yield return new WaitForSeconds(transitionDuration);
        }

        // ✅ Добавляем имя сцены в HashSet, если загрузка была одноразовой
        if (isOncePlayableLoadingScene)
        {
            SceneDataCarrier.Instance.scenesWithOncePlayedLoading.Add(loadingSceneName);
        }

        LocationLoader.Load(loadingScene, targetSceneName, shouldTeleport, targetPosition);
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