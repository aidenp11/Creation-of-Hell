using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
	[SerializeField] GameObject player;
	[SerializeField] GameObject wave;

	[SerializeField] GameObject boss;
	[SerializeField] AudioSource bossMusic;

	private float timer = 0;

	[SerializeField] GameObject text;

	[SerializeField] FloatVariable finalTime;
	[SerializeField] IntVariable enemiesKilled;
	[SerializeField] IntVariable mmfsKilled;
	[SerializeField] IntVariable jumpsterKilleds;
	[SerializeField] IntVariable huggyKilled;
	[SerializeField] IntVariable round;


	private void Update()
	{
		timer += Time.deltaTime;
		if (boss.IsDestroyed())
		{
			bossMusic.Stop();
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		text.SetActive(true);

		if (boss.IsDestroyed())
		{
			text.GetComponent<TextMeshProUGUI>().text = "Press E to exit the lab and Win";
		}

		if (collision.CompareTag("Player") && boss.IsDestroyed() && Input.GetKeyDown(KeyCode.E))
		{
			enemiesKilled.value = player.GetComponent<Inventory>().totalKilled;
			mmfsKilled.value = player.GetComponent<Inventory>().mmfKilled;
			jumpsterKilleds.value = player.GetComponent<Inventory>().jumpsterKilled;
			huggyKilled.value = player.GetComponent<Inventory>().huggyBearKilled;
			round.value = wave.GetComponent<Waves>().roundNumber;
			finalTime.value = timer;
			SceneManager.LoadScene("Victory");
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		text.SetActive(false);
	}

}
