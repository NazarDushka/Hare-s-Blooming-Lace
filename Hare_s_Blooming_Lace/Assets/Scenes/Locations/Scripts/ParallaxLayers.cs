using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;

    // ����� ���������� ��� �����������
    [Header("����������� ��������")]
    public bool useBounds = false; // ����, ����� ��������/��������� �����������
    public float minX;
    public float maxX;

    public void Move(float delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta * parallaxFactor;

        // ���� ���� useBounds �������, �� ��������� �����������
        if (useBounds)
        {
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        }

        transform.localPosition = newPos;
    }
}