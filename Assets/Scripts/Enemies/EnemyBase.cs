using System.Data;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBase : MonoBehaviour
{
	[Header("Enemy Data")]
	private Rigidbody2D rb;
	private SpriteRenderer sr;
	[SerializeField] public int Health;
	[SerializeField] float speed;
	[SerializeField] float acceleration;
	private float ogAccel;
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

	[Header("Animation")]
	[SerializeField] Animator animator;

	[Header("Player Stuff")]
	private GameObject player;
	private GameObject wave;
	[SerializeField] float howCloseToPlayer;
	private Transform playerTransform;

	[SerializeField] LayerMask slopeLayer;
	[SerializeField] float groundRaycastLength;
	[SerializeField] private Vector3 groundRaycastOffset;
	private bool onSlope;

	[Header("Audio")]
	[SerializeField] public AudioSource hit;

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
		for (int i = 0; i < SceneManager.GetActiveScene().GetRootGameObjects().Length; i++)
		{
			if (SceneManager.GetActiveScene().GetRootGameObjects().ElementAt(i).GetComponent<Waves>())
			{
				wave = SceneManager.GetActiveScene().GetRootGameObjects().ElementAt(i);
				break;
			}
		}
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		ogAccel = acceleration;
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
		CheckCollisions();
		playerTransform = player.GetComponent<Transform>();

		if (Mathf.Abs(transform.position.x - playerTransform.position.x) >= 25)
		{
			if (enemyType == EnemyType.normal)
			{
				wave.GetComponent<Waves>().mmfCount++;
				wave.GetComponent<Waves>().totalSpawned--;
				Destroy(gameObject);
			}
			else if (enemyType == EnemyType.jumper)
			{
				wave.GetComponent<Waves>().jumpsterCount++;
				wave.GetComponent<Waves>().totalSpawned--;
				Destroy(gameObject);
			}
			else if (enemyType == EnemyType.hugger)
			{
				wave.GetComponent<Waves>().huggyBearCount++;
				wave.GetComponent<Waves>().totalSpawned--;
				Destroy(gameObject);
			}
		}

		if (onSlope)
		{
			acceleration = ogAccel * 4.0f;
		}
		else acceleration = ogAccel;

		animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
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
			animator.SetTrigger("Death");
			if (attack != null) Destroy(attack);
			GetComponent<Collider2D>().enabled = false;
			GetComponent<Rigidbody2D>().Sleep();
			Invoke("AddPointsToPlayer", fadeSeconds);
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

	private void MoveEnemy()
	{
		if (playerTransform.position.x - transform.position.x > 0)
		{
			sr.flipX = false;
			rb.AddForce(new Vector2(1, 0f) * acceleration);
		}
		else if (playerTransform.position.x - transform.position.x < 0)
		{
			sr.flipX = true;
			rb.AddForce(new Vector2(-1, 0f) * acceleration);
		}

		if (Mathf.Abs(rb.linearVelocity.x) > speed)
		{
			rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * speed, rb.linearVelocity.y);
		}
	}

	private void CheckCollisions()
	{
		onSlope = Physics2D.Raycast(transform.position + groundRaycastOffset, Vector2.down, groundRaycastLength, slopeLayer) ||
				   Physics2D.Raycast(transform.position - groundRaycastOffset, Vector2.down, groundRaycastLength, slopeLayer);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position + groundRaycastOffset, transform.position + groundRaycastOffset + Vector3.down * groundRaycastLength);
		Gizmos.DrawLine(transform.position - groundRaycastOffset, transform.position - groundRaycastOffset + Vector3.down * groundRaycastLength);
	}

	private void Attack()
	{
		if (enemyType != EnemyType.jumper)
		{
			animator.SetTrigger("Attack");
		}
		attack = Instantiate(attackObject, attackPosition);
	}

	private void JumpAttack()
	{
		animator.SetTrigger("Attack");
		rb.AddForce(new Vector2(jumpForwardForce, jumpForce));
	}

	public void ApplyDamage(int damage)
	{
		Health -= damage;
	}

	private void AddPointsToPlayer()
	{
		if (enemyType == EnemyType.normal)
		{
			player.GetComponent<Inventory>().AddPoints(75);
			player.GetComponent<Inventory>().mmfKilled++;
			player.GetComponent<Inventory>().totalKilled++;
		}
		else if (enemyType == EnemyType.jumper)
		{
			player.GetComponent<Inventory>().AddPoints(100);
			player.GetComponent<Inventory>().jumpsterKilled++;
			player.GetComponent<Inventory>().totalKilled++;
		}
		else
		{
			player.GetComponent<Inventory>().AddPoints(125);
			player.GetComponent<Inventory>().huggyBearKilled++;
			player.GetComponent<Inventory>().totalKilled++;
		}
	}
}
