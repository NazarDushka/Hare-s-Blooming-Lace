using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationLoader : MonoBehaviour
{
    private static string loadingSceneName = "LoadingScene";
    private static string targetSceneName;
    public float minLoadTime = 3f;

    // Новые статические переменные для телепортации
    public static bool shouldTeleport = false;
    public static Vector2 teleportPosition;

    public static void Load(string sceneName, bool teleport, Vector2 position)
    {
        if (SceneUtility.GetBuildIndexByScenePath(sceneName) == -1)
        {
            Debug.LogError("Сцена '" + sceneName + "' не найдена в Build Settings! Пожалуйста, добавьте её.");
            return;
        }

        targetSceneName = sceneName;
        shouldTeleport = teleport;
        teleportPosition = position;

        SceneManager.LoadScene(loadingSceneName);
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            StartCoroutine(LoadSceneAsync());
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        float startTime = Time.time;
        AsyncOperation gameSceneOperation = SceneManager.LoadSceneAsync(targetSceneName);
        gameSceneOperation.allowSceneActivation = false;

        while (!gameSceneOperation.isDone)
        {
            float progress = Mathf.Clamp01(gameSceneOperation.progress / 0.9f);
            Debug.Log("Прогресс загрузки: " + (progress * 100) + "%");
            yield return null;

            if (gameSceneOperation.progress >= 0.9f)
            {
                if (Time.time - startTime >= minLoadTime)
                {
                    gameSceneOperation.allowSceneActivation = true;
                }
            }
        }
    }
}