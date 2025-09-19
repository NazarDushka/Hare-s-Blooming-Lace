using UnityEngine;
using UnityEngine.EventSystems;

public class MyButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator buttonAnimator; // Перетащите сюда Animator вашей кнопки

    public AudioSource soundmanager; // Перетащите сюда AudioSource с вашим звуком наведения
    public AudioClip hoverClip; // Перетащите сюда ваш звуковой клип
    public AudioClip clickClip; // Перетащите сюда ваш звуковой клип

    void Start()
    {
        if (buttonAnimator == null)
        {
            buttonAnimator = GetComponent<Animator>();
        }
        if (soundmanager == null)
        {

            soundmanager = GameObject.Find("SoundManager")?.GetComponent<AudioSource>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonAnimator != null)
        {
           
            buttonAnimator.SetTrigger("onHover");

        }
        if (soundmanager != null && hoverClip != null)
        {
            soundmanager.PlayOneShot(hoverClip);
        }
        Debug.Log($"Мышь наведена на кнопку: {gameObject.name}"); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonAnimator != null)
        {
            buttonAnimator.SetTrigger("onNormal");
        }
        Debug.Log($"Мышь ушла с кнопки: {gameObject.name}"); 
    }
    public void OnButtonClick()
    {
        if (soundmanager != null && clickClip != null)
        {
            soundmanager.PlayOneShot(clickClip);
        }
        Debug.Log($"Кнопка нажата: {gameObject.name}");
    }
}