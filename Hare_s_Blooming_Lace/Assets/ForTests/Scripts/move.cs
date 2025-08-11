// Это стандартные библиотеки, которые нужны для работы с Unity.
using UnityEngine;

// 'public' означает, что этот класс виден в Unity.
// 'MonoBehaviour' - это базовый класс для всех скриптов, которые можно прикреплять к игровым объектам.
public class PlayerMovement : MonoBehaviour
{
    // [SerializeField] позволяет нам видеть и изменять эту переменную в окне Inspector
    // даже если она 'private'. Это хорошая практика.
    [SerializeField] private float moveSpeed = 5f;

    // Ссылка на компонент Rigidbody2D. Мы будем использовать его для физического движения.
    private Rigidbody2D rb;

    // Эта переменная будет хранить направление движения.
    private float horizontalInput;

    // Start вызывается один раз, когда объект появляется в сцене.
    void Start()
    {
        // Получаем компонент Rigidbody2D, прикрепленный к этому же игровому объекту.
        // Если его нет, скрипт выдаст ошибку.
        rb = GetComponent<Rigidbody2D>();
    }

    // Update вызывается каждый кадр.
    void Update()
    {
        // Получаем ввод с горизонтальной оси.
        // Клавиша 'A' или стрелка влево вернут -1.
        // Клавиша 'D' или стрелка вправо вернут 1.
        // Ничего не нажато - 0.
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    // FixedUpdate вызывается через равные промежутки времени,
    // что делает его идеальным для работы с физикой (Rigidbody).
    void FixedUpdate()
    {
        // Создаем вектор скорости, который будет горизонтальным.
        // Сохраняем текущую вертикальную скорость (rb.linearVelocity.y), чтобы персонаж не переставал падать/прыгать.
        // *** ИСПРАВЛЕНИЕ: Используем 'linearVelocity' вместо 'velocity'
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // Присваиваем этот новый вектор скорости Rigidbody.
        // *** ИСПРАВЛЕНИЕ: Используем 'linearVelocity' вместо 'velocity'
        rb.linearVelocity = movement;

        // Вызываем функцию для поворота персонажа, чтобы он смотрел в сторону движения.
        FlipCharacter();
    }

    // Метод для поворота персонажа.
    private void FlipCharacter()
    {
        // Если персонаж движется вправо и он не повернут вправо.
        if (horizontalInput > 0)
        {
            // Поворачиваем его вправо. '1' - это нормальный масштаб.
            transform.localScale = new Vector3(1, 1, 1);
        }
        // Если персонаж движется влево.
        else if (horizontalInput < 0)
        {
            // Поворачиваем его влево. '-1' по X переворачивает спрайт.
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}