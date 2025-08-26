using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScript : MonoBehaviour
{
    [Tooltip("Искусственная задержка в секундах, чтобы экран загрузки отображался дольше.")]
    public float ArteficialLatency = 2.0f; // Default to 2 seconds

    private AsyncOperation asyncOperation;

    void Start()
    {
        Cursor.visible = false;
        // Start the coroutine to asynchronously load the next scene
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        // Start loading the next scene in the build settings
        asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        asyncOperation.allowSceneActivation = false; // Prevent immediate activation

        // Record the time when loading started for artificial latency
        float startTime = Time.realtimeSinceStartup;

        // Keep looping while the scene isn't fully loaded OR the artificial latency hasn't passed
        while (!asyncOperation.isDone || (Time.realtimeSinceStartup - startTime < ArteficialLatency))
        {
            
            if (asyncOperation.progress >= 0.9f && (Time.realtimeSinceStartup - startTime >= ArteficialLatency))
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null; 
        }
    }
}