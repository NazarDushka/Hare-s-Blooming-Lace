using UnityEngine;
using UnityEngine.UI;
using System.Collections; // ��� ������������� Coroutines

public class BackgroundTransitioner : MonoBehaviour
{
    [Tooltip("���������� ���� ��������� Animator � ������� BackgroundController.")]
    public Animator backgroundAnimator;

    [Header("������ UI:")]
    [Tooltip("���������� ���� ��� GameObject ������ �������� (OptionsMenu).")]
    public GameObject optionsPanel;
    [Tooltip("���������� ���� ��� GameObject ������ Credits (��� ������ ������).")]
    public GameObject creditsPanel;
    // ���������� ���� ������ ������ �� ���� �������������

    private GameObject currentActivePanel; // ��� ������������, ����� ������ ������ �������

    [Header("������ �� ������� ���� (��� ���������� �����������):")]
    [Tooltip("������� ������ �������� ���� (AnimatedBackground).")]
    public GameObject normalBackgroundObject;
    [Tooltip("������� ������ ��������� ���� (AnimatedBackground(Blurred)).")]
    public GameObject blurredBackgroundObject;

    [Tooltip("������������ �������� BlurOut � ��������. ������������ ��� �������� ������� ������.")]
    public float blurOutDuration = 0.5f;

    void Awake()
    {
        if (backgroundAnimator == null)
        {
            backgroundAnimator = GetComponent<Animator>();
            if (backgroundAnimator == null)
            {
                Debug.LogError("BackgroundTransitioner: Animator component not found on " + gameObject.name + ". Please assign it in the Inspector.");
                enabled = false;
                return;
            }
        }

        // �������������� ����� �������� ����, ���� ��� �� ��������� �������
        if (normalBackgroundObject == null)
        {
            Transform normalBgTransform = transform.Find("AnimatedBackground");
            if (normalBgTransform != null)
            {
                normalBackgroundObject = normalBgTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("BackgroundTransitioner: 'AnimatedBackground' not found as a direct child of " + gameObject.name);
            }
        }
        if (blurredBackgroundObject == null)
        {
            Transform blurredBgTransform = transform.Find("AnimatedBackground(Blurred)");
            if (blurredBgTransform != null)
            {
                blurredBackgroundObject = blurredBgTransform.gameObject;
            }
            else
            {
                Debug.LogWarning("BackgroundTransitioner: 'AnimatedBackground(Blurred)' not found as a direct child of " + gameObject.name);
            }
        }

        // ������������� ��������� ������� � ���� ��� ������ ����
        // ������������, ��� ��� ������ �� ���� �� ������� ���������, � ��� �������.
        // ��� ����� ��������� � Inspector, ������ ��� ������ ����������� �� ���������.
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        // ... �������� ������ ������ ����� ...

        // ��������, ��� ��� ���������� � ���������� ���������
        if (normalBackgroundObject != null && blurredBackgroundObject != null)
        {
            backgroundAnimator.Play("IdleNormal"); // �����������, ��� Animator � "�������" ���������
            normalBackgroundObject.SetActive(true);
            blurredBackgroundObject.SetActive(false);
            currentActivePanel = null; // ��� �������� ������ ��� ������
        }
    }

    /// <summary>
    /// ���������� ��������� ������ � ��������� ���.
    /// </summary>
    /// <param name="panelToShow">������, ������� ����� �������� (��������, optionsPanel ��� creditsPanel).</param>
    public void ShowPanel(GameObject panelToShow)
    {
        if (panelToShow == null)
        {
            Debug.LogWarning("ShowPanel: �� ������� ������ ��� ������.");
            return;
        }

        // ���� ��� ���� �������� ������, ������� ������ ��, ����� �������� ���������
        if (currentActivePanel != null && currentActivePanel != panelToShow)
        {
            currentActivePanel.SetActive(false); // ��������� �������� ����������
        }

        panelToShow.SetActive(true);
        currentActivePanel = panelToShow; // ����������, ����� ������ ������ �������

        // �������� ����
        if (normalBackgroundObject != null && blurredBackgroundObject != null)
        {
            blurredBackgroundObject.SetActive(true); // ���������� �������� ��� ��� ��������
        }
        if (backgroundAnimator != null)
        {
            backgroundAnimator.SetTrigger("BlurIn");
        }
        Debug.Log($"Showing {panelToShow.name} and blurring background.");
    }

    /// <summary>
    /// �������� ������� �������� ������ � ���������� ��� � ������� ���������.
    /// </summary>
    public void HideActivePanel()
    {
        if (backgroundAnimator != null)
        {
            backgroundAnimator.SetTrigger("BlurOut");
        }
        Debug.Log("Hiding active panel and unblurring background.");

        if (currentActivePanel != null)
        {
            // �������� ������ � �������� ��� ����� ���������� �������� BlurOut
            Invoke("DeactivateCurrentPanel", blurOutDuration);
            Invoke("DeactivateBlurredBackground", blurOutDuration);
        }
    }

    private void DeactivateCurrentPanel()
    {
        if (currentActivePanel != null)
        {
            currentActivePanel.SetActive(false);
            Debug.Log($"Panel {currentActivePanel.name} deactivated.");
            currentActivePanel = null; // ���������� �������� ������
        }
    }

    private void DeactivateBlurredBackground()
    {
        if (blurredBackgroundObject != null)
        {
            blurredBackgroundObject.SetActive(false);
            if (normalBackgroundObject != null)
            {
                normalBackgroundObject.SetActive(true); // �����������, ��� ������� ��� �������
            }
            Debug.Log("Blurred background deactivated, Normal background activated.");
        }
    }
}