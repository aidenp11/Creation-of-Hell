using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class AmmoBase : MonoBehaviour
{
    [SerializeField] int damage;
	[SerializeField] float bulletLifespan;
	[SerializeField] int pierce;
	[SerializeField][Range(0, 1)] float pierceDamageFalloff;

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
		if (collision.CompareTag("Wall")) Destroy(gameObject);
		if (pierce > 0 && collision.CompareTag("Enemy"))
		{
			collision.GetComponent<DamagableTest>().ApplyDamage(damage);
			pierce--;
			newDamage = (float)damage * pierceDamageFalloff;
			damage = (int)newDamage;
		}
	}
}
