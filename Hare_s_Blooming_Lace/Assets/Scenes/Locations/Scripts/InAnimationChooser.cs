using UnityEngine;

public class InAnimationChooser : MonoBehaviour
{
    public Animator inAnimator;
    public string whiteOutAnimName = "WhiteOut";
    public string regularOutAnimName = "Out";

    private void Awake()
    {
        if (inAnimator == null)
        {
            Debug.LogError("Animator is not assigned in InAnimationChooser!");
            return;
        }
    }

    void Start()
    {
        // Проверяем, существует ли наш переносчик данных
        if (SceneDataCarrier.Instance != null)
        {
            // ✅ Получаем сохраненное значение
            bool isWhiteIn = SceneDataCarrier.Instance.isWhiteIn;

            if (isWhiteIn)
            {
                inAnimator.SetTrigger(whiteOutAnimName);
            }
            else
            {
                inAnimator.SetTrigger(regularOutAnimName);
            }


        }
        else
        {
            Debug.LogError("SceneDataCarrier instance not found. Playing default animation.");
            inAnimator.SetTrigger(regularOutAnimName);
        }

        SceneDataCarrier.Instance.isWhiteIn= false; // Сбрасываем значение после использования

    }
}