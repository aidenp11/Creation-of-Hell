using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerkStation : MonoBehaviour
{
	[SerializeField] GameObject text;
	private GameObject player;
	private Transform playerTransform;
	[SerializeField] int cost;
	private bool done;

	public enum PerkType
	{
		SPEED,
		HEALTH,
		REGEN,
		PIERCE,
		NOEXPLOSIONDAMAGE,
		GAMBLER
	}
	[SerializeField] PerkType perkType;

	private void Start()
	{
		done = false;
		for (int i = 0; i < SceneManager.GetActiveScene().GetRootGameObjects().Length; i++)
		{
			if (SceneManager.GetActiveScene().GetRootGameObjects().ElementAt(i).GetComponent<PlayerMovement2D>())
			{
				player = SceneManager.GetActiveScene().GetRootGameObjects().ElementAt(i);
				break;
			}
		}
	}

	private void Update()
	{
		playerTransform = player.GetComponent<Transform>();

		if (Mathf.Abs(transform.position.x - playerTransform.position.x) <= 1 && text != null)
		{
			text.SetActive(true);
		}
		else if (text != null)
		{
			text.SetActive(false);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !done)
		{
			switch (perkType)
			{
				case PerkType.SPEED:
					break;
				case PerkType.HEALTH:
					break;
				case PerkType.REGEN:
					break;
				case PerkType.PIERCE:
					break;
				case PerkType.NOEXPLOSIONDAMAGE:
					break;
				case PerkType.GAMBLER:
					if (Input.GetKeyDown(KeyCode.E) && collision.GetComponent<Inventory>().GetPoints() >= cost)
					{
						collision.GetComponent<Inventory>().gambler = true;
						collision.GetComponent<Inventory>().AddPoints(-cost);
						Destroy(text);
						Destroy(gameObject.GetComponent<Collider2D>(), 0.1f);
						done = true;
					}
					break;
			}
		}
	}
}
