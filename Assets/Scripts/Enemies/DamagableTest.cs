using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamagableTest : MonoBehaviour
{
    [SerializeField] int Health;
	[SerializeField] float fadeSeconds;

	private void Update()
	{
		if (Health <= 0)
		{
			GetComponent<Collider2D>().enabled = false;
			Destroy(gameObject, fadeSeconds);
			GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, fadeSeconds * 0.005f);
		}
	}

	public void ApplyDamage(int damage)
    {
        Health -= damage;
    }
}
