using System.Collections;
using UnityEngine;

public class WolfsQuest : MonoBehaviour
{
    public GameObject Wolf;
    public DialogueTrigger dialogueTrigger;

    private CameraScript cameraScript;
    private Transform originalCameraTarget;
    private Vector3 originalWolfPosition;
    private DialogueManager dialogueManager;
    private bool questStarted = false;
    public Animator BroAnimator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D called.");
        if (questStarted)
        {
            Debug.Log("Quest has already been started. Ignoring further triggers.");
            return;
        }
        else
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Trigger with Player detected. Starting quest.");
                StartCoroutine(StartLastQuest());
            }
            else
            {
                Debug.Log("Trigger with object tagged: " + other.tag);
            }
            Debug.Log("Quest is starting for the first time.");
        }
        
    }

    private IEnumerator StartLastQuest()
    {
        Debug.Log("StartLastQuest coroutine initiated.");
        cameraScript = Camera.main.GetComponent<CameraScript>();
        if (cameraScript != null)
        {
            Debug.Log("CameraScript reference found.");
            // 1. Store the original camera target and wolf position
            originalCameraTarget = cameraScript.Player;
            originalWolfPosition = Wolf.transform.position;
            Debug.Log("Original camera target and wolf position stored.");

            // 2. Change camera target to the wolf
            cameraScript.Player = Wolf.transform;
            Debug.Log("Camera target switched to the Wolf.");
        }
        else
        {
            Debug.LogError("CameraScript component not found on Main Camera. Camera will not move.");
        }
        

        // Move and flip the wolf
        if (Wolf != null)
        {
            Wolf.transform.position = new Vector3(19.58f, -0.19f, 1f);
            Wolf.transform.localScale = new Vector3(-1f, 1f, 1f);
            Debug.Log("Wolf moved and flipped.");
        }
        else
        {
            Debug.LogError("Wolf GameObject is not assigned in the Inspector.");
        }
        DialogueManager.instance.player = Wolf.transform;

        yield return new WaitForSeconds(2.5f);
        Debug.Log("1-second wait is over.");

        // 3. Trigger the dialogue
        if (dialogueTrigger != null)
        {
            dialogueTrigger.TriggerDialogue();
            Debug.Log("DialogueTrigger called.");
        }
        else
        {
            Debug.LogError("DialogueTrigger is not assigned in the Inspector. Dialogue will not start.");
        }

        questStarted = true;

        // 4. Wait for the dialogue to finish
        yield return StartCoroutine(WaitForDialogueEnd());
        Debug.Log("Dialogue has finished. Resuming quest script.");

        BroAnimator.SetBool("isStanding", true);
        yield return new WaitForSeconds(1.3f);
        // 5. Restore camera and wolf position after dialogue ends
        ReturnToPlayer();
        Debug.Log("Camera and wolf position have been restored.");
    }

    private IEnumerator WaitForDialogueEnd()
    {
        Debug.Log("Waiting for dialogue to end...");
        // A simple way to wait is to check a flag on the DialogueManager
        while (DialogueManager.instance.isDialogueActive)
        {
            yield return null;
        }
        Debug.Log("DialogueManager reports dialogue is no longer active.");
    }

    private void ReturnToPlayer()
    {
        if (cameraScript != null && originalCameraTarget != null)
        {
            cameraScript.Player = originalCameraTarget;
        }
        else
        {
            Debug.LogError("Cannot restore camera target. Either CameraScript or originalCameraTarget is null.");
        }

        if (Wolf != null)
        {
            Wolf.transform.position = originalWolfPosition;
            Wolf.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            Debug.LogError("Cannot restore wolf position. Wolf GameObject is null.");
        }

        DialogueManager.instance.player = GameObject.FindGameObjectWithTag("Player").transform;

    }
}
