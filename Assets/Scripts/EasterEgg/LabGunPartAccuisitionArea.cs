using UnityEngine;

public class LabGunPartAccuisitionArea : MonoBehaviour
{
    [SerializeField] GameObject labGunPart1;
    [SerializeField] GameObject labGunPart2;
    [SerializeField] GameObject labGunPart3;

    [SerializeField] bool part1;
    [SerializeField] bool part2;
    [SerializeField] bool part3;

    [SerializeField] GameObject text;
    
	private void OnTriggerStay2D(Collider2D collision)
	{
        if (collision.CompareTag("Player")) text.SetActive(true);

		if (part1)
        {
            if (collision.CompareTag("Player") && collision.GetComponent<Inventory>().gambler && collision.GetComponent<Inventory>().healthPerk && 
                collision.GetComponent<Inventory>().piercePerk && collision.GetComponent<Inventory>().regenPerk && collision.GetComponent<PlayerMovement2D>().speedPerk)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    labGunPart1.SetActive(true);
                    Destroy(text);
                    Destroy(gameObject);
                }
            }
        }

        if (part2)
        {
            if (collision.CompareTag("Player"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    labGunPart2.SetActive(true);
                    Destroy(text);
                    Destroy(gameObject);
                }
            }
        }

        if (part3)
        {
			if (collision.CompareTag("Player"))
			{
				if (Input.GetKeyDown(KeyCode.E))
				{
					labGunPart3.SetActive(true);
					Destroy(text);
					Destroy(gameObject);
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.CompareTag("Player")) text.SetActive(false);
	}
}
