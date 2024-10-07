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

	[Header("Movement Variables")]
	[SerializeField] float acceleration;
	[SerializeField] float speed;
	[SerializeField] float drag;

	[SerializeField] Position handTransform;

	private float horizontalDirection;
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

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		horizontalDirection = GetInput().x;
		if (canJump) Jump();
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

		if (Mathf.Abs(rb.linearVelocity.x) > speed)
		{
			rb.linearVelocity = new Vector2(Mathf.Sign(rb.linearVelocity.x) * speed, rb.linearVelocity.y);
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
