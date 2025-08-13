using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [Header("Настройки камеры")]
    public Transform Player;
    public float smoothSpeed = 1.5f;
    public float teleportDistance = 20f; // Дистанция для телепортации

    [Header("Ограничения карты")]
    public float minX;
    public float maxX;

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
            // Создаем новую позицию для камеры
            Vector3 targetPosition = new Vector3(Player.position.x, yOffset, zOffset);

            // --- НОВАЯ ЛОГИКА ---
            // Проверяем, если расстояние слишком большое
            if (Vector3.Distance(transform.position, targetPosition) > teleportDistance)
            {
                // Мгновенно перемещаем камеру к игроку
                transform.position = targetPosition;
            }
            else
            {
                // Иначе используем плавное движение
                Vector3 clampedTargetPosition = new Vector3(
                    Mathf.Clamp(targetPosition.x, minX, maxX),
                    yOffset,
                    zOffset
                );

                Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedTargetPosition, smoothSpeed * Time.fixedDeltaTime);
                transform.position = smoothedPosition;
            }
        }
        else
        {
            Debug.LogWarning("Player Transform is not assigned in CameraScript.");
        }
    }
}