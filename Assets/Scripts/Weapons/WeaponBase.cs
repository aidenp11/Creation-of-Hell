using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBase : MonoBehaviour
{
	[Header("Bullet Data")]
	[SerializeField] float bulletVelocity;
	[SerializeField] float bulletDrop;
	[SerializeField] GameObject bulletPrefab;

	[Header("Weapon Data")]
	[SerializeField] float reloadSpeed;
	[SerializeField] float bloom;
	[SerializeField] float fireRate;
	[SerializeField] int ammoCapacity;
	private int maxAmmoCapacity;
	[SerializeField] int ammoReserve;
	private bool reloading = false;
	private float originalFireRate;
	[SerializeField]
	enum WeaponType
	{
		SEMIAUTO,
		FULLAUTO,
		BURST
	}

	[SerializeField] WeaponType weaponType = WeaponType.SEMIAUTO;

	[SerializeField]
	public enum WeaponClass
	{
		PRIMARY,
		SECONDARY
	}

	[SerializeField] public WeaponClass weaponClass = WeaponClass.PRIMARY;

	[Header("Shotgun Stuff")]
	[SerializeField] bool shotgun = false;
	[SerializeField] int numPellets;
	[SerializeField] float horizontalSpread;

	[Header("Burst Stuff")]
	[SerializeField] float burstFireRate;
	[SerializeField] int burstCount;

	private GameObject bullet;

	[Header("Other Stuff")]
	[SerializeField] Transform muzzleTransform;

	private void Start()
	{
		originalFireRate = fireRate;
		fireRate = 0;
		maxAmmoCapacity = ammoCapacity;
	}

	private void Update()
	{
		if (ammoCapacity <= 0 && !reloading)
		{
			reloading = true;
			Invoke("Reload", reloadSpeed);
		}
		if (Input.GetKeyDown(KeyCode.R) && ammoCapacity < maxAmmoCapacity && !reloading)
		{
			reloading = true;
			Invoke("Reload", reloadSpeed);
		}
		switch (weaponType)
		{
			case WeaponType.SEMIAUTO:
				if (Input.GetMouseButtonDown(0) && fireRate < 0 && !shotgun && !reloading)
				{
					fireRate = originalFireRate;
					Shoot();
					ammoCapacity--;
				}
				else if (Input.GetMouseButtonDown(0) && fireRate < 0 && shotgun && !reloading)
				{
					fireRate = originalFireRate;
					ammoCapacity--;
					for (int i = 0; i < numPellets; i++)
					{
						ShotGunShoot();
					}
				}
				break;
			case WeaponType.FULLAUTO:
				if (Input.GetMouseButton(0) && fireRate < 0 && !shotgun && !reloading)
				{
					fireRate = originalFireRate;
					Shoot();
					ammoCapacity--;
					
				}
				else if (Input.GetMouseButton(0) && fireRate < 0 && shotgun && !reloading)
				{
					fireRate = originalFireRate;
					ammoCapacity--;
					for (int i = 0; i < numPellets; i++)
					{
						ShotGunShoot();
					}
				}
				break;
			case WeaponType.BURST:
				if (Input.GetMouseButtonDown(0) && fireRate < 0 && !reloading)
				{
					for (int i = 0; i < burstCount; i++) 
					{
						Invoke("BurstShoot", burstFireRate * i);
						ammoCapacity--;
						if (ammoCapacity <= 0)
						{
							break;
						}
					}
				}
				break;
		}

		fireRate -= Time.deltaTime;
	}

	private void BurstShoot()
	{
		float randomBloom = Random.Range(-bloom, bloom);
		fireRate = originalFireRate;
		bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
		bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceY(randomBloom);
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity);
	}

	private void Shoot()
	{
		float randomBloom = Random.Range(-bloom, bloom);
		bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
		bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceY(randomBloom);
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity);
	}

	private void ShotGunShoot()
	{
		float randomBloom = Random.Range(-bloom, bloom);
		float randomVelocityChange = Random.Range(-horizontalSpread, horizontalSpread);
		bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
		bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceY(randomBloom);
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity + randomVelocityChange);
	}

	private void Reload()
	{
		for (int i = 0; i < maxAmmoCapacity; i++)
		{
			if (ammoReserve > 0 && ammoCapacity < maxAmmoCapacity)
			{
				ammoCapacity++;
				ammoReserve--;
			}
			else break;
		}
		reloading = false;
	}
}
