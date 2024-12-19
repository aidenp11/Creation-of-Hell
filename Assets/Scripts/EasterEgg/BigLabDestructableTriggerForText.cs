using UnityEngine;

public class BigLabDestructableTriggerForText : MonoBehaviour
{
	[SerializeField] GameObject text;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			text.SetActive(true);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			text.SetActive(false);
		}
	}
}
