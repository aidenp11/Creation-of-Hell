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
	private float originalFireRate;
	[SerializeField]
	enum WeaponType
	{
		SEMIAUTO,
		FULLAUTO,
		BURST
	}

	[SerializeField] WeaponType weaponType = WeaponType.SEMIAUTO;

	[Header("Shotgun Stuff")]
	[SerializeField] bool shotgun = false;
	[SerializeField] int numPellets;

	[Header("Burst Stuff")]
	[SerializeField] float burstFireRate;
	private float originalBurstFireRate;
	[SerializeField] int burstCount;

	private GameObject bullet;

	[Header("Other Stuff")]
	[SerializeField] Transform muzzleTransform;

	private void Start()
	{
		originalFireRate = fireRate;
		originalBurstFireRate = burstFireRate;
		fireRate = 0;
	}

	private void Update()
	{
		float randomBloom = Random.Range(-bloom, bloom);
		switch (weaponType)
		{
			case WeaponType.SEMIAUTO:
				if (Input.GetMouseButtonDown(0) && fireRate < 0 && !shotgun)
				{
					fireRate = originalFireRate;
					bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
					bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
					bullet.GetComponent<Rigidbody2D>().AddRelativeForceY(randomBloom);
					bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity);
				}
				else if (Input.GetMouseButtonDown(0) && fireRate < 0 && shotgun)
				{
					fireRate = originalFireRate;
					for (int i = 0; i < numPellets; i++)
					{
						randomBloom = Random.Range(-bloom, bloom);
						bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
						bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
						bullet.GetComponent<Rigidbody2D>().AddRelativeForceY(randomBloom);
						bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity);
					}
				}
				break;
			case WeaponType.FULLAUTO:
				if (Input.GetMouseButton(0) && fireRate < 0 && !shotgun)
				{
					fireRate = originalFireRate;
					bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
					bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
					bullet.GetComponent<Rigidbody2D>().AddRelativeForceY(randomBloom);
					bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity);
				}
				else if (Input.GetMouseButton(0) && fireRate < 0 && shotgun)
				{
					fireRate = originalFireRate;
					for (int i = 0; i < numPellets; i++)
					{
						randomBloom = Random.Range(-bloom, bloom);
						bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
						bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
						bullet.GetComponent<Rigidbody2D>().AddRelativeForceY(randomBloom);
						bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity);
					}
				}
				break;
			case WeaponType.BURST:
				burstFireRate -= Time.deltaTime;
				if (Input.GetMouseButtonDown(0) && fireRate < 0)
				{
					burstFireRate = 0;
					fireRate = originalFireRate;
					for (int i = 0; i < burstCount; i++)
					{
						if (burstFireRate <= 0)
						{
							while (i < burstCount - 1)
							{

								burstFireRate = originalBurstFireRate;
								randomBloom = Random.Range(-bloom, bloom);
								bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
								bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
								bullet.GetComponent<Rigidbody2D>().AddRelativeForceY(randomBloom);
								bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity);
							}
						}
					}
				}
				break;
		}

		fireRate -= Time.deltaTime;
	}

}
