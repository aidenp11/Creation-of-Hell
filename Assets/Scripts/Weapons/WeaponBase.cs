using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBase : MonoBehaviour
{
	[Header("Bullet Data")]
	[SerializeField] float bulletVelocity;
	[SerializeField] float bulletDrop;
	[SerializeField] public GameObject bulletPrefab;

	[Header("Weapon Data")]
	[SerializeField] public string weaponName;
	[SerializeField] public int genieChance;
	[SerializeField] float reloadSpeed;
	[SerializeField] float bloom;
	[SerializeField] public float fireRate;
	public float fireRateToUse;
	[SerializeField] public int ammoCapacity;
	public int maxAmmoCapacity;
	[SerializeField] public int ammoReserve;
	public bool upgraded;
	public bool upgraded2;
	public int maxAmmoReserve;
	public bool reloading = false;
	private bool outOfBullts = false;
	private float originalFireRate;
	[SerializeField] bool laser;
	[SerializeField] GameObject laserObject;
	[SerializeField] float laserLength;
	private GameObject laser1;
	private GameObject laser2;
	private Transform muzzleTransform;
	[SerializeField] Transform flippedMuzzleTransform;
	[SerializeField] Transform notFlippedMuzzleTransform;

	[SerializeField]
	public enum WeaponType
	{
		SEMIAUTO,
		FULLAUTO,
		BURST
	}

	[SerializeField] public WeaponType weaponType = WeaponType.SEMIAUTO;

	[SerializeField]
	public enum WeaponClass
	{
		PRIMARY,
		SECONDARY
	}

	[SerializeField] public WeaponClass weaponClass = WeaponClass.PRIMARY;

	[Header("Shotgun Stuff")]
	[SerializeField] public bool shotgun = false;
	[SerializeField] int numPellets;
	private int numPelletsToUse;
	[SerializeField] float horizontalSpread;

	[Header("Burst Stuff")]
	[SerializeField] float burstFireRate;
	[SerializeField] int burstCount;
	private bool burstFiring = false;

	[Header("Animation Stuff")]
	[SerializeField] Animator animator;
	[SerializeField] GameObject shotEffect;
	[SerializeField] float secondsToDestroyShotEffect;
	[SerializeField] GameObject cartridge;
	private Transform cartridgeEjectionTransform;
	[SerializeField] Transform cartridgeEjectionTransformFlipped;
	[SerializeField] Transform cartridgeEjectionTransformNotFlipped;

	private GameObject bullet;
	private bool upgradeDone = false;
	private bool upgrade2Done = false;

	[Header("Audio")]
	[SerializeField] AudioSource shotSound;
	[SerializeField] AudioSource reloadSound;

	private void Start()
	{
		upgraded = false;
		upgraded2 = false;
		originalFireRate = fireRate;
		numPelletsToUse = numPellets;
		fireRateToUse = 0;
		maxAmmoCapacity = ammoCapacity;
		maxAmmoReserve = ammoReserve;
		if (laser)
		{
			laser1 = Instantiate(laserObject, flippedMuzzleTransform);
			laser2 = Instantiate(laserObject, notFlippedMuzzleTransform);
			laser1.transform.localScale = new Vector3(laserLength, laser1.transform.localScale.y, laser1.transform.localScale.z);
			laser2.transform.localScale = new Vector3(laserLength, laser2.transform.localScale.y, laser2.transform.localScale.z);
		}

		if (GetComponent<SpriteRenderer>().flipY == true)
		{
			muzzleTransform = flippedMuzzleTransform;
			cartridgeEjectionTransform = cartridgeEjectionTransformFlipped;
			if (laser)
			{
				laser1.SetActive(true);
				laser2.SetActive(false);
			}
		}
		else
		{
			muzzleTransform = notFlippedMuzzleTransform;
			cartridgeEjectionTransform = cartridgeEjectionTransformNotFlipped;
			if (laser)
			{
				laser1.SetActive(false);
				laser2.SetActive(true);
			}
		}

	}

	private void Update()
	{
		if (upgraded && !upgradeDone)
		{
			Upgrade();
			upgradeDone = true;
		}
		if (upgraded2 && !upgrade2Done)
		{
			Upgrade();
			upgrade2Done = true;
		}
		if (GetComponent<SpriteRenderer>().flipY == true)
		{
			muzzleTransform = flippedMuzzleTransform;
			cartridgeEjectionTransform = cartridgeEjectionTransformFlipped;
			if (laser)
			{
				laser1.SetActive(true);
				laser2.SetActive(false);
			}
		}
		else
		{
			muzzleTransform = notFlippedMuzzleTransform;
			cartridgeEjectionTransform = cartridgeEjectionTransformNotFlipped;
			if (laser)
			{
				laser1.SetActive(false);
				laser2.SetActive(true);
			}
		}
		if (ammoCapacity <= 0 && ammoReserve <= 0)
		{
			outOfBullts = true;
		}
		else outOfBullts = false;
		if (ammoCapacity <= 0 && !reloading && !outOfBullts)
		{
			reloading = true;
			animator.SetTrigger("Reload");
			Invoke("Reload", reloadSpeed);
			reloadSound.Play();
		}
		else if (Input.GetKeyDown(KeyCode.R) && ammoCapacity < maxAmmoCapacity && ammoReserve > 0 && !reloading && !burstFiring && !outOfBullts)
		{
			reloading = true;
			animator.SetTrigger("Reload");
			Invoke("Reload", reloadSpeed);
			reloadSound.Play();
		}
		switch (weaponType)
		{
			case WeaponType.SEMIAUTO:
				if (Input.GetMouseButtonDown(0) && fireRateToUse < 0 && !shotgun && !reloading && !outOfBullts)
				{
					fireRateToUse = originalFireRate;
					Shoot();
					shotSound.Play();
					animator.SetTrigger("Shoot");
					GameObject cartridged = Instantiate(cartridge, cartridgeEjectionTransform.position, cartridgeEjectionTransform.rotation);
					cartridged.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 125));
					Destroy(cartridged, 0.8f);
					Destroy(Instantiate(shotEffect, muzzleTransform), secondsToDestroyShotEffect);
					ammoCapacity--;
				}
				else if (Input.GetMouseButtonDown(0) && fireRateToUse < 0 && shotgun && !reloading && !outOfBullts)
				{
					fireRateToUse = originalFireRate;
					animator.SetTrigger("Shoot");
					GameObject cartridged = Instantiate(cartridge, cartridgeEjectionTransform.position, cartridgeEjectionTransform.rotation);
					cartridged.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 125));
					Destroy(cartridged, 0.8f);
					Destroy(Instantiate(shotEffect, muzzleTransform), secondsToDestroyShotEffect);
					ammoCapacity--;
					for (int i = 0; i < numPelletsToUse; i++)
					{
						ShotGunShoot();
					}
					shotSound.Play();
				}
				break;
			case WeaponType.FULLAUTO:
				if (Input.GetMouseButton(0) && fireRateToUse < 0 && !shotgun && !reloading && !outOfBullts)
				{
					fireRateToUse = originalFireRate;
					animator.SetTrigger("Shoot");
					GameObject cartridged = Instantiate(cartridge, cartridgeEjectionTransform.position, cartridgeEjectionTransform.rotation);
					cartridged.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 125));
					Destroy(cartridged, 0.8f);
					Destroy(Instantiate(shotEffect, muzzleTransform), secondsToDestroyShotEffect);
					Shoot();
					shotSound.Play();
					ammoCapacity--;

				}
				else if (Input.GetMouseButton(0) && fireRateToUse < 0 && shotgun && !reloading && !outOfBullts)
				{
					fireRateToUse = originalFireRate;
					animator.SetTrigger("Shoot");
					GameObject cartridged = Instantiate(cartridge, cartridgeEjectionTransform.position, cartridgeEjectionTransform.rotation);
					cartridged.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 125));
					Destroy(cartridged, 0.8f);
					Destroy(Instantiate(shotEffect, muzzleTransform), secondsToDestroyShotEffect);
					ammoCapacity--;
					for (int i = 0; i < numPelletsToUse; i++)
					{
						ShotGunShoot();
					}
					shotSound.Play();
				}
				break;
			case WeaponType.BURST:
				if (Input.GetMouseButtonDown(0) && fireRateToUse < 0 && !reloading && !outOfBullts)
				{
					burstFiring = true;
					for (int i = 0; i < burstCount; i++)
					{
						Invoke("BurstShoot", burstFireRate * i);
						ammoCapacity--;
						if (ammoCapacity <= 0)
						{
							Invoke("SwitchBurstFiring", reloadSpeed);
							break;
						}
					}
					Invoke("SwitchBurstFiring", burstFireRate * burstCount);
				}
				break;
		}
		fireRateToUse -= Time.deltaTime;
	}

	private void BurstShoot()
	{
		if (!reloading)
		{
			animator.SetTrigger("Shoot");
			shotSound.Play();
		}
		GameObject cartridged = Instantiate(cartridge, cartridgeEjectionTransform.position, cartridgeEjectionTransform.rotation);
		cartridged.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 125));
		Destroy(cartridged, 0.8f);
		Destroy(Instantiate(shotEffect, muzzleTransform), secondsToDestroyShotEffect);
		float randomBloom = UnityEngine.Random.Range(-bloom, bloom);
		fireRateToUse = originalFireRate;
		bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
		bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceY(randomBloom);
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity);
	}

	private void SwitchBurstFiring()
	{
		burstFiring = false;
	}

	private void Shoot()
	{
		float randomBloom = UnityEngine.Random.Range(-bloom, bloom);
		bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
		bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceY(randomBloom);
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity);
	}

	private void ShotGunShoot()
	{
		float randomBloom = UnityEngine.Random.Range(-bloom, bloom);
		float randomVelocityChange = UnityEngine.Random.Range(-horizontalSpread, horizontalSpread);
		bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
		bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceY(randomBloom);
		bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity + randomVelocityChange);
	}

	private void Upgrade()
	{
		if (upgraded && !upgraded2)
		{
			GetComponent<SpriteRenderer>().color = new Color(1 * 2, 0.9109956f * 2, 0.4371068f * 2);
		}
		else
		{
			GetComponent<SpriteRenderer>().color = new Color(1.5f, 0, 2.3f);
		}
		if (weaponType == WeaponType.SEMIAUTO && !shotgun)
		{
			if (maxAmmoCapacity == 1) maxAmmoCapacity = 3;
			else maxAmmoCapacity = (int)((float)maxAmmoCapacity * 1.75f);
			maxAmmoReserve = (int)((float)maxAmmoReserve * 1.75f);
			if (weaponName == "Pistol" && upgraded2)
			{
				weaponType = WeaponType.FULLAUTO;
				originalFireRate = 0.1f;
			}
			InstaReload();
		}
		else if (weaponType == WeaponType.SEMIAUTO && shotgun)
		{
			if (maxAmmoCapacity == 1 && weaponName != "Lab Gun") maxAmmoCapacity = 4;
			else maxAmmoCapacity = (int)((float)maxAmmoCapacity * 1.65f);
			numPelletsToUse = (int)((float)numPelletsToUse * 1.5f);
			maxAmmoReserve = (int)((float)maxAmmoReserve * 1.75f);
			InstaReload();
		}
		else if (weaponType == WeaponType.FULLAUTO && !shotgun)
		{
			originalFireRate = originalFireRate * 0.75f;
			maxAmmoCapacity = (int)((float)maxAmmoCapacity * 1.45f);
			maxAmmoReserve = (int)((float)maxAmmoReserve * 1.85f);
			InstaReload();
		}
		else if (weaponType == WeaponType.FULLAUTO && shotgun)
		{
			originalFireRate = originalFireRate * 0.55f;
			numPelletsToUse = (int)((float)numPelletsToUse * 1.65f);
			maxAmmoCapacity = (int)((float)maxAmmoCapacity * 2.25f);
			maxAmmoReserve = (int)((float)maxAmmoReserve * 2.5f);
			InstaReload();
		}
		else if (weaponType == WeaponType.BURST)
		{
			originalFireRate = originalFireRate * 0.8f;
			maxAmmoCapacity = (int)((float)maxAmmoCapacity * 1.65f);
			maxAmmoReserve = (int)((float)maxAmmoReserve * 2.05f);
			InstaReload();
		}
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

	private void InstaReload()
	{
		ammoCapacity = maxAmmoCapacity;
		ammoReserve = maxAmmoReserve;
	}
}
