using UnityEngine;
using UnityEngine.EventSystems; 
                                

public class ButtonHoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // [Tooltip] - ��� �������, ������� ��������� ����������� ��������� � ���������� Unity
    // ��� ��������� �� ����. ������� ��� ������������!

    [Tooltip("������ �� ��������� Animator �� ���� �� ������� �������. ������ ����������� �������������.")]
    public Animator buttonAnimator; // ����� �� ����� ������� ������ �� ��������� Animator ������.
                                    // �� � ���� ����� ����������, ����� "�������" Animator'�,
                                    // ����� �������� ���������.

    [Tooltip("��� ��������, ������� ��������� �������� ��������� '���������' � Animator Controller. ��������, 'OnHover'.")]
    public string hoverTriggerName = "onHover"; // ��� ��������� ��� ������ �������� "���������" � Animator.
                                                // ������ ��������� � ���, ��� �� ������ � Animator.

    [Tooltip("��� ��������, ������� ��������� �������� '�����������' ��������� � Animator Controller. ��������, 'OnNormal'.")]
    public string normalTriggerName = "onNormal"; // ��� ��������� ��� ������ �������� "����������" ���������.
                                                  // ������ ��������� � ���, ��� �� ������ � Animator.

    // ����� Awake() ���������� ���� ���, ����� ������ �����������.
    // �� ���������� ������ ������ Start() � OnEnable().
    // ��� ������� ����� ��� ��������� ������ �� ������ ����������.
    void Awake()
    {
        // ���������, ��������� �� ������ �� Animator � ����������.
        // ���� ��� (�� ����, ���� buttonAnimator ������), �� �������� ����� ���
        // �� ���� �� ������� ������� (GetComponent<Animator>()).
        if (buttonAnimator == null)
        {
            buttonAnimator = GetComponent<Animator>();
        }

        // ���� ����� ������� ����� Animator, �� ��� ��� null (�� ������),
        // ������, ���-�� ����� �� ���. ������� ��������� �� ������ � ������� Unity.
        if (buttonAnimator == null)
        {
            Debug.LogError("������: ��������� Animator �� ������ �� ������� '" + gameObject.name + "'. �������� ��������� �� ����� ��������.", this);
            // 'this' � Debug.LogError ��������� �������� �� ������ � �������
            // � ���������� ������, �� ������� ��������� ���� ������.

            enabled = false; // ��������� ���� ������, ����� �������� ���������� ������,
                             // ��� ��� �� �� ������ �������� ��� Animator.
            return;          // ������� �� ������ Awake().
        }

        // ��� ������� ���� ��� ��� ������ ��������� �������,
        // �� ����� ������������� ���������� ��������� ������.
        // ��� �����������, ��� ������ �������� ��������� � ������ ������.
        buttonAnimator.SetTrigger(normalTriggerName);
    }

    // ����� OnPointerEnter ���������� �������� EventSystem Unity
    // ������ ���, ����� ������ ���� ������ � ������� UI-��������,
    // � �������� ���������� ���� ������.
    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���������, ��� Animator ����������, ������ ��� �������� � ��� ��������.
        if (buttonAnimator != null)
        {
            // ������������� ������� "Hover" � Animator'�.
            // ��� "�������" Animator'� ������� � ��������� �������� ���������.
            buttonAnimator.SetTrigger(hoverTriggerName);
            Debug.Log("���� �������� �� ������: " + gameObject.name);
        }
    }

    // ����� OnPointerExit ���������� �������� EventSystem Unity
    // ������ ���, ����� ������ ���� �������� ������� UI-��������.
    public void OnPointerExit(PointerEventData eventData)
    {
        // ���������, ��� Animator ����������.
        if (buttonAnimator != null)
        {
            // ������������� ������� "Normal" � Animator'�.
            // ��� "�������" Animator'� ��������� � ���������� ���������.
            buttonAnimator.SetTrigger(normalTriggerName);
            Debug.Log("���� ���� � ������: " + gameObject.name);
        }
    }
}