using UnityEngine;

public class HintScript : MonoBehaviour
{
    public GameObject GameObject;

    void Start()
    {

        GameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, что вошёл именно игрок по тегу "Player"
        if (other.CompareTag("Player"))
        {
            // Включаем весь игровой объект
            GameObject.SetActive(true);
            Debug.Log("Подсказка включена.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Проверяем, что вышел именно игрок
        if (other.CompareTag("Player"))
        {
            // Выключаем весь игровой объект
            GameObject.SetActive(false);
            Debug.Log("Подсказка выключена.");
        }
    }
}