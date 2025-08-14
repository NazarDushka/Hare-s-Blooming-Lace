using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationLoader : MonoBehaviour
{
    private static string loadingSceneName;
    private static string targetSceneName;
    public float minLoadTime = 0f;

    public static bool shouldTeleport = false;
    public static Vector2 teleportPosition;

    public static void Load(string loadingScene, string sceneName, bool teleport, Vector2 position)
    {
        if (SceneUtility.GetBuildIndexByScenePath(sceneName) == -1)
        {
            Debug.LogError("Сцена '" + sceneName + "' не найдена в Build Settings! Пожалуйста, добавьте её.");
            return;
        }

        loadingSceneName = loadingScene;
        targetSceneName = sceneName;
        shouldTeleport = teleport;
        teleportPosition = position;

        if (string.IsNullOrEmpty(loadingSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            SceneManager.LoadScene(loadingSceneName);
        }
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(targetSceneName) && SceneManager.GetActiveScene().name == loadingSceneName)
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