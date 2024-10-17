using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
	[Header("Enemy Data")]
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	[SerializeField] int Health;
	[SerializeField] float speed;
	[SerializeField] float acceleration;
	[SerializeField] GameObject attackObject;
	private GameObject attack;
	[SerializeField] float attackSpeed;
	[SerializeField] float attackCooldown;
	[SerializeField] Transform notFlippedAttackPosition;
	[SerializeField] Transform flippedAttackPosition;
	private Transform attackPosition;
	[SerializeField] float fadeSeconds;
	public bool attacking;
	public enum EnemyType
	{
		normal,
		jumper,
		hugger
	}
	[SerializeField] public EnemyType enemyType;
	[Header("Jumper Stuff")]
	[SerializeField] float jumpForce;
	[SerializeField] float jumpForwardForce;
	private float positiveJumpForwardForce;
	private float negativeJumpForwardForce;
	[SerializeField] float jumpAttackWaitSpeed;

	[Header("Hugger Stuff")]

	[Header("Player Stuff")]
	[SerializeField] GameObject player;
	[SerializeField] float howCloseToPlayer;
	private Transform playerTransform;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		positiveJumpForwardForce = jumpForwardForce;
		negativeJumpForwardForce = -jumpForwardForce;
		if (GetComponent<SpriteRenderer>().flipX == true)
		{
			attackPosition = flippedAttackPosition;
			jumpForwardForce = negativeJumpForwardForce;
		}
		else
		{
			attackPosition = notFlippedAttackPosition;
			jumpForwardForce = positiveJumpForwardForce;
		}
	}

	private void Update()
	{
		if (GetComponent<SpriteRenderer>().flipX == true)
		{
			attackPosition = flippedAttackPosition;
			jumpForwardForce = negativeJumpForwardForce;
		}
		else
		{
			attackPosition = notFlippedAttackPosition;
			jumpForwardForce = positiveJumpForwardForce;
		}
		if (Health <= 0)
		{
			if (attack != null) Destroy(attack);
			GetComponent<Collider2D>().enabled = false;
			GetComponent<Rigidbody2D>().Sleep();
			Destroy(gameObject, fadeSeconds);
			GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, fadeSeconds * 0.005f);
		}
		else if (Mathf.Abs(playerTransform.position.x - transform.position.x) <= howCloseToPlayer && !attacking)
		{
			if (enemyType == EnemyType.jumper)
			{
				MoveEnemy();
				Invoke("JumpAttack", jumpAttackWaitSpeed);
				attacking = true;
				Invoke("Attack", attackSpeed);
			}
			else
			{
				attacking = true;
				Invoke("Attack", attackSpeed);
			}
		}
		else if (!attacking)
		{
			MoveEnemy();
		}
		if (attack != null)
		{
			Destroy(attack, attackCooldown);
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
		attack = Instantiate(attackObject, attackPosition);
	}

	private void JumpAttack()
	{
		rb.AddForce(new Vector2(jumpForwardForce, jumpForce));
	}

	public void ApplyDamage(int damage)
	{
		Health -= damage;
	}
}
