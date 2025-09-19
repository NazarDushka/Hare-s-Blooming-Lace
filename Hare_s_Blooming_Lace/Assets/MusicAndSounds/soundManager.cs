using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class soundManager : MonoBehaviour
{
    public static soundManager instance;

    // A dedicated AudioSource for looping background/walk sounds
    public AudioSource loopSoundSource;
    // A dedicated AudioSource for one-shot sound effects
    private AudioSource audioSource;

    public float currentVolume = 0.2f;

    [Header("Звуки ходьбы")]
    public AudioClip walkSound;
    public AudioClip homeWalkSound;
    public AudioClip caveWalkSound;

    [Header("Звуки сбора")]
    public AudioClip collectSoundClip;

    private PlayerController playerController;
    private bool isPlayingWalkSound = false;

    public Slider volumeSoundSlider;

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

            if (volumeSoundSlider == null)
            {
                volumeSoundSlider = GameObject.Find("SoundSlider")?.GetComponent<Slider>();
            }

            if (volumeSoundSlider != null)
            {
                // Загружаем сохраненную громкость или используем значение по умолчанию
                currentVolume = PlayerPrefs.GetFloat("SoundVolume", 0.2f);
                volumeSoundSlider.value = currentVolume; // Устанавливаем значение слайдера
                SetVolume(currentVolume); // Применяем громкость

                // Привязываем слушатель
                volumeSoundSlider.onValueChanged.AddListener(SetVolume);
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
        if (volumeSoundSlider != null)
        {
            volumeSoundSlider.onValueChanged.RemoveListener(SetVolume);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
        }
        else
        {
            playerController = null;
            Debug.LogWarning("Объект игрока не найден в новой сцене.");
        }

        // Find and set up the UI slider
        if (volumeSoundSlider == null)
        {
            volumeSoundSlider = GameObject.Find("SoundSlider")?.GetComponent<Slider>();
        }
        if (volumeSoundSlider != null)
        {
            volumeSoundSlider.onValueChanged.RemoveAllListeners(); // Prevent duplicate listeners
            volumeSoundSlider.onValueChanged.AddListener(SetVolume);
            volumeSoundSlider.value = currentVolume;
            SetVolume(currentVolume);
        }
    }

    public void SetVolume(float volume)
    {
        currentVolume = volume;
        if (loopSoundSource != null)
        {
            loopSoundSource.volume = currentVolume;
        }
        if (audioSource != null)
        {
            audioSource.volume = currentVolume;
        }
        PlayerPrefs.SetFloat("SoundVolume", currentVolume);
    }

    private void Update()
    {
        if (playerController == null || loopSoundSource == null)
        {
            return;
        }
        if (volumeSoundSlider == null)
        {
            volumeSoundSlider = GameObject.Find("SoundSlider")?.GetComponent<Slider>();
        }
        bool isPlayerMoving = playerController.currentSpeed != 0;
        bool isPlayerMovingOnGround = playerController.isGrounded;

        if (isPlayerMoving && !isPlayingWalkSound && isPlayerMovingOnGround)
        {
            PlayWalkSound();
            isPlayingWalkSound = true;
        }
        else if ((!isPlayerMoving || !isPlayerMovingOnGround) && isPlayingWalkSound)
        {
            loopSoundSource.Stop();
            isPlayingWalkSound = false;
        }
    }

    private void PlayWalkSound()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        AudioClip currentWalkClip = null;

        switch (sceneName)
        {
            case "Village":
                currentWalkClip = walkSound;
                break;
            case "House":
                currentWalkClip = homeWalkSound;
                break;
            case "Wolf's cave":
                currentWalkClip = caveWalkSound;
                break;
            default:
                currentWalkClip = walkSound;
                break;
        }

        if (currentWalkClip != null)
        {
            loopSoundSource.clip = currentWalkClip;
            loopSoundSource.loop = true;
            loopSoundSource.Play();
        }
    }

    public void PlayCollectSound()
    {
        if (collectSoundClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(collectSoundClip);
        }
        else
        {
            Debug.LogWarning("SoundManager: Collect sound clip or SFX AudioSource is missing!");
        }
    }
}