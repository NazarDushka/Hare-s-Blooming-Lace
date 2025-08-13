using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{

    public static string NextScene = "Under the tree (start)";
    public bool shouldTeleport = false;
    public Vector2 targetPosition;
    public void StartGame()
    {
        LocationLoader.Load(NextScene, shouldTeleport, targetPosition);
    }
    

    public void ExitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

}
