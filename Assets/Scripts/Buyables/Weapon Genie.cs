using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponGenie : MonoBehaviour
{
	[SerializeField] List<GameObject> weapons = new List<GameObject>();
	[SerializeField] List<GameObject> weaponsList = new List<GameObject>();
	private GameObject weapon;
	[SerializeField] GameObject text;
	private string ogText;
	private GameObject player;
	private Transform playerTransform;
	[SerializeField] int cost;
	private Sprite spriteToShow;
	[SerializeField] GameObject spotToShow;
	private bool alreadySpinning;

	private float timeToShow = 0;
	private bool spinning = false;

	[SerializeField] Animator animator;

	[SerializeField] AudioSource jingle;

	private int random;

	private void Start()
	{
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

		if (Mathf.Abs(transform.position.x - playerTransform.position.x) <= 2.25f)
		{
			animator.SetBool("PlayerClose", true);
			text.SetActive(true);
		}
		else if(!spinning && animator.GetBool("PlayerClose"))
		{
			animator.SetBool("PlayerClose", false);
			text.SetActive(false);
		}
		else
		{
			text.SetActive(false);
		}

		if (spinning && weapon == null)
		{
			timeToShow -= Time.deltaTime;
			if (timeToShow <= 0)
			{
				timeToShow = 0.25f;
				Sprite randomSprite = weapons.ElementAt(UnityEngine.Random.Range(0, weapons.Count)).GetComponent<SpriteRenderer>().sprite;
				spriteToShow = randomSprite;
				spotToShow.GetComponent<SpriteRenderer>().sprite = spriteToShow;
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E) &&
			collision.GetComponent<Inventory>().GetPoints() >= cost && !alreadySpinning)
		{
			weaponsList.Clear();
			text.GetComponent<TextMeshProUGUI>().text = "";
			player.GetComponent<Inventory>().AddPoints(-cost);
			jingle.Play();
			Invoke("GetRandomWeapon", 2.5f);
			Invoke("SwitchState", 9f);
			Invoke("SwitchAnimState", 9f);
			alreadySpinning = true;
			spinning = true;
		}
		if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E) && alreadySpinning && weapon != null &&
			!collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().reloading 
			&& collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().fireRateToUse <= 0)
		{
			if (collision.GetComponent<Inventory>().currentWeapons.Count <= 2)
			{
				collision.GetComponent<Inventory>().AddWeapon(weapon);
				text.GetComponent<TextMeshProUGUI>().text = "";
				weapon = null;
				spriteToShow = null;
				spinning = false;
				spotToShow.GetComponent<SpriteRenderer>().sprite = null;
				return;
			}
			int secondaryCount = 0;
			if (collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponClass == WeaponBase.WeaponClass.SECONDARY)
			{
				if (weapon.GetComponent<WeaponBase>().weaponClass == WeaponBase.WeaponClass.SECONDARY)
				{
					collision.GetComponent<Inventory>().AddWeapon(weapon);
					text.GetComponent<TextMeshProUGUI>().text = "";
					weapon = null;
					spriteToShow = null;
					spinning = false;
					spotToShow.GetComponent<SpriteRenderer>().sprite = null;
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
					text.GetComponent<TextMeshProUGUI>().text = "";
					weapon = null;
					spriteToShow = null;
					spinning = false;
					spotToShow.GetComponent<SpriteRenderer>().sprite = null;
					return;
				}
				else
				{

				}
			}
			else
			{
				collision.GetComponent<Inventory>().AddWeapon(weapon);
				text.GetComponent<TextMeshProUGUI>().text = "";
				weapon = null;
				spriteToShow = null;
				spinning = false;
				spotToShow.GetComponent<SpriteRenderer>().sprite = null;
				return;
			}
		}
	}

	private void GetRandomWeapon()
	{
		bool add = true;
		foreach (GameObject weapon in weapons)
		{
			add = true;
			foreach (GameObject equippedWeapon in player.GetComponent<Inventory>().currentWeapons)
			{
				if (weapon.GetComponent<WeaponBase>().weaponName == equippedWeapon.GetComponent<WeaponBase>().weaponName)
				{
					add = false;
					break;
				}
			}
			if (add)
			{
				for (int i = 0; i < weapon.GetComponent<WeaponBase>().genieChance; i++)
				{
					weaponsList.Add(weapon);
				}
			}
		}
		random = UnityEngine.Random.Range(0, weaponsList.Count);
		weapon = weaponsList.ElementAt(random);
		spriteToShow = weapon.GetComponent<SpriteRenderer>().sprite;
		spotToShow.GetComponent<SpriteRenderer>().sprite = spriteToShow;
		text.GetComponent<TextMeshProUGUI>().text = "Press E to pick up " + weapon.GetComponent<WeaponBase>().weaponName;
	}

	private void SwitchState()
	{
		if (alreadySpinning)
		{
			text.GetComponent<TextMeshProUGUI>().text = ogText;
			weapon = null;
			spriteToShow = null;
			spotToShow.GetComponent<SpriteRenderer>().sprite = null;
			alreadySpinning = false;
		}
	}

	private void SwitchAnimState()
	{
		if (spinning) spinning = false; 
	}
}
