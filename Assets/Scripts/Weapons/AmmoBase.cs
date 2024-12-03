using System.Linq;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class AmmoBase : MonoBehaviour
{
	[SerializeField] int damage;
	public int damageToUse;
	[SerializeField] float bulletLifespan;
	[SerializeField] int pierce;
	private int piercePerkPierce;
	public int pierceToUse;
	[SerializeField][Range(0, 1)] float pierceDamageFalloff;
	[SerializeField] GameObject hitEffect;
	private GameObject destroyHitEffect;
	private GameObject destroyWallHitEffect;
	[SerializeField] GameObject wallHitEffect;
	private GameObject player;

	private float newDamage;

	private bool doneUpgrade = false;
	private bool doneUpgrade2 = false;

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
		if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == true && !doneUpgrade)
		{
			UpgradeBullet();
			doneUpgrade = true;
		}
		if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded2 == true && !doneUpgrade2)
		{
			UpgradeBullet();
			doneUpgrade2 = true;
		}
		if (player.GetComponent<Inventory>().piercePerk == true)
		{
			pierceToUse = piercePerkPierce;
		}
	}
	private void Update()
	{
		
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
			destroyWallHitEffect = Instantiate(wallHitEffect, transform.position, transform.rotation);
			Destroy(destroyWallHitEffect, 0.5f);
			Destroy(gameObject);
		}
		if (pierceToUse > 0 && collision.CompareTag("Enemy"))
		{
			if (player.GetComponent<Inventory>().gambler == true)
			{
				player.GetComponent<Inventory>().AddPoints(Random.Range(-15, 60));
			}
			else
			{
				player.GetComponent<Inventory>().AddPoints(10);
			}
			collision.GetComponent<EnemyBase>().hit.Play();
			destroyHitEffect = Instantiate(hitEffect, transform.position, transform.rotation);
			Destroy(destroyHitEffect, 0.5f);
			collision.GetComponent<EnemyBase>().ApplyDamage(damageToUse);
			pierceToUse--;
			newDamage = (float)damageToUse * pierceDamageFalloff;
			damageToUse = (int)newDamage;
		}
	}

	private void UpgradeBullet()
	{
		if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded && !player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded2)
		{
			GetComponent<SpriteRenderer>().color = new Color(1 * 2, 0.9109956f * 2, 0.4371068f * 2);
		}
		else
		{
			GetComponent<SpriteRenderer>().color = new Color(1.5f, 0, 2.3f);
		}
		if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.SEMIAUTO
			&& player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().shotgun == false)
		{
			damageToUse = (int)((float)damageToUse * 2f);
			pierceToUse = (int)((float)pierceToUse * 2f);
			piercePerkPierce = pierceToUse + 3;
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.SEMIAUTO
			&& player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().shotgun == true)
		{
			damageToUse = (int)((float)damageToUse * 1.75f);
			pierceToUse = pierceToUse + 2;
			piercePerkPierce = pierceToUse + 3;
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.FULLAUTO
			&& player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().shotgun == false)
		{
			damageToUse = (int)((float)damageToUse * 1.5f);
			pierceToUse = (int)((float)pierceToUse * 1.5f);
			piercePerkPierce = pierceToUse + 3;
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.FULLAUTO
			&& player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().shotgun == true)
		{
			damageToUse = (int)((float)damageToUse * 2.5f);
			pierceToUse = pierceToUse + 3;
			piercePerkPierce = pierceToUse + 3;
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.BURST)
		{
			damageToUse = (int)((float)damageToUse * 2.25f);
			pierceToUse = pierceToUse + 2;
			piercePerkPierce = pierceToUse + 3;
		}
	}
}
