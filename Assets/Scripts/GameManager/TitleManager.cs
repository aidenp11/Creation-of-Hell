using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
	private void Start()
	{
        Cursor.visible = false;
	}

	public void OnStartButtonClick()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("SampleScene");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
