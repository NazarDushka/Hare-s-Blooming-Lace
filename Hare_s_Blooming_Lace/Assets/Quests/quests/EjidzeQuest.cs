using UnityEngine;

public class EjidzeQuest : MonoBehaviour
{
    public string uniqueId;
    public int questIdToActivate = 2;
    public int questIdToComplete = 2;
    public int requiredApplesCount = 9;
    public int nextQuestId = 4;
    public int questIdToVerify = 3;

    public Animator animator;
    public string happyAnimParam = "IsHappy";

    private bool playerInRange = false;
    private bool isFirstInteraction;
    private bool hasHappyAnimationPlayed;

    private void Awake()
    {
        if (animator == null)
        {
            Debug.LogError("Animator не назначен в инспекторе. Пожалуйста, привяжите его!");
            return;
        }

        if (QuestManager.instance != null)
        {
            // Загружаем состояние первого взаимодействия
            isFirstInteraction = QuestManager.instance.GetQuestState(uniqueId);

            // ✅ Проверяем, был ли квест уже завершен в предыдущей сессии
            Quest completedQuest = QuestManager.instance.GetQuestById(questIdToComplete);
            if (completedQuest != null && completedQuest.isCompleted)
            {
                // Если квест завершен, сразу ставим анимацию в "счастливую"
                animator.SetBool(happyAnimParam, true);
                Debug.Log("Квест был завершен ранее. Установка анимации IsHappy.");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
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

            // Если квест уже завершен, мы ничего не делаем
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

                    // Установка анимации и сохранение состояния
                    animator.SetBool(happyAnimParam, true);
                    Debug.Log("Квест выполнен. Установка анимации IsHappy.");

                    // Теперь сохранение происходит в QuestManager.CompleteQuest

                    //Quest questToVerify = QuestManager.instance.GetQuestById(questIdToVerify);
                    //if (questToVerify != null && questToVerify.isCompleted)
                    //{
                    //    QuestManager.instance.SetActiveQuest(nextQuestId);
                    //}
                }
            }
        }
    }
}