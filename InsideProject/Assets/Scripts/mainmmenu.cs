using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    [SerializeField] string levelName;

    public void PlayGame()
        {
            SceneManager.LoadScene(levelName);
        }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}