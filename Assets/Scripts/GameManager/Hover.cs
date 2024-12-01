using UnityEngine;

public class Hover : MonoBehaviour
{
	private float seconds = 5;
	void Update()
	{
		if (seconds > 2.5f)
		{
			transform.position += new Vector3(0, 0.01f);
		}
		else if (seconds < 2.5f)
		{
			transform.position += new Vector3(0, -0.01f);
		}

		if (seconds > 0)
		{
			seconds -= Time.deltaTime;
		}
		else if (seconds <= 0) seconds = 5;
	}
}
