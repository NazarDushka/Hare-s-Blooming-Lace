using UnityEngine;
using UnityEngine.EventSystems;

public class MyButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator buttonAnimator; // Перетащите сюда Animator вашей кнопки

    void Start()
    {
        if (buttonAnimator == null)
        {
            buttonAnimator = GetComponent<Animator>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonAnimator != null)
        {
            // Убедитесь, что это соответствует вашему параметру Animator'а
            buttonAnimator.SetTrigger("onHover"); // <-- Это должно быть здесь
        }
        Debug.Log($"Мышь наведена на кнопку: {gameObject.name}"); // Вы уже это видите в консоли
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonAnimator != null)
        {
            // А это для возврата в normalButton
            buttonAnimator.SetTrigger("onNormal");
        }
        Debug.Log($"Мышь ушла с кнопки: {gameObject.name}"); //
    }
}