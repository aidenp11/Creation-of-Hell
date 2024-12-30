using System.Linq;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class ExplosionAmmo : MonoBehaviour
{
	[SerializeField] GameObject explosion;
	[SerializeField] int damage;
	public int damageToUse;
	[SerializeField] float bulletLifespan;
	[SerializeField] GameObject explosionEffect;
	private GameObject destroyExplosionEffect;
	private GameObject explosionPrefab;
	private GameObject player;

	private int pierce = 1;

	public bool piercePerk;

	private bool doneUpgrade = false;
	private bool doneUpgrade2 = false;

	public bool upgrade1;
	public bool upgrade2;

	private void Start()
	{
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
			upgrade1 = true;
			doneUpgrade = true;
		}
		if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded2 == true && !doneUpgrade2)
		{
			UpgradeBullet();
			upgrade2 = true;
			doneUpgrade2 = true;
		}
		if (player.GetComponent<Inventory>().piercePerk == true)
		{
			piercePerk = true;
		}
	}
	private void Update()
	{

		bulletLifespan -= Time.deltaTime;
		if (bulletLifespan < 0)
		{
			Instantiate(explosion, transform.position, transform.rotation);
			destroyExplosionEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
			Destroy(destroyExplosionEffect, 0.2f);
			Destroy(gameObject);
		}
		if (pierce <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Wall"))
		{
			Instantiate(explosion, transform.position, transform.rotation);
			destroyExplosionEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
			Destroy(destroyExplosionEffect, 0.2f);
			Destroy(gameObject);
		}
		if (pierce > 0 && collision.CompareTag("Enemy") && collision.GetComponent<EnemyBase>().Health >= 0)
		{
			pierce--;
			if (player.GetComponent<Inventory>().gambler == true)
			{
				player.GetComponent<Inventory>().AddPoints(Random.Range(-15, 20));
			}
			else
			{
				player.GetComponent<Inventory>().AddPoints(10);
			}
			Instantiate(explosion, transform.position, transform.rotation);
			collision.GetComponent<EnemyBase>().hit.Play();
			destroyExplosionEffect = Instantiate(explosionEffect, transform.position, transform.rotation);
			Destroy(destroyExplosionEffect, 0.2f);
			collision.GetComponent<EnemyBase>().ApplyDamage(damageToUse);
			Destroy(gameObject);
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
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.SEMIAUTO
			&& player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().shotgun == true)
		{

			damageToUse = (int)((float)damageToUse * 1.75f);
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.FULLAUTO
			&& player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().shotgun == false)
		{

			damageToUse = (int)((float)damageToUse * 1.85f);
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.FULLAUTO
			&& player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().shotgun == true)
		{
			damageToUse = (int)((float)damageToUse * 2.25f);
		}
		else if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().weaponType == WeaponBase.WeaponType.BURST)
		{
			damageToUse = (int)((float)damageToUse * 3.5f);
		}
	}
}
