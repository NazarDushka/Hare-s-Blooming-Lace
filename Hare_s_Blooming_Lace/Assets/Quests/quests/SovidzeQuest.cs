using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SovidzeQuest : MonoBehaviour
{
    // Уникальный ID для сохранения состояния первого взаимодействия
    public string uniqueId = "SovaFirstContact";

    [Tooltip("ID квеста, который нужно активировать.")]
    public int questIdToActivate = 3;

    public Animator animator;

    [Tooltip("Название анимации появления.")]
    public string appearingAnimName = "Appearing";

    [Tooltip("Название анимации бездействия.")]
    public string idleAnimName = "Idle";

    private bool playerInRange = false;
    private bool isFirstInteraction;
    private bool hasAppeared = false;
    private Vector2 pos;

    [Tooltip("Аниматор для перехода между сценами (оставьте пустым, если не нужна анимация)")]
    public Animator transitionAnimator;

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
        }
        Quest quest = QuestManager.instance.GetQuestById(questIdToActivate);
        if (quest.isCompleted)
        {
            animator.Play("Pointer");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Quest quest = QuestManager.instance.GetQuestById(questIdToActivate);

        if (other.CompareTag("Player") && !quest.isCompleted)
        {
            playerInRange = true;
            Debug.Log("Игрок вошел в зону взаимодействия с Совой.");

            Debug.Log("Игрок вошел в зону. Загрузка сцены с катсценой...");
            // Запускаем переход, только если квест завершен
            if (quest != null && !quest.isCompleted)
            {
                if (!SceneDataCarrier.Instance.isPlayedBeforeSovidze)
                {
                    pos = other.transform.position;
                    // ✅ Запускаем корутину для проигрывания анимации и загрузки сцены
                    StartCoroutine(PlayTransitionAndLoadScene());
                }
                else
                {
                    if (animator != null && !hasAppeared)
                    {
                        // Запускаем анимацию появления
                        animator.Play(appearingAnimName);

                        // Устанавливаем флаг, чтобы анимация сработала только один раз
                        hasAppeared = true;
                    }
                }
            }
            
        }
    }

    // Вспомогательный корутин для задержки и загрузки сцены
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
        SceneDataCarrier.Instance.isPlayedBeforeSovidze = true;
        LocationLoader.Load("Sovidze Meeting", SceneManager.GetActiveScene().name, true, pos);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Игрок вышел из зоны взаимодействия с Совой.");
        }
    }

    private void Update()
    {
        
        // Проверяем, находится ли игрок в зоне и нажал ли клавишу E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("isFirstInteraction: " + isFirstInteraction);
            if (QuestManager.instance == null)
            {
                Debug.LogError("QuestManager не найден в сцене!");
                return;
            }

            if (isFirstInteraction)
            {
                // Если это первое взаимодействие, активируем квест
                QuestManager.instance.SetActiveQuest(questIdToActivate);
                Debug.Log($"Активирован квест с ID {questIdToActivate}.");

                // Сохраняем состояние, чтобы избежать повторной активации
                isFirstInteraction = false;
                QuestManager.instance.SaveQuestState(uniqueId, isFirstInteraction);
            }
            else
            {
                Debug.Log("Квест уже активирован. Дополнительный диалог не предусмотрен.");
            }
            // Можно добавить else-блок для диалога, если квест уже активирован
        }
    }
}
