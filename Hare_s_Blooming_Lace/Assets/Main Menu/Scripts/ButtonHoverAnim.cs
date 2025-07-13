using UnityEngine;
using UnityEngine.EventSystems; 
                                

public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // [Tooltip] - это атрибут, который добавляет всплывающую подсказку в Инспекторе Unity
    // при наведении на поле. Полезно для документации!

    [Tooltip("Ссылка на компонент Animator на этом же игровом объекте. Обычно назначается автоматически.")]
    public Animator buttonAnimator; // Здесь мы будем хранить ссылку на компонент Animator кнопки.
                                    // Мы к нему будем обращаться, чтобы "сказать" Animator'у,
                                    // какую анимацию проиграть.

    [Tooltip("Имя триггера, который запускает анимацию состояния 'наведения' в Animator Controller. Например, 'OnHover'.")]
    public string hoverTriggerName = "onHover"; // Это текстовое имя нашего триггера "наведение" в Animator.
                                                // Должно совпадать с тем, что ты указал в Animator.

    [Tooltip("Имя триггера, который запускает анимацию 'нормального' состояния в Animator Controller. Например, 'OnNormal'.")]
    public string normalTriggerName = "onNormal"; // Это текстовое имя нашего триггера "нормальное" состояние.
                                                  // Должно совпадать с тем, что ты указал в Animator.

    // Метод Awake() вызывается один раз, когда скрипт загружается.
    // Он вызывается раньше метода Start() и OnEnable().
    // Это хорошее место для получения ссылок на другие компоненты.
    void Awake()
    {
        // Проверяем, назначена ли ссылка на Animator в инспекторе.
        // Если нет (то есть, поле buttonAnimator пустое), то пытаемся найти его
        // на этом же игровом объекте (GetComponent<Animator>()).
        if (buttonAnimator == null)
        {
            buttonAnimator = GetComponent<Animator>();
        }

        // Если после попытки найти Animator, он все еще null (не найден),
        // значит, что-то пошло не так. Выводим сообщение об ошибке в консоль Unity.
        if (buttonAnimator == null)
        {
            Debug.LogError("Ошибка: Компонент Animator не найден на объекте '" + gameObject.name + "'. Анимация наведения не будет работать.", this);
            // 'this' в Debug.LogError позволяет кликнуть на ошибку в консоли
            // и подсветить объект, на котором находится этот скрипт.

            enabled = false; // Отключаем этот скрипт, чтобы избежать дальнейших ошибок,
                             // так как он не сможет работать без Animator.
            return;          // Выходим из метода Awake().
        }

        // При запуске игры или при первой активации объекта,
        // мы сразу устанавливаем нормальное состояние кнопки.
        // Это гарантирует, что кнопка выглядит правильно с самого начала.
        buttonAnimator.SetTrigger(normalTriggerName);
    }

    // Метод OnPointerEnter вызывается системой EventSystem Unity
    // каждый раз, когда курсор мыши входит в область UI-элемента,
    // к которому прикреплен этот скрипт.
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Проверяем, что Animator существует, прежде чем пытаться с ним работать.
        if (buttonAnimator != null)
        {
            // Устанавливаем триггер "Hover" в Animator'е.
            // Это "говорит" Animator'у перейти в состояние анимации наведения.
            buttonAnimator.SetTrigger(hoverTriggerName);
            Debug.Log("Мышь наведена на кнопку: " + gameObject.name);
        }
    }

    // Метод OnPointerExit вызывается системой EventSystem Unity
    // каждый раз, когда курсор мыши покидает область UI-элемента.
    public void OnPointerExit(PointerEventData eventData)
    {
        // Проверяем, что Animator существует.
        if (buttonAnimator != null)
        {
            // Устанавливаем триггер "Normal" в Animator'е.
            // Это "говорит" Animator'у вернуться в нормальное состояние.
            buttonAnimator.SetTrigger(normalTriggerName);
            Debug.Log("Мышь ушла с кнопки: " + gameObject.name);
        }
    }
}