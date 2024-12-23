using UnityEngine;
using UnityEngine.UI;

public class LabGunPart : MonoBehaviour
{
    public bool eepart1;
    public bool eepart2;
    public bool eepart3;

	[SerializeField] Image p1;
	[SerializeField] Image p2;
	[SerializeField] Image p3;

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && eepart1 && Input.GetKey(KeyCode.E))
		{
			collision.GetComponent<Inventory>().part1 = true;
			p1.color = Color.white;
			Destroy(gameObject);
		}

		if (collision.CompareTag("Player") && eepart2 && Input.GetKey(KeyCode.E))
		{
			collision.GetComponent<Inventory>().part2 = true;
			p2.color = Color.white;
			Destroy(gameObject);
		}

		if (collision.CompareTag("Player") && eepart3 && Input.GetKey(KeyCode.E))
		{
			collision.GetComponent<Inventory>().part3 = true;
			p3.color = Color.white;
			Destroy(gameObject);
		}
	}
}
