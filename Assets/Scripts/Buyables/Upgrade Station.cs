using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeStation : MonoBehaviour
{
    private int cost = 5000;
    private GameObject player;
	[SerializeField] GameObject text;
	private string ogText;

	private void Start()
	{
		text.SetActive(false);
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

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == false)
		{
			text.GetComponent<TextMeshProUGUI>().text = ogText;
			text.SetActive(true);
		}
		if (collision.CompareTag("Player") && collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == true)
		{
			text.GetComponent<TextMeshProUGUI>().text = "Weapon is already upgraded";
			text.SetActive(true);
		}
		if (collision.CompareTag("Player") && Input.GetKey(KeyCode.E) &&
			!collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().reloading
			&& collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().fireRateToUse <= 0)
		{
			if (collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == false
				&& collision.GetComponent<Inventory>().GetPoints() >= cost)
			{
				collision.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded = true;
				collision.GetComponent<Inventory>().AddPoints(-cost);
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
