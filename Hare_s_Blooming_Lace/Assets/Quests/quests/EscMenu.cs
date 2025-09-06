using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class EscMenu : MonoBehaviour
{

    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;

    public PlayerController playerMovement;

    public Animator animator;

    private bool isEscMenuOpened = false;
    private KeyCode escKey = KeyCode.Escape;


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
                    // Open the menu
                    isEscMenuOpened = true;
                    animator.SetTrigger("EscMenuOn");
                    Cursor.visible = true;

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
            }
        }
    }

    public void CloseMenu()
    {
        if (animator != null && isEscMenuOpened)
        {
            isEscMenuOpened = false;
            Cursor.visible = false;

            // Add a null check before accessing playerMovement
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