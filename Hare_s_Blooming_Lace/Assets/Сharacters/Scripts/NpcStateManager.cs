using UnityEngine;

public class NpcStateManager : MonoBehaviour
{
    public Animator animator;
    public string happyAnimParam = "IsHappy";

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (animator == null)
        {
            Debug.LogError("Animator �� ������ �� ������� " + gameObject.name);
        }
    }

    /// <summary>
    /// ������������� �������� "IsHappy" � true.
    /// </summary>
    public void SetHappy()
    {
        if (animator != null)
        {
            animator.SetBool(happyAnimParam, true);
            Debug.Log(gameObject.name + " ���� ����������!");
        }
    }
}