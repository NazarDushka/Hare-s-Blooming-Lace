using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    // Этот метод будет вызываться сразу, как только персонаж появится в сцене
    void Start()
    {
        // Проверяем, нужно ли нам телепортироваться
        if (LocationLoader.shouldTeleport)
        {
            // Перемещаем персонажа в нужную точку
            transform.position = LocationLoader.teleportPosition;

            // Сбрасываем флаг, чтобы при следующей загрузке не было телепортации
            LocationLoader.shouldTeleport = false;
        }
    }
}