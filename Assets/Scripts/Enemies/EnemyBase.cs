using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
	[Header("Enemy Data")]
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	[SerializeField] int Health;
	[SerializeField] int damage;
	[SerializeField] float speed;
	[SerializeField] float acceleration;
	[SerializeField] float attackSpeed;
	[SerializeField] Transform attackPosition;
	[SerializeField] float fadeSeconds;
	private bool attacking;

	[SerializeField] GameObject player;
	[SerializeField] float howCloseToPlayer;
	private Transform playerTransform;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (Health <= 0)
		{
			GetComponent<Collider2D>().enabled = false;
			GetComponent<Rigidbody2D>().Sleep();
			Destroy(gameObject, fadeSeconds);
			GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, fadeSeconds * 0.005f);
		}
		else if (Mathf.Abs(playerTransform.position.x - transform.position.x) <= howCloseToPlayer && !attacking)
		{

		}
		else if (!attacking)
		{
			MoveEnemy();
		}
	}


	private void FixedUpdate()
	{
		playerTransform = player.GetComponent<Transform>();
	}

	private void MoveEnemy()
	{
		if (playerTransform.position.x - transform.position.x > 0)
		{
			sr.flipX = false;
			rb.AddForce(new Vector2(playerTransform.position.x - transform.position.x, 0f) * acceleration);
		}
		else if (playerTransform.position.x - transform.position.x < 0)
		{
			sr.flipX = true;
			rb.AddForce(new Vector2(playerTransform.position.x - transform.position.x, 0f) * acceleration);
		}

		if (Mathf.Abs(rb.linearVelocity.x) > speed)
		{
			rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * speed, rb.linearVelocity.y);
		}
	}

	private void Attack()
	{

	}

	public void ApplyDamage(int damage)
	{
		Health -= damage;
	}
}
