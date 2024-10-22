using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponsDealer : MonoBehaviour
{
	[SerializeField] GameObject weapon;
	[SerializeField] GameObject text;
	private string ogText;
	private GameObject player;
	private Transform playerTransform;
	[SerializeField] int cost;
	[SerializeField] int ammoCost;
	[SerializeField] Sprite spriteToShow;
	[SerializeField] GameObject spotToShow;
	private bool alreadyEquipped;
	[SerializeField] string weaponName;

	private void Start()
	{
		ogText = text.GetComponent<TextMeshProUGUI>().text.ToString();
		spotToShow.GetComponent<SpriteRenderer>().sprite = spriteToShow;
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

		if (Mathf.Abs(transform.position.x - playerTransform.position.x) <= 1)
		{
			text.SetActive(true);
		}
		else
		{
			text.SetActive(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			for (int i = 0; i < collision.GetComponent<Inventory>().currentWeapons.Count; i++)
			{
				if (collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().weaponName == weaponName)
				{
					alreadyEquipped = true;
					break;
				}
				else
				{
					alreadyEquipped = false;
				}
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E) &&
			collision.GetComponent<Inventory>().GetPoints() >= ammoCost && alreadyEquipped)
		{
			text.GetComponent<TextMeshProUGUI>().text = "Press E to Buy Ammo: 250";
			for (int i = 0; i < collision.GetComponent<Inventory>().currentWeapons.Count; i++)
			{
				if (collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().weaponName == weaponName)
				{
					if (collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().ammoReserve <
						collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().maxAmmoReserve)
					{
						collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().ammoReserve = collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().maxAmmoReserve;
						collision.GetComponent<Inventory>().AddPoints(-ammoCost);
						break;
					}
					break;
				}
			}
		}
		if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E) && !collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().reloading &&
			collision.GetComponent<Inventory>().GetPoints() >= cost && !alreadyEquipped)
		{
			text.GetComponent<TextMeshProUGUI>().text = ogText;
			if (collision.GetComponent<Inventory>().currentWeapons.Count <= 2)
			{
				collision.GetComponent<Inventory>().AddWeapon(weapon);
				collision.GetComponent<Inventory>().AddPoints(-cost);
				alreadyEquipped = true;
				return;
			}
			int secondaryCount = 0;
			if (collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponClass == WeaponBase.WeaponClass.SECONDARY)
			{
				if (weapon.GetComponent<WeaponBase>().weaponClass == WeaponBase.WeaponClass.SECONDARY)
				{
					collision.GetComponent<Inventory>().AddWeapon(weapon);
					collision.GetComponent<Inventory>().AddPoints(-cost);
					alreadyEquipped = true;
					return;
				}
				for (int i = 0; i < collision.GetComponent<Inventory>().currentWeapons.Count; i++)
				{
					if (collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().weaponClass == WeaponBase.WeaponClass.SECONDARY)
					{
						secondaryCount++;
					}
				}
				if (secondaryCount >= 2)
				{
					collision.GetComponent<Inventory>().AddWeapon(weapon);
					collision.GetComponent<Inventory>().AddPoints(-cost);
					alreadyEquipped = true;
					return;
				}
				else
				{
					return;
				}
			}
			else
			{
				collision.GetComponent<Inventory>().AddWeapon(weapon);
				collision.GetComponent<Inventory>().AddPoints(-cost);
				alreadyEquipped = true;
				return;
			}
		}
	}

	private void OnDestroy()
	{
		Destroy(text);
	}
}
