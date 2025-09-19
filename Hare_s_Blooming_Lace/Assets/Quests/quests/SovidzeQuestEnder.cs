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
    public float destroyDelay = 2f; // Задержка в секундах перед уничтожением

    public GameObject player; // Ссылка на игрока
    public Animator blackoutAnimator; // Аниматор для затемнения экрана

    private bool playerInRange = false;

    private void Awake()
    {
        Quest quest = QuestManager.instance.GetQuestById(questIdToComplete);
        if (quest != null && quest.isCompleted)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
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

            StartCoroutine(PlayAnimationsInSequence());
        }
    }

    private IEnumerator PlayAnimationsInSequence()
    {
        Debug.Log("Корутина PlayAnimationsInSequence запущена.");

        var rb2d = player.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;
            Debug.Log("Скорость игрока сброшена.");
        }

        //// 1. Проигрываем анимацию BlackIn
        //if (blackoutAnimator != null)
        //{
        //    Debug.Log("Запуск анимации BlackIn.");
        //    blackoutAnimator.Play("BlackIn");
        //    yield return new WaitForSeconds(0.1f);

        //    while (!blackoutAnimator.GetCurrentAnimatorStateInfo(0).IsName("BlackIn") || blackoutAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        //    {
        //        Debug.Log($"Ожидание BlackIn... normalizedTime: {blackoutAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime}");
        //        yield return null;
        //    }
        //    Debug.Log("Анимация BlackIn завершена.");
        //}
        //else
        //{
        //    Debug.LogWarning("Blackout Animator не назначен. Пропускаем BlackIn.");
        //}

        player.transform.position = new Vector2(63.56f, -1.674056f);
        Debug.Log("Позиция игрока перемещена.");
        yield return new WaitForSeconds(0.5f);

        //// ✅ 2. Проигрываем анимацию OutOfBlack с фиксированным ожиданием
        //if (blackoutAnimator != null)
        //{
        //    Debug.Log("Запуск анимации OutOfBlack.");
        //    blackoutAnimator.Play("OutOfBlack");
        //    // Ждем ровно столько, сколько длится анимация
        //    yield return new WaitForSeconds(blackoutAnimator.GetCurrentAnimatorStateInfo(0).length);
        //    Debug.Log("Анимация OutOfBlack завершена.");
        //}
        //else
        //{
        //    Debug.LogWarning("Blackout Animator не назначен. Пропускаем OutOfBlack.");
        //}

        // 3. Проигрываем анимацию owlBox и ждём её завершения
        if (animator != null)
        {
            Debug.Log("Запуск анимации owlbox.");
            animator.Play("owlbox");
            yield return new WaitForSeconds(0.1f);

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("owlbox") || animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                Debug.Log($"Ожидание owlbox... normalizedTime: {animator.GetCurrentAnimatorStateInfo(0).normalizedTime}");
                yield return null;
            }
            Debug.Log("Анимация owlbox завершена.");
        }
        else
        {
            Debug.LogWarning("Animator не назначен, анимация 'owlBox' не будет проиграна.");
        }

        Debug.Log("Ожидание финальной задержки.");
        yield return new WaitForSeconds(destroyDelay);
        Debug.Log("Финальная задержка завершена.");

        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero;
            Debug.Log("Скорость игрока сброшена повторно.");
        }

        // Логика активации квеста
        Quest quest2 = QuestManager.instance.GetQuestById(questIdToVerify_1);
        Quest quest3 = QuestManager.instance.GetQuestById(questIdToVerify_2);

        // ✅ Проверяем, выполнены ли оба квеста
        //if (quest2 != null && quest2.isCompleted && quest3 != null && quest3.isCompleted)
        //{
        //    // Если оба квеста выполнены, активируем следующий квест
        QuestManager.instance.SetActiveQuest(nextQuestId);
        //    Debug.Log($"Квесты {questIdToVerify_1} и {questIdToVerify_2} выполнены. Активирован квест {nextQuestId}.");
        //}
        //else
        //{
        //    Debug.Log("Один или оба квеста-предшественника еще не завершены. Следующий квест не активирован.");
        //}

        if (transform.parent != null)
        {
            Debug.Log("Уничтожение родительского объекта.");
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Debug.Log("Уничтожение текущего объекта.");
            Destroy(gameObject);
        }
    }
}