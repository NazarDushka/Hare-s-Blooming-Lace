using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // Добавляем пространство имен для нового Input System

public class MouseDustEffect : MonoBehaviour
{
    [Tooltip("Ссылка на систему частиц 'пыли'.")]
    public ParticleSystem dustParticleSystem;

    [Tooltip("Canvas, к которому относится ваш UI. Нужен для правильной конвертации координат мыши.")]
    public Canvas targetCanvas;

    [Tooltip("Дочерний Rect Transform, который отслеживает размер Canvas.")]
    public RectTransform canvasRectTransform;

    [Tooltip("Задержка в секундах, прежде чем частицы начнут исчезать после остановки движения мыши.")]
    public float particleFadeDelay = 0.5f;

    [Tooltip("Длительность исчезновения частиц после задержки.")]
    public float particleFadeDuration = 1.0f;

    private Vector2 lastMousePosition;
    private float lastMouseMovementTime;
    private bool isFadingOut;
    private ParticleSystem.EmissionModule emissionModule;

    void Awake()
    {
        if (dustParticleSystem == null)
        {
            dustParticleSystem = GetComponent<ParticleSystem>();
            if (dustParticleSystem == null)
            {
                Debug.LogError("Ошибка: ParticleSystem 'DustParticles' не назначен или не найден на объекте " + gameObject.name);
                enabled = false;
                return;
            }
        }

        emissionModule = dustParticleSystem.emission;
        emissionModule.enabled = false;

        if (targetCanvas == null)
        {
            targetCanvas = GetComponentInParent<Canvas>();
            if (targetCanvas == null)
            {
                Debug.LogError("Ошибка: Canvas не назначен или не найден для " + gameObject.name);
                enabled = false;
                return;
            }
        }
        if (canvasRectTransform == null)
        {
            canvasRectTransform = targetCanvas.GetComponent<RectTransform>();
        }

        // Инициализация lastMousePosition с текущей позицией мыши из новой системы ввода
        if (Mouse.current != null) // Проверяем, что мышь существует
        {
            lastMousePosition = Mouse.current.position.ReadValue();
        }
        lastMouseMovementTime = Time.time;
    }

    void Update()
    {
        // Получаем текущую позицию мыши из нового Input System
        Vector2 currentMousePosition = Vector2.zero;
        if (Mouse.current != null) // Убеждаемся, что мышь существует
        {
            currentMousePosition = Mouse.current.position.ReadValue();
        }
        else
        {
            // Если мыши нет, выходим
            emissionModule.enabled = false;
            return;
        }


        Vector2 localPoint;
        // Проверяем, находится ли Canvas в режиме Screen Space - Camera, тогда worldCamera должен быть назначен
        Camera currentCamera = targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCanvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, currentMousePosition, currentCamera, out localPoint))
        {
            dustParticleSystem.transform.localPosition = new Vector3(localPoint.x, localPoint.y, 0f);
        }


        // Проверяем, движется ли мышь
        if (Vector2.Distance(currentMousePosition, lastMousePosition) > 0.1f)
        {
            if (!emissionModule.enabled)
            {
                emissionModule.enabled = true;
            }
            isFadingOut = false;
            lastMouseMovementTime = Time.time;
        }
        else
        {
            if (emissionModule.enabled && !isFadingOut && Time.time - lastMouseMovementTime > particleFadeDelay)
            {
                isFadingOut = true;
            }

            if (isFadingOut)
            {
                if (dustParticleSystem.particleCount == 0 && emissionModule.enabled)
                {
                    emissionModule.enabled = false;
                }
            }
        }

        lastMousePosition = currentMousePosition;
    }
}