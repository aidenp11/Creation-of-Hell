using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
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
	private Transform muzzleTransform;
	[SerializeField] Transform flippedMuzzleTransform;
	[SerializeField] Transform notFlippedMuzzleTransform;
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

	private void Start()
	{
		originalFireRate = fireRate;
		fireRate = 0;
		maxAmmoCapacity = ammoCapacity;
		if (GetComponent<SpriteRenderer>().flipY == true)
		{
			muzzleTransform = flippedMuzzleTransform;
			cartridgeEjectionTransform = cartridgeEjectionTransformFlipped;
		}
		else
		{
			muzzleTransform = notFlippedMuzzleTransform;
			cartridgeEjectionTransform = cartridgeEjectionTransformNotFlipped;
		}
	}

	private void Update()
	{
        if (GetComponent<SpriteRenderer>().flipY == true)
        {
			muzzleTransform = flippedMuzzleTransform;
			cartridgeEjectionTransform = cartridgeEjectionTransformFlipped;
		}
		else
		{
			muzzleTransform = notFlippedMuzzleTransform;
			cartridgeEjectionTransform = cartridgeEjectionTransformNotFlipped;
		}
		if (ammoCapacity <= 0 && ammoReserve <= 0)
		{
			reloading = true;
		}
        else if (ammoCapacity <= 0 && !reloading)
		{
			reloading = true;
			animator.SetTrigger("Reload");
			Invoke("Reload", reloadSpeed);
		}
		else if (Input.GetKeyDown(KeyCode.R) && ammoCapacity < maxAmmoCapacity && ammoReserve > 0 && !reloading && !burstFiring)
		{
			reloading = true;
			animator.SetTrigger("Reload");
			Invoke("Reload", reloadSpeed);
		}
		switch (weaponType)
		{
			case WeaponType.SEMIAUTO:
				if (Input.GetMouseButtonDown(0) && fireRate < 0 && !shotgun && !reloading)
				{
					fireRate = originalFireRate;
					Shoot();
					animator.SetTrigger("Shoot");
					GameObject cartridged = Instantiate(cartridge, cartridgeEjectionTransform.position, cartridgeEjectionTransform.rotation);
					cartridged.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 125));
					Destroy(cartridged, 0.8f);
					Destroy(Instantiate(shotEffect, muzzleTransform), secondsToDestroyShotEffect);
					ammoCapacity--;
				}
				else if (Input.GetMouseButtonDown(0) && fireRate < 0 && shotgun && !reloading)
				{
					fireRate = originalFireRate;
					animator.SetTrigger("Shoot");
					GameObject cartridged = Instantiate(cartridge, cartridgeEjectionTransform.position, cartridgeEjectionTransform.rotation);
					cartridged.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 125));
					Destroy(cartridged, 0.8f);
					Destroy(Instantiate(shotEffect, muzzleTransform), secondsToDestroyShotEffect);
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
					animator.SetTrigger("Shoot");
					GameObject cartridged = Instantiate(cartridge, cartridgeEjectionTransform.position, cartridgeEjectionTransform.rotation);
					cartridged.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 125));
					Destroy(cartridged, 0.8f);
					Destroy(Instantiate(shotEffect, muzzleTransform), secondsToDestroyShotEffect);
					Shoot();
					ammoCapacity--;
					
				}
				else if (Input.GetMouseButton(0) && fireRate < 0 && shotgun && !reloading)
				{
					fireRate = originalFireRate;
					animator.SetTrigger("Shoot");
					GameObject cartridged = Instantiate(cartridge, cartridgeEjectionTransform.position, cartridgeEjectionTransform.rotation);
					cartridged.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 125));
					Destroy(cartridged, 0.8f);
					Destroy(Instantiate(shotEffect, muzzleTransform), secondsToDestroyShotEffect);
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
		fireRate -= Time.deltaTime;
	}

	private void BurstShoot()
	{
		animator.SetTrigger("Shoot");
		GameObject cartridged = Instantiate(cartridge, cartridgeEjectionTransform.position, cartridgeEjectionTransform.rotation);
		cartridged.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 125));
		Destroy(cartridged, 0.8f);
		Destroy(Instantiate(shotEffect, muzzleTransform), secondsToDestroyShotEffect);
		float randomBloom = UnityEngine.Random.Range(-bloom, bloom);
		fireRate = originalFireRate;
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
