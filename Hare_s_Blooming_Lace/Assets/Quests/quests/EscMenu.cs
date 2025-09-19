using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EscMenu : MonoBehaviour
{
    public static bool isEscMenuOpened = false;

    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public PlayerController playerMovement;
    public Animator animator;

    private KeyCode escKey = KeyCode.Escape;

    // —сылка на слайдер громкости
    private Slider volumeMusicSlider;
    private Slider volumeSoundSlider;

    private void Start()
    {
        // »щем слайдер при старте, чтобы не делать это посто€нно
        if (musicManager.instance != null && musicManager.instance.volumeMusicSlider != null)
        {
            volumeMusicSlider = musicManager.instance.volumeMusicSlider;
        }
        if (soundManager.instance != null && soundManager.instance.volumeSoundSlider != null)
        {
        volumeSoundSlider = soundManager.instance.volumeSoundSlider;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(escKey))
        {
            if (animator != null)
            {
                if (isEscMenuOpened)
                {
                    CloseMenu();
                }
                else
                {
                    OpenMenu();
                }
            }
        }
    }

    public void OpenMenu()
    {
        isEscMenuOpened = true;
        animator.SetTrigger("EscMenuOn");
        Cursor.visible = true;

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (volumeMusicSlider != null)
        {
            volumeMusicSlider.interactable = true; // ¬ключаем слайдер
        }
        if (volumeSoundSlider != null)
        {
            volumeSoundSlider.interactable = true; // ¬ключаем слайдер
        }
        else
        {
            Debug.LogWarning("Volume slider not found in EscMenu.cs");
        }
        // Null check before disabling player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            playerMovement.maxSpeed = 0;
            playerMovement.jumpForce = 0;
        }
        else
        {
            Debug.LogWarning("PlayerController reference is null when opening EscMenu. Cannot disable player movement.");
        }

        // --- Get the latest active quest ---
        Quest latestQuest = GetLatestActiveQuest();

        if (latestQuest != null)
        {
            Title.text = latestQuest.title;
            Description.text = latestQuest.description;
        }
        else
        {
            // If no active quests, show default text
            Title.text = "No Active Quests";
            Description.text = "Check your quest log later.";
        }
    }

    public void CloseMenu()
    {
        if (animator != null && isEscMenuOpened)
        {
            isEscMenuOpened = false;
            Cursor.visible = false;

            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }

            if (volumeMusicSlider != null)
            {
                volumeMusicSlider.interactable = false; // ќтключаем слайдер
            }
            if (volumeSoundSlider != null)
            {
                volumeSoundSlider.interactable = false; // ќтключаем слайдер
            }
            
            if (playerMovement != null)
            {
                playerMovement.enabled = true;
                playerMovement.maxSpeed = 3.8f;
                playerMovement.jumpForce = 10;
            }

            else 
            {
                Debug.LogWarning("PlayerController reference is null when trying to close EscMenu. Player might have been destroyed or is inaccessible.");
            }

            animator.SetTrigger("EscMenuOff");

        }

    }

    /// <summary>
    /// Finds and returns the active quest with the highest index.
    /// </summary>
    /// <returns>The latest active quest, or null if none exist.</returns>
    private Quest GetLatestActiveQuest()
    {
        // First, perform a null and count check for safety
        if (QuestManager.instance == null || QuestManager.instance.activeQuests == null || QuestManager.instance.activeQuests.Count == 0)
        {
            return null; // Return null if there are no active quests
        }

        // Return the last quest in the list. The last element is at index (Count - 1).
        return QuestManager.instance.activeQuests[QuestManager.instance.activeQuests.Count - 1];
    }
}