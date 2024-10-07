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
		SEMIAUTO = 0,
		FULLAUTO = 1,
		BURST = 2,
		ACTION = 3
	}
	[Header("shotgun stuff")]
	[SerializeField] bool shotgun = false;
	[SerializeField] int numPellets;

	private GameObject bullet;

	[Header("Other Stuff")]
	[SerializeField] Transform muzzleTransform;

	private void Start()
	{
		originalFireRate = fireRate;
		fireRate = 0;
	}

	private void Update()
	{
		float randomBloom = Random.Range(-bloom, bloom);
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
		fireRate -= Time.deltaTime;
	}

}
