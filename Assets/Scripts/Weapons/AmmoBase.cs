using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class AmmoBase : MonoBehaviour
{
    [SerializeField] int damage;
	[SerializeField] float bulletLifespan;

	private void Update()
	{
		bulletLifespan -= Time.deltaTime;
		if (bulletLifespan < 0) 
		{
			Destroy(gameObject);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Destroy(gameObject);
	}
}
