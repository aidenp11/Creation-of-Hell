using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
	[SerializeField] Texture2D cursor;
	[SerializeField] IntVariable skinNum;
	[SerializeField] GameObject skinmenu;
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

	public void OnSoldierButton()
	{
		skinNum.value = 0;
	}

	public void OnMikuButton()
	{
		skinNum.value = 1;
	}

	public void OnBackButton()
	{
		skinmenu.SetActive(false);
	}

	public void OnSkinButton()
	{
		skinmenu.SetActive(true);
	}
}
