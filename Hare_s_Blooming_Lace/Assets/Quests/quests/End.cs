using UnityEngine;
using System.Collections; // You need this for coroutines

public class End : MonoBehaviour
{

    // Use a public variable to set the name of your cutscene scene in the Inspector.
    public string cutsceneSceneName = "The End";
    public Animator transitionAnimator; // Animator for transition effects
    // Start is a good place to initialize things, but Awake works too.
    private bool playerInRange=false;
    private void Start()
    {
        // Get the DialogueManager instance to ensure it's not null.
        if (DialogueManager.instance == null)
        {
            Debug.LogError("DialogueManager not found. Make sure it is in the scene.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered the trigger area.");
        }
    }
    private void Update()
    {
        if (playerInRange)
        {
         
            if (DialogueManager.instance != null && !DialogueManager.instance.isDialogueActive)
            {

                StartCoroutine(LoadCutscene());
            }
        }
    }

    private IEnumerator LoadCutscene()
    {
        transitionAnimator.SetTrigger("WhiteIn");
        yield return new WaitForSeconds(2.5f); // Wait for the animation to finish

        Debug.Log("Dialogue ended. Loading cutscene: " + cutsceneSceneName);

        LocationLoader.Load(null, cutsceneSceneName, false, Vector2.zero);

        yield return null;
    }
}