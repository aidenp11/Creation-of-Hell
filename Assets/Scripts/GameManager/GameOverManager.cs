using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void OnTitleButton()
    {
        SceneManager.LoadScene("Title");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
