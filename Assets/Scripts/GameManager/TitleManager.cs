using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
	private void Start()
	{
		Time.timeScale = 1;	
	}

	public void OnStartButtonClick()
	{
		SceneManager.LoadScene("SampleScene");
	}

	public void OnQuitButtonClick()
	{
		Application.Quit();
	}
}
