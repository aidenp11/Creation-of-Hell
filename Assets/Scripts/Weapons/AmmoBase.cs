using System.Linq;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class AmmoBase : MonoBehaviour
{
	[SerializeField] int damage;
	[SerializeField] float bulletLifespan;
	[SerializeField] int pierce;
	[SerializeField][Range(0, 1)] float pierceDamageFalloff;
	[SerializeField] GameObject hitEffect;
	private GameObject destroyHitEffect;
	//[SerializeField] GameObject wallHitEffect;
	private GameObject player;

	private float newDamage;


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
	}
	private void Update()
	{
		bulletLifespan -= Time.deltaTime;
		if (pierce <= 0)
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
		if (pierce > 0 && collision.CompareTag("Enemy"))
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
			collision.GetComponent<EnemyBase>().ApplyDamage(damage);
			pierce--;
			newDamage = (float)damage * pierceDamageFalloff;
			damage = (int)newDamage;
		}
	}
}
