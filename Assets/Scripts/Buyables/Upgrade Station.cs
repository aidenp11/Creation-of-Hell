using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeStation : MonoBehaviour
{
	private int cost = 5000;
	private GameObject player;
	[SerializeField] GameObject text;
	private string ogText;
	private bool upgraded;
	private bool upgraded2;

	private bool upgrading = false;
	private bool upgrading2 = false;

	[SerializeField] AudioSource upgradeSound;
	[SerializeField] Animator animator;

	private void Start()
	{
		text.SetActive(false);
		upgraded = false;
		upgraded2 = false;
		ogText = text.GetComponent<TextMeshProUGUI>().text;
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
		if (upgraded)
		{
			player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded = true;
			upgraded = false;
		}
		else if (upgraded2)
		{
			player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded2 = true;
			upgraded2 = false;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == false)
		{
			text.GetComponent<TextMeshProUGUI>().text = ogText;
			text.SetActive(true);
		}
		else if (collision.CompareTag("Player") && collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded2 == false)
		{
			text.GetComponent<TextMeshProUGUI>().text = "Press E to upgrade again" + '\n' + "Cost: " + cost * 3;
			text.SetActive(true);
		}
		if (collision.CompareTag("Player") && collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == true &&
			collision.CompareTag("Player") && collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded2 == true)
		{
			text.GetComponent<TextMeshProUGUI>().text = "Weapon is already upgraded";
			text.SetActive(true);
		}
		if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E) &&
			!collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().reloading
			&& collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().fireRateToUse <= 0 &&
			collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == false
				&& collision.GetComponent<Inventory>().GetPoints() >= cost && !upgrading)
		{
			upgrading = true;
			Invoke("Set1", 2);
			Invoke("SetUpgradeTrue", 2);
			animator.SetTrigger("upgrade");
			upgradeSound.Play();
			collision.GetComponent<Inventory>().AddPoints(-cost);
		}
		else if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E) &&
			!collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().reloading
			&& collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().fireRateToUse <= 0 && 
			collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == true &&
			collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded2 == false
			&& collision.GetComponent<Inventory>().GetPoints() >= cost * 3 && !upgrading2)
		{
			upgrading2 = true;
			Invoke("Set2", 2);
			Invoke("SetUpgrade2True", 2);
			animator.SetTrigger("upgrade");
			upgradeSound.Play();
			collision.GetComponent<Inventory>().AddPoints(-cost * 3);
		}
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && text)
		{
			text.SetActive(false);
		}
	}

	private void SetUpgradeTrue()
	{
		upgraded = true;
	}

	private void SetUpgrade2True()
	{
		upgraded2 = true;
	}

	private void Set1()
	{
		upgrading = false;
	}

	private void Set2()
	{
		upgrading2 = false;
	}
}
