using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [Header("Настройки перехода")]
    public string targetSceneName;

    // Добавляем новые переменные для координат
    public bool shouldTeleport = false;
    public Vector2 targetPosition;

    private bool playerInTrigger = false;

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            // Теперь мы передаем координаты вместе с названием сцены
            LocationLoader.Load(targetSceneName, shouldTeleport, targetPosition);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            Debug.Log("Нажмите E, чтобы перейти на следующую локацию.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}