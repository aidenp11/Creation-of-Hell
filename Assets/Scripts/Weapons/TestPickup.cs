using System.Linq;
using UnityEngine;

public class TestPickup : MonoBehaviour
{
	[SerializeField] GameObject weapon;
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E))
		{
			if (collision.GetComponent<Inventory>().currentWeapons.Count <= 2)
			{
				collision.GetComponent<Inventory>().AddWeapon(weapon);
				Destroy(gameObject);
				return;
			}
			int secondaryCount = 0;
			if (collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponClass == WeaponBase.WeaponClass.SECONDARY)
			{
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
					Destroy(gameObject);
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
				Destroy(gameObject);
				return;
			}
		}
	}
}
