using UnityEngine;
using System.Collections; 

public class BackgroundAnimation : MonoBehaviour
{
    
    public Animator backgroundAnimator;

    
    public float minDelayBetweenPlays = 5f; 

    
    public float maxDelayBetweenPlays = 15f; 

    
    public float animationDuration = 0.267f; 

    
    public string animationTriggerName = "PlayAnimation";

   
    private Coroutine playAnimationCoroutine;

    void OnEnable()
    {
        
        if (backgroundAnimator == null)
        {
            backgroundAnimator = GetComponent<Animator>();
        }

        
        if (backgroundAnimator == null)
        {
            Debug.LogError("Ошибка: Компонент Animator не найден на объекте " + gameObject.name + ". Пожалуйста, прикрепите Animator или назначьте его в скрипте.");
            return;
        }

        
        if (playAnimationCoroutine != null)
        {
            StopCoroutine(playAnimationCoroutine);
        }

        
        playAnimationCoroutine = StartCoroutine(PlayAnimationRandomlyRoutine());
    }

   
    void OnDisable()
    {
        
        if (playAnimationCoroutine != null)
        {
            StopCoroutine(playAnimationCoroutine);
            playAnimationCoroutine = null; 
        }
    }

    IEnumerator PlayAnimationRandomlyRoutine()
    {
        
        while (true)
        {
            float currentDelay = Random.Range(minDelayBetweenPlays, maxDelayBetweenPlays);
            Debug.Log($"Следующее проигрывание анимации (обычный фон) через {currentDelay:F2} секунд.");

            yield return new WaitForSeconds(currentDelay);

            Debug.Log($"Запускаем анимацию (обычный фон): {animationTriggerName}");
            backgroundAnimator.SetTrigger(animationTriggerName);

            yield return new WaitForSeconds(animationDuration);
        }
    }
}