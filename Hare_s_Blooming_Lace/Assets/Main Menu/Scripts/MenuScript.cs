using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public string LoadingScene;
    public static string NextScene = "Under the tree (start)";
    public bool shouldTeleport = false;
    public Vector2 targetPosition;
    public float animationDuration = 3.0f; // ѕродолжительность анимации перехода

    Animator anim;


    private void Awake()
    {
        Cursor.visible = true;
        anim = GameObject.Find("Canvas").GetComponent<Animator>();
    }


    public void StartGame()
    {
        // «апускаем анимацию перехода
        anim.SetTrigger("Start");
        QuestManager.instance.SetActiveQuest(0); // јктивируем квест с ID 0
        Cursor.visible = false;
        // «агружаем сцену с задержкой, чтобы анимаци€ успела проигратьс€
        Invoke("LoadNextScene", animationDuration);
    }

    private void LoadNextScene()
    {
        // ƒобавлен недостающий аргумент transitionOut (передайте нужное значение, например, пустую строку или им€ клипа)
        LocationLoader.Load(LoadingScene, NextScene, shouldTeleport, targetPosition);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}