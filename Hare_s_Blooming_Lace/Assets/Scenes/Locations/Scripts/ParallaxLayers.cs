using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;

    // Новые переменные для ограничений
    [Header("Ограничения движения")]
    public bool useBounds = false; // Флаг, чтобы включить/отключить ограничения
    public float minX;
    public float maxX;

    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * parallaxFactor;

        // Если флаг useBounds включен, то применяем ограничения
        if (useBounds)
        {
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        }

        transform.localPosition = newPos;
    }
}