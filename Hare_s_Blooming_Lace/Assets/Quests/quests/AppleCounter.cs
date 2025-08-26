using TMPro;
using UnityEngine;

public class AppleCounter : MonoBehaviour
{
    public TextMeshProUGUI appleText;
    [Tooltip("Объект счетчика")]
    public GameObject Counter;
    // ID квеста, который должен активировать счетчик
    private const int targetQuestID = 2;

    private void Start()
    {
        if (Counter != null)
        {
            Counter.SetActive(false);
        }

        if (appleText == null)
        {
            Debug.LogError("Apple Text (TextMeshPro) не назначен! Пожалуйста, прикрепите его в инспекторе.");
            enabled = false;
            return;
        }

        UpdateAppleCountText();
    }

    private void Update()
    {
        if (QuestManager.instance == null)
        {
            if (Counter != null)
            {
                Counter.SetActive(false);
            }
            return;
        }

        Quest questToVerify = QuestManager.instance.GetQuestById(targetQuestID);

        if (Counter == null)
        {
            return;
        }

        if (questToVerify != null)
        {
            if (questToVerify.isCompleted)
            {
                Destroy(Counter);
                return;
            }
            else if (questToVerify.isActive)
            {
                if (!Counter.activeSelf)
                {
                    Counter.SetActive(true);
                }
            }
            else
            {
                if (Counter.activeSelf)
                {
                    Counter.SetActive(false);
                }
            }
        }
        else
        {
            if (Counter.activeSelf)
            {
                Counter.SetActive(false);
            }
        }

        UpdateAppleCountText();
    }

    private void UpdateAppleCountText()
    {
        if (QuestManager.instance != null)
        {
            appleText.text = QuestManager.instance.collectedObjects.Count + "/9";
        }
        else
        {
            appleText.text = "0/9";
        }
    }
}