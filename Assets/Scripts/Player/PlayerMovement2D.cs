using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
	[Header("Components")]
	private Rigidbody2D rb;

	[Header("Layer Masks")]
	[SerializeField] LayerMask groundLayer;
	[SerializeField] LayerMask slopeLayer;

	[Header("Movement Variables")]
	[SerializeField] float acceleration;
	[SerializeField] public float speed;
	private float speedToUse;
	[SerializeField] float drag;
	private float ogAccel;

	[SerializeField] Position handTransform;

	[SerializeField] Animator animator;

	public float horizontalDirection;
	private bool changingDirection => (rb.linearVelocity.x > 0 && horizontalDirection < 0) || (rb.linearVelocity.x < 0 && horizontalDirection > 0);

	[Header("Jump Variables")]
	[SerializeField] float jumpForce = 15;
	[SerializeField] float airDrag = 3;
	private bool canJump => Input.GetButtonDown("Jump") && onGround;

	[SerializeField] float fallMultiplier = 8;
	[SerializeField] float lowJumpFallMultiplier = 5;

	[Header("Ground Check Variables")]
	[SerializeField] float groundRaycastLength;
	[SerializeField] private Vector3 groundRaycastOffset;
	private bool onGround;
	private bool onSlope;

	[Header("Perks")]
	public bool speedPerk;

	private void Start()
	{
		speedPerk = false;
		speedToUse = speed;
		ogAccel = acceleration;
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (speedPerk)
		{
			speedToUse = speed * 1.5f;
		}
		animator.SetFloat("Speed", Math.Abs(rb.linearVelocity.x));
		animator.SetBool("Simulated", rb.simulated);
		if (onSlope == true && onGround == false)
		{
			animator.SetBool("Jump", onSlope);
		}
		else
		{
			animator.SetBool("Jump", onGround);
		}
		horizontalDirection = GetInput().x;
		if (canJump) Jump();

		if (onSlope && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
		{
			rb.simulated = true;
			acceleration = ogAccel * 12.5f;
		}
		else if (onSlope && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
		{
			rb.simulated = false;
		}
		else
		{
			rb.simulated = true;
			acceleration = ogAccel;
		}
	}

	private void FixedUpdate()
	{
		CheckCollisions();
		MovePlayer();
		if (onGround)
		{
			ApplyDrag();
		}
		else
		{
			ApplyAirDrag();
			FallMultiplier();
		}
	}

	private Vector2 GetInput()
	{
		return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
	}

	private void MovePlayer()
	{
		rb.AddForce(new Vector2(horizontalDirection, 0f) * acceleration);

		if (Mathf.Abs(rb.linearVelocity.x) > speedToUse)
		{
			rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * speedToUse, rb.linearVelocity.y);
		}
	}

	private void ApplyDrag()
	{
		if (Mathf.Abs(horizontalDirection) < 0.4f || changingDirection)
		{
			rb.linearDamping = drag;
		}
		else
		{
			rb.linearDamping = 0;
		}
	}

	private void ApplyAirDrag()
	{
		rb.linearDamping = airDrag;
	}
	private void Jump()
	{
		rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
		rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
	}

	private void CheckCollisions()
	{
		onGround = Physics2D.Raycast(transform.position + groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer) ||
				   Physics2D.Raycast(transform.position - groundRaycastOffset, Vector2.down, groundRaycastLength, groundLayer);
		onSlope = Physics2D.Raycast(transform.position + groundRaycastOffset, Vector2.down, groundRaycastLength, slopeLayer) ||
				   Physics2D.Raycast(transform.position - groundRaycastOffset, Vector2.down, groundRaycastLength, slopeLayer);
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position + groundRaycastOffset, transform.position + groundRaycastOffset + Vector3.down * groundRaycastLength);
		Gizmos.DrawLine(transform.position - groundRaycastOffset, transform.position - groundRaycastOffset + Vector3.down * groundRaycastLength);
	}

	private void FallMultiplier()
	{
		if (rb.linearVelocity.y < 0)
		{
			rb.gravityScale = fallMultiplier;
		}
		else if (rb.linearVelocity.y > 0 && !Input.GetButtonDown("Jump"))
		{
			rb.gravityScale = lowJumpFallMultiplier;
		}
		else
		{
			rb.gravityScale = 1;
		}
	}
}
