using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Explosion : MonoBehaviour
{
    [SerializeField] int damage;
	[SerializeField] float lifespan;
	private int damageToUse;
    [SerializeField] int pierce;
	private int piercePerkPierce;
	public int pierceToUse;

	private GameObject player;

	private bool doneUpgrade;
	private bool doneUpgrade2;

	private void Start()
	{
		for (int i = 0; i < SceneManager.GetActiveScene().GetRootGameObjects().Length; i++)
		{
			if (SceneManager.GetActiveScene().GetRootGameObjects().ElementAt(i).GetComponent<PlayerMovement2D>())
			{
				player = SceneManager.GetActiveScene().GetRootGameObjects().ElementAt(i);
				break;
			}
		}
		damageToUse = damage;
		pierceToUse = pierce;
		piercePerkPierce = pierce + 3;
		if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded == true == true && !doneUpgrade)
		{
			UpgradeExplosion();
			doneUpgrade = true;
		}
		if (player.GetComponent<Inventory>().activeWeapon.GetComponent<WeaponBase>().upgraded2 == true && !doneUpgrade2)
		{
			UpgradeExplosion();
			doneUpgrade2 = true;
		}
		if (player.GetComponent<Inventory>().piercePerk == true)
		{
			pierceToUse = piercePerkPierce;
		}
	}

	private void Update()
	{
		lifespan -= Time.deltaTime;
		if (pierceToUse <= 0)
		{
			Destroy(gameObject);
		}
		if (lifespan <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (pierceToUse > 0 && collision.CompareTag("Enemy") && collision.GetComponent<EnemyBase>().Health >= 0)
		{
			collision.GetComponent<EnemyBase>().ApplyDamage(damageToUse);
			pierceToUse--;
		}
	}

	private void UpgradeExplosion()
	{
		damageToUse = (int)((float)damageToUse * 3);
		pierceToUse = (int)((float)pierceToUse * 1.5);
		piercePerkPierce = pierceToUse + 3;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
	}
}
