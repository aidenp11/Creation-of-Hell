using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmmoBox : MonoBehaviour
{
	[SerializeField] GameObject text;
	private string ogText;
	private GameObject player;
	private Transform playerTransform;
	[SerializeField] int cost;
	private int costToUse;

	[SerializeField] AudioSource chaChing;

	private void Start()
	{
		costToUse = cost;
		ogText = text.GetComponent<TextMeshProUGUI>().text.ToString();
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

		if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == true && player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded2 == false)
		{
			costToUse = 1750;
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == true && player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded2 == true)
		{
			costToUse = 2500;
		}
		else costToUse = cost;

		if (Mathf.Abs(transform.position.x - playerTransform.position.x) <= 1)
		{
			text.SetActive(true);
		}
		else
		{
			text.SetActive(false);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		text.GetComponent<TextMeshProUGUI>().text = "Press E to Buy Ammo" + '\n' + "Cost: " + costToUse;

		if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E) && collision.GetComponent<Inventory>().GetPoints() >= costToUse)
		{
			if (collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().ammoReserve <
						collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().maxAmmoReserve)
			{
				chaChing.Play();
				collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().ammoReserve = collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().maxAmmoReserve;
				collision.GetComponent<Inventory>().AddPoints(-costToUse);
			}
		}
	}
}
