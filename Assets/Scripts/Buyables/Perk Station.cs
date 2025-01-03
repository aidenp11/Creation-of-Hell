using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PerkStation : MonoBehaviour
{
	[SerializeField] GameObject text;
	private GameObject player;
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

	[SerializeField] AudioSource chaChing;

	private void Start()
	{
		text.SetActive(false);
		done = false;
		for (int i = 0; i < SceneManager.GetActiveScene().GetRootGameObjects().Length; i++)
		{
			if (SceneManager.GetActiveScene().GetRootGameObjects().ElementAt(i).GetComponent<PlayerMovement2D>())
			{
				player = SceneManager.GetActiveScene().GetRootGameObjects().ElementAt(i);
				break;
			}
		}
		switch(perkType)
		{
			case PerkType.SPEED:
				text.GetComponent<TextMeshProUGUI>().text = "Press E to buy Speed" + '\n' + "Cost: " + cost;
				break;
			case PerkType.HEALTH:
				text.GetComponent<TextMeshProUGUI>().text = "Press E to buy Health Milk" + '\n' + "Cost: " + cost;
				break;
			case PerkType.REGEN:
				text.GetComponent<TextMeshProUGUI>().text = "Press E to buy Regeneration Serum" + '\n' + "Cost: " + cost;
				break;
			case PerkType.PIERCE:
				text.GetComponent<TextMeshProUGUI>().text = "Press E to buy Sharper Bullets" + '\n' + "Cost: " + cost;
				break;
			case PerkType.NOEXPLOSIONDAMAGE:
				text.GetComponent<TextMeshProUGUI>().text = "Press E to buy Explosion Resistance" + '\n' + "Cost: " + cost;
				break;
			case PerkType.GAMBLER:
				text.GetComponent<TextMeshProUGUI>().text = "Press E to buy Gambler" + '\n' + "Cost: " + cost;
				break;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (text != null && collision.CompareTag("Player")) text.SetActive(true);
		if (collision.CompareTag("Player") && !done)
		{
			switch (perkType)
			{
				case PerkType.SPEED:
					if (Input.GetKey(KeyCode.E) && collision.GetComponent<Inventory>().GetPoints() >= cost)
					{
						chaChing.Play();
						collision.GetComponent<PlayerMovement2D>().speedPerk = true;
						collision.GetComponent<Inventory>().AddPoints(-cost);
						Destroy(text);
						Destroy(gameObject.GetComponent<Collider2D>(), 0.1f);
						done = true;
					}
					break;
				case PerkType.HEALTH:
					if (Input.GetKey(KeyCode.E) && collision.GetComponent<Inventory>().GetPoints() >= cost)
					{
						chaChing.Play();
						collision.GetComponent<Inventory>().healthPerk = true;
						collision.GetComponent<Inventory>().AddPoints(-cost);
						Destroy(text);
						Destroy(gameObject.GetComponent<Collider2D>(), 0.1f);
						done = true;
					}
					break;
				case PerkType.REGEN:
					if (Input.GetKey(KeyCode.E) && collision.GetComponent<Inventory>().GetPoints() >= cost)
					{
						chaChing.Play();
						collision.GetComponent<Inventory>().regenPerk = true;
						collision.GetComponent<Inventory>().AddPoints(-cost);
						Destroy(text);
						Destroy(gameObject.GetComponent<Collider2D>(), 0.1f);
						done = true;
					}
					break;
				case PerkType.PIERCE:
					if (Input.GetKey(KeyCode.E) && collision.GetComponent<Inventory>().GetPoints() >= cost)
					{
						chaChing.Play();
						collision.GetComponent<Inventory>().piercePerk = true;
						collision.GetComponent<Inventory>().AddPoints(-cost);
						Destroy(text);
						Destroy(gameObject.GetComponent<Collider2D>(), 0.1f);
						done = true;
					}
					break;
				case PerkType.NOEXPLOSIONDAMAGE:
					break;
				case PerkType.GAMBLER:
					if (Input.GetKey(KeyCode.E) && collision.GetComponent<Inventory>().GetPoints() >= cost)
					{
						chaChing.Play();
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

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (text != null && collision.CompareTag("Player")) text.SetActive(false);
	}
}
