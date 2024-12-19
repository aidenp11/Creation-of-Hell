using UnityEngine;

public class BigLabDoorDestructable : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("LabGunBullet"))
		{
			Destroy(gameObject);
		}
	}
}
