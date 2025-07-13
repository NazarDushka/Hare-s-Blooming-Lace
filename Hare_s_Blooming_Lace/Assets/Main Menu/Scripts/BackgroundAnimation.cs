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
            Debug.LogError("������: ��������� Animator �� ������ �� ������� " + gameObject.name + ". ����������, ���������� Animator ��� ��������� ��� � �������.");
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
            Debug.Log($"��������� ������������ �������� (������� ���) ����� {currentDelay:F2} ������.");

            yield return new WaitForSeconds(currentDelay);

            Debug.Log($"��������� �������� (������� ���): {animationTriggerName}");
            backgroundAnimator.SetTrigger(animationTriggerName);

            yield return new WaitForSeconds(animationDuration);
        }
    }
}