using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Настройки камеры")]
    public Transform Player; // Цель, за которой будет следить камера
    public float smoothSpeed = 1.5f; // Скорость сглаживания движения камеры

    [Header("Ограничения карты")]
    public float minX; // Минимальная позиция камеры по X
    public float maxX; // Максимальная позиция камеры по X

    private float yOffset;
    private float zOffset;

    void Start()
    {
        yOffset = transform.position.y;
        zOffset = transform.position.z;
    }

    void FixedUpdate()
    {
        if (Player != null)
        {
            // Создаем новую позицию для камеры, используя X игрока
            Vector3 targetPosition = new Vector3(Player.position.x, yOffset, zOffset);

            // Ограничиваем X-координату, чтобы она не выходила за пределы minX и maxX
            float clampedX = Mathf.Clamp(targetPosition.x, minX, maxX);

            // Применяем ограниченную X-координату к целевой позиции
            targetPosition = new Vector3(clampedX, yOffset, zOffset);

            // Сглаживаем движение камеры
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.fixedDeltaTime);

            // Применяем новую позицию к камере
            transform.position = smoothedPosition;
        }
        else
        {
            Debug.LogWarning("Player Transform is not assigned in CameraScript.");
        }
    }
}