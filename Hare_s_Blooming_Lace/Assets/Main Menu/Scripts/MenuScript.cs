using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{


    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    

    public void ExitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

}
