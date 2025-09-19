using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- Настраиваемые поля в инспекторе ---
    [Header("Настройки движения")]
    [Tooltip("Максимальная скорость передвижения")]
    public float maxSpeed = 4.6f;
    [Tooltip("Скорость набора скорости")]
    public float acceleration = 15f;
    [Tooltip("Скорость торможения")]
    public float linearDrag = 15f;

    [Header("Настройки прыжка")]
    [Tooltip("Сила прыжка")]
    public float jumpForce = 10f;
    [Tooltip("Длительность замедления в верхней точке прыжка")]
    public float jumpHangTime = 0.2f;

    [Header("Проверка земли")]
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask whatIsGround;

    // --- Компоненты ---
    private Rigidbody2D rb;
    private Animator animator;

    // --- Внутренние переменные ---
    private float moveInput;
    public float currentSpeed;
    private float jumpTimer;
    public bool isGrounded;


    // Start вызывается один раз при запуске игры
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update вызывается каждый кадр. Здесь только ввод игрока.
    void Update()
    {
        // 1. Проверяем ввод от игрока
        moveInput = Input.GetAxisRaw("Horizontal");

        // 2. Проверяем, стоит ли персонаж на земле
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        // 3. Обрабатываем прыжок
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpTimer = Time.time + jumpHangTime;
        }

        // 4. Обновляем аниматор
        UpdateAnimations();

        // 5. Проверяем и переворачиваем спрайт
        CheckDirection();
    }

    // FixedUpdate вызывается с фиксированной частотой. Здесь только физика.
    void FixedUpdate()
    {
        // Check if the player is giving any horizontal input
        if (moveInput != 0)
        {
            // Smoothly accelerate to the target speed
            float targetSpeed = moveInput * maxSpeed;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

            // Apply the new velocity
            rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
        }
        else
        {
            // If there's no input, stop immediately
            currentSpeed = 0;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        // Manage gravity for the jump hang effect
        if (rb.linearVelocity.y > 0 && Time.time < jumpTimer)
        {
            rb.gravityScale = 0.5f;
        }
        else
        {
            rb.gravityScale = 1.0f;
        }
    }

    // Функция для обновления всех параметров аниматора
    void UpdateAnimations()
    {
        // Передаем скорость в аниматор (Mathf.Abs делает любое число положительным)
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
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
    }

    // Это необязательная функция, она просто рисует в редакторе круг, чтобы было видно радиус проверки земли
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}