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
    [SerializeField] enum WeaponType
    {
        SEMIAUTO = 0,
        FULLAUTO = 1,
        BURST = 2,
        ACTION = 3
    }

    private GameObject bullet;

    [Header("Other Stuff")]
    [SerializeField] Transform muzzleTransform;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
        {
            bullet = Instantiate(bulletPrefab, new Vector2(muzzleTransform.position.x, muzzleTransform.position.y), muzzleTransform.rotation);
            bullet.GetComponent<Rigidbody2D>().gravityScale = bulletDrop;
            bullet.GetComponent<Rigidbody2D>().AddRelativeForceX(bulletVelocity);
        }
	}

}
