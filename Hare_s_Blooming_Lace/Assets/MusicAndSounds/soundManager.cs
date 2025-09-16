using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class soundManager : MonoBehaviour
{
    public static soundManager instance;

    public AudioSource soundSource;
    public float volumeSound = 0.2f;

    [Header("����� ������")]
    public AudioClip walkSound;
    public AudioClip homeWalkSound;
    public AudioClip caveWalkSound;

    [Header("����� �����")]
    public AudioClip collectSoundClip;

    private PlayerController playerController;
    private bool isPlayingWalkSound = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (soundSource == null)
        {
            soundSource = GetComponent<AudioSource>();
            if (soundSource == null)
            {
                Debug.LogError("��������� AudioSource �� ������ �� ������� SoundManager. ����������, �������� ���.");
                return;
            }
        }
        SetVolume(volumeSound);
        // ��������� ���������, ������� ����� �������� ����� OnSceneLoaded ��� �������� ����� �����
        SceneManager.sceneLoaded += OnSceneLoaded;
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
            Debug.LogWarning("������ ������ �� ������ � ����� �����.");
        }
    }

    public void SetVolume(float volume)
    {
        volumeSound = volume;
        if (soundSource != null)
        {
            soundSource.volume = volumeSound;
            PlayerPrefs.SetFloat("SoundVolume", volumeSound);
        }
    }

    private void Update()
    {
        if (playerController == null || soundSource == null)
        {
            return;
        }

        bool isPlayerMoving = playerController.currentSpeed != 0;
        bool isPlayerMovingOnGround = playerController.isGrounded;

        if (isPlayerMoving && !isPlayingWalkSound && isPlayerMovingOnGround)
        {
            PlayWalkSound();
            isPlayingWalkSound = true;
        }

        else if ((!isPlayerMoving || !isPlayerMovingOnGround) && isPlayingWalkSound )
        {
            soundSource.Stop();
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
            soundSource.clip = currentWalkClip;
            soundSource.loop = true; 
            soundSource.Play();
        }
    }

    public void PlayCollectSound()
    {
        if (collectSoundClip != null && soundSource != null)
        {
            soundSource.PlayOneShot(collectSoundClip);
        }
        else
        {
            Debug.LogWarning("SoundManager: Collect sound clip or AudioSource is missing!");
        }
    }
}