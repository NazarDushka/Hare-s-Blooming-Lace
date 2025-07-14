using UnityEngine;
using UnityEngine.UI;
using System.Collections; // Для использования Coroutines

public class BackgroundTransitioner : MonoBehaviour
{
    [Tooltip("Перетащите сюда компонент Animator с объекта BackgroundController.")]
    public Animator backgroundAnimator;

    [Header("Панели UI:")]
    [Tooltip("Перетащите сюда ваш GameObject панели настроек (OptionsMenu).")]
    public GameObject optionsPanel;
    [Tooltip("Перетащите сюда ваш GameObject панели Credits (или другой панели).")]
    public GameObject creditsPanel;
    // Добавляйте сюда другие панели по мере необходимости

    private GameObject currentActivePanel; // Для отслеживания, какая панель сейчас открыта

    [Header("Ссылки на объекты фона (для управления активностью):")]
    [Tooltip("Игровой объект обычного фона (AnimatedBackground).")]
    public GameObject normalBackgroundObject;
    [Tooltip("Игровой объект размытого фона (AnimatedBackground(Blurred)).")]
    public GameObject blurredBackgroundObject;

    [Tooltip("Длительность анимации BlurOut в секундах. Используется для задержки скрытия панели.")]
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

        // Автоматический поиск объектов фона, если они не назначены вручную
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

        // Инициализация состояний панелей и фона при старте игры
        // Предполагаем, что при старте ни одна из панелей неактивна, и фон обычный.
        // Это можно настроить в Inspector, сделав все панели неактивными по умолчанию.
        if (optionsPanel != null) optionsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        // ... добавьте другие панели здесь ...

        // Убедимся, что фон начинается в правильном состоянии
        if (normalBackgroundObject != null && blurredBackgroundObject != null)
        {
            backgroundAnimator.Play("IdleNormal"); // Гарантируем, что Animator в "обычном" состоянии
            normalBackgroundObject.SetActive(true);
            blurredBackgroundObject.SetActive(false);
            currentActivePanel = null; // Нет активной панели при старте
        }
    }

    /// <summary>
    /// Показывает указанную панель и размывает фон.
    /// </summary>
    /// <param name="panelToShow">Панель, которую нужно показать (например, optionsPanel или creditsPanel).</param>
    public void ShowPanel(GameObject panelToShow)
    {
        if (panelToShow == null)
        {
            Debug.LogWarning("ShowPanel: Не указана панель для показа.");
            return;
        }

        // Если уже есть активная панель, сначала скроем ее, чтобы избежать наложения
        if (currentActivePanel != null && currentActivePanel != panelToShow)
        {
            currentActivePanel.SetActive(false); // Мгновенно скрываем предыдущую
        }

        panelToShow.SetActive(true);
        currentActivePanel = panelToShow; // Запоминаем, какая панель сейчас активна

        // Размытие фона
        if (normalBackgroundObject != null && blurredBackgroundObject != null)
        {
            blurredBackgroundObject.SetActive(true); // Активируем размытый фон для анимации
        }
        if (backgroundAnimator != null)
        {
            backgroundAnimator.SetTrigger("BlurIn");
        }
        Debug.Log($"Showing {panelToShow.name} and blurring background.");
    }

    /// <summary>
    /// Скрывает текущую активную панель и возвращает фон в обычное состояние.
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
            // Скрываем панель и размытый фон после завершения анимации BlurOut
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
            currentActivePanel = null; // Сбрасываем активную панель
        }
    }

    private void DeactivateBlurredBackground()
    {
        if (blurredBackgroundObject != null)
        {
            blurredBackgroundObject.SetActive(false);
            if (normalBackgroundObject != null)
            {
                normalBackgroundObject.SetActive(true); // Гарантируем, что обычный фон активен
            }
            Debug.Log("Blurred background deactivated, Normal background activated.");
        }
    }
}