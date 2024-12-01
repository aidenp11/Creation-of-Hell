using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
	public void OnStartButtonClick()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void OnQuitButtonClick()
	{
		Application.Quit();
	}
}
