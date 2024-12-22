using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] IntVariable playerHealth;

	[SerializeField] GameObject wave;

	private float timer = 0;
	[SerializeField] GameObject TimerText;

	[SerializeField] FloatVariable finalTime;
	[SerializeField] IntVariable enemiesKilled;
	[SerializeField] IntVariable mmfsKilled;
	[SerializeField] IntVariable jumpsterKilleds;
	[SerializeField] IntVariable huggyKilled;
	[SerializeField] IntVariable round;
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
		timer += Time.deltaTime;
		TimeSpan t = TimeSpan.FromSeconds(timer);

		string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
						t.Hours,
						t.Minutes,
						t.Seconds,
						t.Milliseconds);
		TimerText.GetComponent<TextMeshProUGUI>().text = answer;
		if (Input.GetKeyDown(KeyCode.P) && !player.GetComponent<Inventory>().invbool && !player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().reloading && player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().fireRateToUse <= 0)
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
				enemiesKilled.value = player.GetComponent<Inventory>().totalKilled;
				mmfsKilled.value = player.GetComponent<Inventory>().mmfKilled;
				jumpsterKilleds.value = player.GetComponent<Inventory>().jumpsterKilled;
				huggyKilled.value = player.GetComponent<Inventory>().huggyBearKilled;
				round.value = wave.GetComponent<Waves>().roundNumber;
				finalTime.value = timer;
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
