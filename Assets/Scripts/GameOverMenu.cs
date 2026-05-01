
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Arena");
    }
    public void Quit()
    {
        Application.Quit();
    }
}