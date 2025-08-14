using System.Collections;
using UnityEngine;

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
                LocationLoader.Load(loadingSceneName, targetSceneName, shouldTeleport, targetPosition);
            }
        }
    }

    private IEnumerator LoadSceneAfterAnimation()
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