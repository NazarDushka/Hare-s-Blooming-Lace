using System.Collections;
using UnityEngine;

public class QuestCompleter : MonoBehaviour
{
    [Tooltip("ID квеста, который нужно завершить при взаимодействии.")]
    public int questIdToComplete = 3;

    [Tooltip("ID квестов, которые нужно проверить на завершение перед активацией следующего.")]
    public int questIdToVerify_1 = 2;
    public int questIdToVerify_2 = 3;

    [Tooltip("ID квеста, который нужно запустить после выполнения всех проверок.")]
    public int nextQuestId = 4;

    public Animator animator;
    public float destroyDelay = 3.017f; // Задержка в секундах перед уничтожением

    private bool playerInRange = false;

    private void Awake()
    {
        Quest quest = QuestManager.instance.GetQuestById(questIdToComplete);
        if (quest.isCompleted)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject); // Если родителя нет, уничтожаем сам объект
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Игрок вошел в зону завершения квеста.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Игрок вышел из зоны завершения квеста.");
        }
    }

    private void Update()
    {
        // Проверяем, находится ли игрок в зоне и нажал ли клавишу 'E'
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (QuestManager.instance == null)
            {
                Debug.LogError("QuestManager не найден в сцене!");
                return;
            }

            // Завершаем текущий квест
            QuestManager.instance.CompleteQuest(questIdToComplete);
            Debug.Log($"Квест с ID {questIdToComplete} успешно завершен.");

            // Запускаем корутину для проигрывания анимации и последующего уничтожения
            StartCoroutine(HandleQuestCompletion());
        }
    }

    private IEnumerator HandleQuestCompletion()
    {
        // Проверяем, есть ли аниматор, и запускаем анимацию
        if (animator != null)
        {
            animator.Play("DeliveringBox");
        }
        else
        {
            Debug.LogWarning("Animator не назначен в инспекторе, анимация 'DeliveringBox' не будет проиграна.");
        }

        // Ждем указанное время перед уничтожением объекта
        yield return new WaitForSeconds(destroyDelay);

        // Уничтожаем родительский объект, если он существует
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject); // Если родителя нет, уничтожаем сам объект
        }
        // Получаем ссылки на квесты для проверки
        Quest quest2 = QuestManager.instance.GetQuestById(questIdToVerify_1);
        Quest quest3 = QuestManager.instance.GetQuestById(questIdToVerify_2);

        //// Проверяем, выполнены ли оба квеста
        //if (quest2 != null && quest2.isCompleted && quest3 != null && quest3.isCompleted)
        //{
        //    // Если оба квеста выполнены, активируем следующий квест
            QuestManager.instance.SetActiveQuest(nextQuestId);
        //    Debug.Log($"Квесты {questIdToVerify_1} и {questIdToVerify_2} выполнены. Активирован квест {nextQuestId}.");

            
        //}
        //else
        //{
        //    Debug.Log("Один или оба квеста-предшественника еще не завершены.");
        //}
    }
}