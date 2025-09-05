using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EjidzeQuest : MonoBehaviour
{
    public string uniqueId;
    public int questIdToActivate = 2;
    public int questIdToComplete = 2;
    public int requiredApplesCount = 9;
    public int nextQuestId = 4;
    public int questIdToVerify = 3;

    public Animator animator;
    public Animator transitionAnimator; // Аниматор для перехода
    public string happyAnimParam = "IsHappy";

    private bool playerInRange = false;
    private bool isFirstInteraction;
    private bool hasHappyAnimationPlayed;

    public GameObject hint; // Ссылка на объект подсказки
    private DialogueTrigger dialogueTrigger;

    private Vector2 pos;

    private void Awake()
    {
        if (animator == null)
        {
            Debug.LogError("Animator не назначен в инспекторе. Пожалуйста, привяжите его!");
            return;
        }

        if (QuestManager.instance != null)
        {
            isFirstInteraction = QuestManager.instance.GetQuestState(uniqueId);
            Quest completedQuest = QuestManager.instance.GetQuestById(questIdToComplete);
            if (completedQuest != null && completedQuest.isCompleted)
            {
                animator.SetBool(happyAnimParam, true);
                Debug.Log("Квест был завершен ранее. Установка анимации IsHappy.");

                Destroy(hint);
                dialogueTrigger = GetComponent<DialogueTrigger>();
                if (dialogueTrigger != null)
                {
                    dialogueTrigger.enabled = false; // Отключаем компонент DialogueTrigger
                }

            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Quest quest = QuestManager.instance.GetQuestById(questIdToActivate);

            // Запускаем переход, только если квест завершен
            if (quest != null && quest.isCompleted)
            {
                if (SceneDataCarrier.Instance != null && !SceneDataCarrier.Instance.isPlayedBeforeEjidze)
                { 
                    pos = other.transform.position;
                    // ✅ Запускаем корутину для проигрывания анимации и загрузки сцены
                    StartCoroutine(PlayTransitionAndLoadScene());
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (QuestManager.instance == null) return;

            Quest completedQuest = QuestManager.instance.GetQuestById(questIdToComplete);
            if (completedQuest != null && completedQuest.isCompleted)
            {
                return;
            }

            if (isFirstInteraction)
            {
                QuestManager.instance.SetActiveQuest(questIdToActivate);
                isFirstInteraction = false;
                QuestManager.instance.SaveQuestState(uniqueId, isFirstInteraction);
            }
            else
            {
                bool isQuestToCompleteActive = QuestManager.instance.activeQuests.Exists(q => q.id == questIdToComplete);
                if (isQuestToCompleteActive && AppleCollector.applesCount >= requiredApplesCount)
                {
                    QuestManager.instance.CompleteQuest(questIdToComplete);
                    animator.SetBool(happyAnimParam, true);
                    Debug.Log("Квест выполнен. Установка анимации IsHappy.");
                }
            }
        }
    }

    // ✅ Новая корутина для плавного перехода
    private IEnumerator PlayTransitionAndLoadScene()
    {
        if (transitionAnimator != null)
        {
            // Запускаем анимацию "WhiteOut"
            transitionAnimator.SetTrigger("WhiteIn");

            // Ждём, пока анимация WhiteOut завершится
            float transitionTime = 1f; // Или получите точное время из Animator Controller
            AnimatorStateInfo stateInfo = transitionAnimator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("WhiteIn"))
            {
                transitionTime = stateInfo.length;
            }
            yield return new WaitForSeconds(transitionTime);
        }
        else
        {
            Debug.LogError("Transition Animator не назначен!");
            // В случае отсутствия аниматора, просто ждём короткое время
            yield return new WaitForSeconds(0.5f);
        }

        // Устанавливаем флаг и загружаем сцену
        SceneDataCarrier.Instance.isPlayedBeforeEjidze = true;
        LocationLoader.Load("Returning Ejidze's Apples", SceneManager.GetActiveScene().name, true, pos);
    }
}