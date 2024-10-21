using UnityEditor.UI;
using UnityEngine;

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

	private float newDamage;

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
			destroyHitEffect = Instantiate(hitEffect, collision.transform);
			Destroy(destroyHitEffect, 0.5f);
			collision.GetComponent<EnemyBase>().ApplyDamage(damage);
			pierce--;
			newDamage = (float)damage * pierceDamageFalloff;
			damage = (int)newDamage;
		}
	}
}
