using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] IntVariable playerHealth;
	public enum State
	{
		RUNNING,
		PAUSE,
		GAMEOVER
	}

	[SerializeField] GameObject pauseMenu;
	[SerializeField] GameObject player;

	public State state = State.RUNNING;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.P) && !player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().reloading && player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().fireRateToUse <= 0)
		{
			state = State.PAUSE;
		}

		if (playerHealth <= 0)
		{
			state = State.GAMEOVER;
		}

		switch (state)
		{
			case State.RUNNING:
				player.SetActive(true);
				pauseMenu.SetActive(false);
				Time.timeScale = 1;
				break;
			case State.PAUSE:
				player.SetActive(false);	
				pauseMenu.SetActive(true);
				Time.timeScale = 0;
				break;
			case State.GAMEOVER:
				SceneManager.LoadScene("GameOver");
				break;
		}
	}

	public void OnPauseQuitButton()
	{
		SceneManager.LoadScene("Title");
	}

	public void OnResumeButton()
	{
		state = State.RUNNING;
	}
}
