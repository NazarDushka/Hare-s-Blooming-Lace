using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Rendering.CameraUI;

[System.Serializable]
public class LocationMusic
{
    public string locationName;
    public List<AudioClip> musicTracks;
}

public class musicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public static musicManager instance;
    public List<LocationMusic> locationMusics;

    // Fade duration
    public float fadeDuration = 0.5f;

    // Used to prevent multiple fade coroutines from running at once
    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string currentSceneName = scene.name;
        PlayMusicForLocation(currentSceneName);
    }

    public void PlayMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    /// <summary>
    /// Plays a random song from the playlist of the specified location with a fade effect.
    /// </summary>
    /// <param name="locationName">The name of the location (e.g., "Forest", "Town").</param>
    public void PlayMusicForLocation(string locationName)
    {
        LocationMusic playlist = locationMusics.FirstOrDefault(p => p.locationName == locationName);

        if (playlist != null && playlist.musicTracks.Count > 0)
        {
            int randomIndex = Random.Range(0, playlist.musicTracks.Count);
            AudioClip nextClip = playlist.musicTracks[randomIndex];

            // Проверяем, играет ли уже эта песня
            if (audioSource.clip == nextClip && audioSource.isPlaying)
            {
                return;
            }

            // Если никакой музыки не играет, сразу запускаем новый трек
            if (!audioSource.isPlaying)
            {
                audioSource.clip = nextClip;
                audioSource.Play();
                return;
            }

            // Если музыка играет, запускаем плавный переход
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeMusic(nextClip));
        }
        else
        {
            Debug.LogWarning("No music playlist found for location: " + locationName);
        }
    }

    /// <summary>
    /// Coroutine to fade out the current music and fade in the new one.
    /// </summary>
    /// <param name="newClip">The new music clip to play.</param>
    private IEnumerator FadeMusic(AudioClip newClip)
    {
        float currentVolume = audioSource.volume;
        float timer = 0f;

        // Фаза 1: Затухание текущей песни
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(currentVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        // Переключение на новую песню
        audioSource.Stop();
        audioSource.clip = newClip;

        // Фаза 2: Появление новой песни
        audioSource.Play();
        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, currentVolume, timer / fadeDuration);
            yield return null;
        }

        // Убеждаемся, что громкость установлена на конечное значение
        audioSource.volume = currentVolume;
    }

    // Your existing ChangeMusic method
    public void ChangeMusic(AudioClip newClip)
    {
        if (audioSource != null)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}