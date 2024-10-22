using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPickup : MonoBehaviour
{
	[SerializeField] GameObject weapon;
	[SerializeField] GameObject text;
	private GameObject player;
	private Transform playerTransform;

	private void Start()
	{
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

		if (Mathf.Abs(transform.position.x - playerTransform.position.x) <= 1.5)
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
		if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E) && !collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().reloading)
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
				if (weapon.GetComponent<WeaponBase>().weaponClass == WeaponBase.WeaponClass.SECONDARY)
				{
					collision.GetComponent<Inventory>().AddWeapon(weapon);
					Destroy(gameObject);
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

	private void OnDestroy()
	{
		Destroy(text);
	}
}
