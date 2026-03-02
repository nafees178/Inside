using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
public void PlayGame()
    {
        SceneManager.LoadScene("level1copy");
    }

public void QuitGame()
{
    Application.Quit();
    Debug.Log("Game Quit");
}
}