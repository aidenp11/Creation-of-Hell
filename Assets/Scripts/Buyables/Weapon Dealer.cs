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
	private int ammoCostToUse;
	[SerializeField] Sprite spriteToShow;
	[SerializeField] GameObject spotToShow;
	private bool alreadyEquipped;
	[SerializeField] string weaponName;

	private void Start()
	{
		ammoCostToUse = ammoCost;
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
				if (collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().weaponName == weaponName
					&& collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().upgraded == false)
				{
					alreadyEquipped = true;
					ammoCostToUse = ammoCost;
					text.GetComponent<TextMeshProUGUI>().text = "Press E to Buy Ammo: " + ammoCostToUse;
					break;
				}
				else if (collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().weaponName == weaponName
					&& collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().upgraded == true)
				{
					alreadyEquipped = true;
					ammoCostToUse = ammoCost * 4;
					text.GetComponent<TextMeshProUGUI>().text = "Press E to Buy Ammo: " + ammoCostToUse;
					break;
				}
				else
				{
					alreadyEquipped = false;
					text.GetComponent<TextMeshProUGUI>().text = ogText;
				}
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E) &&
			collision.GetComponent<Inventory>().GetPoints() >= ammoCostToUse && alreadyEquipped)
		{
			text.GetComponent<TextMeshProUGUI>().text = "Press E to Buy Ammo: " + ammoCostToUse;
			for (int i = 0; i < collision.GetComponent<Inventory>().currentWeapons.Count; i++)
			{
				if (collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().weaponName == weaponName)
				{
					if (collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().ammoReserve <
						collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().maxAmmoReserve)
					{
						collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().ammoReserve = collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().maxAmmoReserve;
						collision.GetComponent<Inventory>().AddPoints(-ammoCostToUse);
						break;
					}
					break;
				}
			}
		}
		if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E) && !collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().reloading &&
			collision.GetComponent<Inventory>().GetPoints() >= cost && !alreadyEquipped && collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().fireRateToUse <= 0)
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
}
