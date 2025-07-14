using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // ��������� ������������ ���� ��� ������ Input System

public class MouseDustEffect : MonoBehaviour
{
    [Tooltip("������ �� ������� ������ '����'.")]
    public ParticleSystem dustParticleSystem;

    [Tooltip("Canvas, � �������� ��������� ��� UI. ����� ��� ���������� ����������� ��������� ����.")]
    public Canvas targetCanvas;

    [Tooltip("�������� Rect Transform, ������� ����������� ������ Canvas.")]
    public RectTransform canvasRectTransform;

    [Tooltip("�������� � ��������, ������ ��� ������� ������ �������� ����� ��������� �������� ����.")]
    public float particleFadeDelay = 0.5f;

    [Tooltip("������������ ������������ ������ ����� ��������.")]
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
                Debug.LogError("������: ParticleSystem 'DustParticles' �� �������� ��� �� ������ �� ������� " + gameObject.name);
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
                Debug.LogError("������: Canvas �� �������� ��� �� ������ ��� " + gameObject.name);
                enabled = false;
                return;
            }
        }
        if (canvasRectTransform == null)
        {
            canvasRectTransform = targetCanvas.GetComponent<RectTransform>();
        }

        // ������������� lastMousePosition � ������� �������� ���� �� ����� ������� �����
        if (Mouse.current != null) // ���������, ��� ���� ����������
        {
            lastMousePosition = Mouse.current.position.ReadValue();
        }
        lastMouseMovementTime = Time.time;
    }

    void Update()
    {
        // �������� ������� ������� ���� �� ������ Input System
        Vector2 currentMousePosition = Vector2.zero;
        if (Mouse.current != null) // ����������, ��� ���� ����������
        {
            currentMousePosition = Mouse.current.position.ReadValue();
        }
        else
        {
            // ���� ���� ���, �������
            emissionModule.enabled = false;
            return;
        }


        Vector2 localPoint;
        // ���������, ��������� �� Canvas � ������ Screen Space - Camera, ����� worldCamera ������ ���� ��������
        Camera currentCamera = targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCanvas.worldCamera;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, currentMousePosition, currentCamera, out localPoint))
        {
            dustParticleSystem.transform.localPosition = new Vector3(localPoint.x, localPoint.y, 0f);
        }


        // ���������, �������� �� ����
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