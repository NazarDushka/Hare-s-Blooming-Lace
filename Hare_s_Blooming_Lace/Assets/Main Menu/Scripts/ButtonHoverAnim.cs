using UnityEngine;
using UnityEngine.EventSystems;

public class MyButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator buttonAnimator; // ���������� ���� Animator ����� ������

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
            // ���������, ��� ��� ������������� ������ ��������� Animator'�
            buttonAnimator.SetTrigger("onHover"); // <-- ��� ������ ���� �����
        }
        Debug.Log($"���� �������� �� ������: {gameObject.name}"); // �� ��� ��� ������ � �������
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonAnimator != null)
        {
            // � ��� ��� �������� � normalButton
            buttonAnimator.SetTrigger("onNormal");
        }
        Debug.Log($"���� ���� � ������: {gameObject.name}"); //
    }
}