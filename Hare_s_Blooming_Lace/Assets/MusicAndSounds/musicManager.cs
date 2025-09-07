using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public Slider volumeMusicSlider;

    public float fadeDuration = 0.5f;
    private Coroutine fadeCoroutine;


    private float currentVolume = 0.2f; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    Debug.LogError("AudioSource component not found on musicManager GameObject. Please add one.");
                }
            }

            if (volumeMusicSlider == null)
            {
                volumeMusicSlider = GameObject.Find("MusicSlider")?.GetComponent<Slider>();
            }

            if (volumeMusicSlider != null)
            {
                // Загружаем сохраненную громкость или используем значение по умолчанию
                currentVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
                volumeMusicSlider.value = currentVolume; // Устанавливаем значение слайдера
                SetVolume(currentVolume); // Применяем громкость

                // Привязываем слушатель
                volumeMusicSlider.onValueChanged.AddListener(SetVolume);
            }
            else
            {
                Debug.LogWarning("VolumeMusicSlider not found. Volume control will not work.");
            }
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
        if (volumeMusicSlider != null)
        {
            volumeMusicSlider.onValueChanged.RemoveListener(SetVolume);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string currentSceneName = scene.name;

        // Повторно находим слайдер, если он был уничтожен или пересоздан
        if (volumeMusicSlider == null)
        {
            volumeMusicSlider = GameObject.Find("MusicSlider")?.GetComponent<Slider>();
            if (volumeMusicSlider != null)
            {
                // Повторно привязываем слушателя, если слайдер был найден
                volumeMusicSlider.onValueChanged.AddListener(SetVolume);
                volumeMusicSlider.value = currentVolume;
                // Применяем сохраненную громкость
                SetVolume(currentVolume);
            }
        }
        PlayMusicForLocation(currentSceneName);
    }

    // Этот метод теперь просто устанавливает громкость и сохраняет ее
    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            currentVolume = volume; // Обновляем текущую громкость
            audioSource.volume = currentVolume;
            // Сохраняем громкость для последующего использования (например, между сессиями игры)
            PlayerPrefs.SetFloat("MusicVolume", currentVolume);
        }
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

    public void PlayMusicForLocation(string locationName)
    {
        LocationMusic playlist = locationMusics.FirstOrDefault(p => p.locationName == locationName);

        if (playlist != null && playlist.musicTracks.Count > 0)
        {
            int randomIndex = Random.Range(0, playlist.musicTracks.Count);
            AudioClip nextClip = playlist.musicTracks[randomIndex];

            if (audioSource.clip == nextClip && audioSource.isPlaying)
            {
                return;
            }

            if (!audioSource.isPlaying)
            {
                audioSource.clip = nextClip;
                audioSource.volume = currentVolume; // Применяем текущую громкость
                audioSource.Play();
                return;
            }

            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(FadeMusic(nextClip));
        }
        else
        {
            audioSource.Stop();
            Debug.LogWarning("No music playlist found for location: " + locationName);
        }
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();
        timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, currentVolume, timer / fadeDuration); // Используем currentVolume
            yield return null;
        }

        audioSource.volume = currentVolume; // Устанавливаем окончательную громкость
    }
}