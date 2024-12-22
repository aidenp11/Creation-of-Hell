using UnityEngine;

public class BossSpawn : MonoBehaviour
{
	[SerializeField] GameObject boss;
	[SerializeField] AudioSource bossMusic;
	[SerializeField] AudioSource bossLaugh;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			boss.SetActive(true);
			bossMusic.Play();
			bossLaugh.Play();
			Destroy(gameObject);
		}
	}
}
