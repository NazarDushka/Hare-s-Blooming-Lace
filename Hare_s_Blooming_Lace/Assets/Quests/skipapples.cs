using System.Xml;
using UnityEngine;

public class skipapples : MonoBehaviour
{
  

    public static int applesCount = 0;
    private bool playerInRange = false;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            applesCount=9;
            Debug.Log($"������ �������! ����� �����: {applesCount}");

           

            // ���������� ������������ ������
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                // ���� � ������� ��� ��������, ���������� ��� ������
                Destroy(gameObject);
            }
        }
    }
}
