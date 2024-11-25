using System.Linq;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class ExplosionAmmo : MonoBehaviour
{
	[SerializeField] int damage;
	private int damageToUse;
	[SerializeField] float bulletLifespan;
	[SerializeField] int pierce;
	private int piercePerkPierce;
	private int pierceToUse;
	[SerializeField][Range(0, 1)] float pierceDamageFalloff;
	[SerializeField] GameObject hitEffect;
	private GameObject destroyHitEffect;
	//[SerializeField] GameObject wallHitEffect;
	private GameObject player;

	private float newDamage;

	private bool doneUpgrade = false;

	private void Start()
	{
		piercePerkPierce = pierce + 3;
		pierceToUse = pierce;
		damageToUse = damage;
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
		if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == true && !doneUpgrade)
		{
			UpgradeExplosionBullet();
			doneUpgrade = true;
		}
		if (player.GetComponent<Inventory>().piercePerk == true)
		{
			pierceToUse = piercePerkPierce;
		}
		bulletLifespan -= Time.deltaTime;
		if (pierceToUse <= 0)
		{
			Destroy(gameObject);
		}
		if (bulletLifespan < 0)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Wall"))
		{
			//Instantiate(wallHitEffect, collision.transform);
			Destroy(gameObject);
		}
		if (pierceToUse > 0 && collision.CompareTag("Enemy"))
		{
			if (player.GetComponent<Inventory>().gambler == true)
			{
				player.GetComponent<Inventory>().AddPoints(Random.Range(-10, 30));
			}
			else
			{
				player.GetComponent<Inventory>().AddPoints(5);
			}
			destroyHitEffect = Instantiate(hitEffect, transform.position, transform.rotation);
			Destroy(destroyHitEffect, 0.5f);
			collision.GetComponent<EnemyBase>().ApplyDamage(damageToUse);
			pierceToUse--;
			newDamage = (float)damageToUse * pierceDamageFalloff;
			damageToUse = (int)newDamage;
		}
	}

	private void UpgradeExplosionBullet()
	{
		if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.SEMIAUTO
			&& player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().shotgun == false)
		{
			damageToUse = (int)((float)damage * 2.5f);
			pierceToUse = (int)((float)pierce * 3.5f);
			piercePerkPierce = pierceToUse + 3;
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.SEMIAUTO
			&& player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().shotgun == true)
		{
			damageToUse = (int)((float)damage * 2.75f);
			pierceToUse = pierce + 3;
			piercePerkPierce = pierceToUse + 3;
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.FULLAUTO
			&& player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().shotgun == false)
		{
			damageToUse = (int)((float)damage * 2.25f);
			pierceToUse = (int)((float)pierce * 2.25f);
			piercePerkPierce = pierceToUse + 3;
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.FULLAUTO
			&& player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().shotgun == true)
		{
			damageToUse = (int)((float)damage * 3.5f);
			pierceToUse = pierce + 4;
			piercePerkPierce = pierceToUse + 3;
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.BURST)
		{
			damageToUse = (int)((float)damage * 3.25f);
			pierceToUse = pierce + 2;
			piercePerkPierce = pierceToUse + 3;
		}
	}
}
