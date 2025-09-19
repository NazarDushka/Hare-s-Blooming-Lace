using UnityEngine;
using UnityEngine.EventSystems;

public class MyButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator buttonAnimator; // ���������� ���� Animator ����� ������

    public AudioSource soundmanager; // ���������� ���� AudioSource � ����� ������ ���������
    public AudioClip hoverClip; // ���������� ���� ��� �������� ����
    public AudioClip clickClip; // ���������� ���� ��� �������� ����

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
        Debug.Log($"���� �������� �� ������: {gameObject.name}"); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonAnimator != null)
        {
            buttonAnimator.SetTrigger("onNormal");
        }
        Debug.Log($"���� ���� � ������: {gameObject.name}"); 
    }
    public void OnButtonClick()
    {
        if (soundmanager != null && clickClip != null)
        {
            soundmanager.PlayOneShot(clickClip);
        }
        Debug.Log($"������ ������: {gameObject.name}");
    }
}