using System.Linq;
using TMPro;
using UnityEngine;

public class LabGunCreationTable : MonoBehaviour
{
	[SerializeField] GameObject labGun;
	[SerializeField] GameObject text;

	[SerializeField] GameObject labGunDisplay;

	private bool created = false;

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && created)
		{
			for (int i = 0; i < collision.GetComponent<Inventory>().currentWeapons.Count; i++)
			{
				if (collision.GetComponent<Inventory>().currentWeapons.ElementAt(i).GetComponent<WeaponBase>().weaponName == "Lab Gun")
				{
					text.SetActive(false);
					break;
				}
				else
				{
					text.GetComponent<TextMeshProUGUI>().text = "Press E to pickup Lab Gun";
					text.SetActive(true);
					if (Input.GetKey(KeyCode.E) && !collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().reloading &&
					collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().fireRateToUse <= 0)
					{
						collision.GetComponent<Inventory>().AddWeapon(labGun);
					}
				}
			}
		} else if (collision.CompareTag("Player") && !created)
		{
			if (collision.GetComponent<Inventory>().part1 &&  collision.GetComponent<Inventory>().part2 && collision.GetComponent<Inventory>().part3)
			{
				text.GetComponent<TextMeshProUGUI>().text = "Press E to create Lab Gun";
				text.SetActive(true);
				if (Input.GetKey(KeyCode.E))
				{
					labGunDisplay.SetActive(true);
					created = true;
				}
			}
			else
			{
				text.GetComponent<TextMeshProUGUI>().text = "You still need more Lab Gun Parts";
				text.SetActive(true);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			text.SetActive(false);
		}
	}
}
