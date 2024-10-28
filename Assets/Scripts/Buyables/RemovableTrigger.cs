using System.Linq;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RemovableTrigger : MonoBehaviour
{
	[SerializeField] GameObject toRemove;
	[SerializeField] int costToRemove;
	[SerializeField] GameObject text;
	private bool done = false;

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && !done)
		{
			text.SetActive(true);
			if (collision.GetComponent<Inventory>().GetPoints() >= costToRemove && Input.GetKey(KeyCode.E))
			{
				done = true;
				toRemove.GetComponent<Path>().remove = true;
				collision.GetComponent<Inventory>().AddPoints(-costToRemove);
				Destroy(text);
				Invoke("DestroySelf", 0.1f);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player") && text != null)
		{
			text.SetActive(false);
		}
	}

	private void DestroySelf()
	{
		Destroy(gameObject);
	}
}
