using UnityEngine;

public class AppleCollector : MonoBehaviour
{
    // Уникальный ID для этого яблока на сцене
    // Этот ID должен быть у родительского объекта, так как его мы будем уничтожать
    public string uniqueId;

    public static int applesCount = 0;
    private bool playerInRange = false;

    // Метод Awake вызывается при загрузке объекта
    private void Awake()
    {

        if (QuestManager.instance != null && QuestManager.instance.IsObjectCollected(uniqueId))
        {
            // Если яблоко уже собрано, уничтожаем родительский объект
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject); // Если родителя нет, уничтожаем себя
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
            Collect();
        }
    }

    private void Collect() 
    {
        
        applesCount++;
        Debug.Log($"Яблоко собрано! Всего яблок: {applesCount}");

        soundManager.instance.PlayCollectSound();
        // Сохраняем информацию о том, что яблоко собрано
        if (QuestManager.instance != null)
        {
            QuestManager.instance.AddCollectedObject(uniqueId);
        }

        // Уничтожаем родительский объект
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            // Если у объекта нет родителя, уничтожаем его самого
            Destroy(gameObject);
        }
    }
}