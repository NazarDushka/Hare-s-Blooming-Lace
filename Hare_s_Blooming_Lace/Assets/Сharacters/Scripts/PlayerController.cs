using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- Настраиваемые поля в инспекторе ---
    [Header("Настройки движения")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Проверка земли")]
    public Transform groundCheck; // Объект для проверки, стоит ли персонаж на земле
    public float checkRadius = 0.2f; // Радиус проверки
    public LayerMask whatIsGround; // Слой, который считается землей

    // --- Компоненты ---
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // --- Внутренние переменные ---
    private float moveInput;
    private bool isGrounded;
    private bool isFacingRight = true; // Переменная, чтобы отслеживать, куда смотрит персонаж

    // Start вызывается один раз при запуске игры
    void Start()
    {
        // Получаем ссылки на компоненты, прикрепленные к этому же объекту
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update вызывается каждый кадр
    void Update()
    {
        // 1. Проверяем ввод от игрока
        moveInput = Input.GetAxisRaw("Horizontal"); // -1 для движения влево, 1 для вправо, 0 на месте

        // 2. Проверяем, стоит ли персонаж на земле
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // 3. Обрабатываем прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // 4. Обновляем аниматор
        UpdateAnimations();

        // 5. Проверяем и переворачиваем спрайт
        CheckDirection();
    }

    // FixedUpdate вызывается с фиксированной частотой, идеально для физики
    void FixedUpdate()
    {
        // Применяем движение к Rigidbody2D
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // Функция для обновления всех параметров аниматора
    void UpdateAnimations()
    {
        // Передаем скорость в аниматор (Mathf.Abs делает любое число положительным)
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        // Сообщаем аниматору, в прыжке ли мы
        animator.SetBool("IsJumping", !isGrounded);
    }

    // Функция, которая проверяет направление движения и переворачивает спрайт
    void CheckDirection()
    {
        // Если движемся вправо, но смотрим влево...
        if (moveInput > 0)
        {
            // Переворачиваемся вправо
            transform.localScale = new Vector3(-1, 1, 1);
        }
        // Если движемся влево...
        else if (moveInput < 0)
        {
            // Переворачиваемся влево
            transform.localScale = new Vector3(1, 1, 1);
        }
        // Если не двигаемся...
        else
        {
            // Возвращаем масштаб в исходное состояние (смотрим вправо)
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    // Это необязательная функция, она просто рисует в редакторе круг, чтобы было видно радиус проверки земли
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}