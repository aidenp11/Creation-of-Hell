using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] int damage;
	[SerializeField] GameObject hitEffect;
	private GameObject destroyHitEffect;

	private Collider2D collided;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && GetComponentInParent<EnemyBase>().enemyType != EnemyBase.EnemyType.hugger
			&& !collision.GetComponent<Inventory>().justAttacked)
		{
			destroyHitEffect = Instantiate(hitEffect, transform.position, transform.rotation);
			Destroy(destroyHitEffect, 0.5f);
			collision.GetComponent<Inventory>().ApplyDamage(damage);
			collision.GetComponent<Inventory>().justAttacked = true;
			GetComponent<Collider2D>().enabled = false;
		}
		else if (collision.CompareTag("Player") && GetComponentInParent<EnemyBase>().enemyType == EnemyBase.EnemyType.hugger)
		{
			collision.GetComponent<Inventory>().ApplyDamage(damage);
			collision.GetComponent<PlayerMovement2D>().enabled = false;
			collision.GetComponent<Rigidbody2D>().Sleep();
			collided = collision;
		}
	}

	private void OnDestroy()
	{
		if (GetComponentInParent<EnemyBase>().enemyType == EnemyBase.EnemyType.hugger && collided != null) collided.GetComponent<PlayerMovement2D>().enabled = true;
		GetComponentInParent<EnemyBase>().attacking = false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
	}
}
