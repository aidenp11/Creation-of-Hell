using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
	[SerializeField] FloatVariable finalTime;
	[SerializeField] IntVariable enemiesKilled;
	[SerializeField] IntVariable mmfsKilled;
	[SerializeField] IntVariable jumpsterKilleds;
	[SerializeField] IntVariable huggyKilled;
	[SerializeField] IntVariable round;

	[SerializeField] GameObject ftText;
	[SerializeField] GameObject ekText;
	[SerializeField] GameObject mmfkText;
	[SerializeField] GameObject jkText;
	[SerializeField] GameObject hkText;
	[SerializeField] GameObject rText;
	private void Start()
	{
		TimeSpan t = TimeSpan.FromSeconds(finalTime.value);

		string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
						t.Hours,
						t.Minutes,
						t.Seconds,
						t.Milliseconds);
		ftText.GetComponent<TextMeshProUGUI>().text = "Final Time: " + answer;
		ekText.GetComponent<TextMeshProUGUI>().text = "Total Enemies Killed: " + enemiesKilled.value.ToString();
		mmfkText.GetComponent<TextMeshProUGUI>().text = "Mutant Man Fish Killed: " + mmfsKilled.value.ToString();
		jkText.GetComponent<TextMeshProUGUI>().text = "Jumpster Killed: " + jumpsterKilleds.value.ToString();
		hkText.GetComponent<TextMeshProUGUI>().text = "Huggy Bear Killed: " + huggyKilled.value.ToString();
		rText.GetComponent<TextMeshProUGUI>().text = "Final Round: " + round.value.ToString();
	}
	public void OnTitleButton()
    {
        SceneManager.LoadScene("Title");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
