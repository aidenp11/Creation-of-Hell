using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] int damage;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			collision.GetComponent<Inventory>().ApplyDamage(damage);
			GetComponent<Collider2D>().enabled = false;
		}
	}

	private void OnDestroy()
	{
		GetComponentInParent<EnemyBase>().attacking = false;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius);
	}
}
