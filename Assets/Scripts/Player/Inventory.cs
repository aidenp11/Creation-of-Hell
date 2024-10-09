using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[Header("Weapons")]
	[SerializeField] GameObject startingWeapon;
	[SerializeField] Transform handPosition;
	public GameObject activeWeapon;
	public List<GameObject> currentWeapons = new List<GameObject>();

	[Header("Other Stuff")]
	[SerializeField] int score;

	private void Start()
	{
		activeWeapon = Instantiate(startingWeapon, handPosition);
		currentWeapons.Add(activeWeapon);
	}

	private void Update()
	{
		if (currentWeapons.Count == 1)
		{

		}
		else if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if (currentWeapons.IndexOf(activeWeapon) == currentWeapons.Count - 1)
			{
				activeWeapon.SetActive(false);
				activeWeapon = currentWeapons.ElementAt(0);
				activeWeapon.SetActive(true);
			}
			else
			{
				for (int i = 0; i < currentWeapons.Count; i++)
				{
					if (currentWeapons.ElementAt(i) == activeWeapon)
					{
						activeWeapon.SetActive(false);
						activeWeapon = currentWeapons.ElementAt(i + 1);
						activeWeapon.SetActive(true);
						break;
					}
				}
			}
		}
		else if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			if (currentWeapons.IndexOf(activeWeapon) == 0)
			{
				activeWeapon.SetActive(false);
				activeWeapon = currentWeapons.ElementAt(currentWeapons.Count - 1);
				activeWeapon.SetActive(true);
			}
			else
			{
				for (int i = 0; i < currentWeapons.Count; i++)
				{
					if (currentWeapons.ElementAt(i) == activeWeapon)
					{
						activeWeapon.SetActive(false);
						activeWeapon = currentWeapons.ElementAt(i - 1);
						activeWeapon.SetActive(true);
						break;
					}
				}
			}
		}
	}

	public void AddWeapon(GameObject weaponToAdd)
	{
		if (currentWeapons.Count < 3)
		{
			activeWeapon.SetActive(false);
			activeWeapon = Instantiate(weaponToAdd, handPosition);
			currentWeapons.Add(activeWeapon);
		}
		else if (weaponToAdd.GetComponent<WeaponBase>().weaponClass == WeaponBase.WeaponClass.SECONDARY)
		{
			activeWeapon.SetActive(false);
			int index = currentWeapons.IndexOf(activeWeapon);
			currentWeapons.RemoveAt(index);
			Destroy(activeWeapon);
			activeWeapon = Instantiate(weaponToAdd, handPosition);
			currentWeapons.Insert(index, activeWeapon);
		}
		else if (weaponToAdd.GetComponent<WeaponBase>().weaponClass == WeaponBase.WeaponClass.PRIMARY)
		{
			int secondaryCount = 0;
			for (int i = 0; i < currentWeapons.Count; i++)
			{
				if (currentWeapons.ElementAt(i).GetComponent<WeaponBase>().weaponClass == WeaponBase.WeaponClass.SECONDARY)
				{
					secondaryCount++;
				}
			}
			if (secondaryCount >= 2)
			{
				activeWeapon.SetActive(false);
				int index = currentWeapons.IndexOf(activeWeapon);
				currentWeapons.RemoveAt(index);
				Destroy(activeWeapon);
				activeWeapon = Instantiate(weaponToAdd, handPosition);
				currentWeapons.Insert(index, activeWeapon);
			}
			else
			{
				activeWeapon.SetActive(false);
				int index = currentWeapons.IndexOf(activeWeapon);
				currentWeapons.RemoveAt(index);
				Destroy(activeWeapon);
				activeWeapon = Instantiate(weaponToAdd, handPosition);
				currentWeapons.Insert(index, activeWeapon);
			}
		}
	}
}
