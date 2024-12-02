using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
	[SerializeField] Texture2D cursor;
	private void Start()
	{
		Cursor.SetCursor(cursor, new Vector2(0, 50), CursorMode.ForceSoftware);
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
