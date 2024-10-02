using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Rigidbody2D rb;

    [Header("Movement Variables")]
    [SerializeField] float acceleration;
    [SerializeField] float speed;
    [SerializeField] float drag;

    private float horizontalDirection;
    private bool changingDirection => (rb.linearVelocity.x > 0 && horizontalDirection < 0) || (rb.linearVelocity.x < 0 && horizontalDirection > 0);

    [Header("Jump Variables")]
    [SerializeField] float jumpForce = 15;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
        horizontalDirection = GetInput().x;
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
	}

	private void FixedUpdate()
	{
        MovePlayer();
        ApplyDrag();
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

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
