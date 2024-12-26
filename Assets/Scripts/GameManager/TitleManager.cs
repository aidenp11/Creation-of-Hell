using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
	[SerializeField] Texture2D cursor;
	[SerializeField] IntVariable skinNum;
	[SerializeField] GameObject skinmenu;

	[SerializeField] Image image;
	[SerializeField] Image imagearm;
	[SerializeField] Sprite miku;
	[SerializeField] Sprite mikuarm;
	[SerializeField] Sprite soldier;
	[SerializeField] Sprite soldierarm;
	private void Start()
	{
		if (skinNum == 0)
		{
			image.sprite = soldier;
			imagearm.sprite = soldierarm;
		} else if (skinNum == 1) 
		{
			image.sprite = miku;
			imagearm.sprite = mikuarm;
		}
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
		image.sprite = soldier;
		imagearm.sprite = soldierarm;
		skinNum.value = 0;
	}

	public void OnMikuButton()
	{
		image.sprite = miku;
		imagearm.sprite = mikuarm;	
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
