using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public bool valid = true;

	private void Update()
	{
		if (!valid) Invoke("ChangeValid", GetComponentInParent<Waves>().ogTimeBetweenSpawns);
	}

	private void ChangeValid()
	{
		valid = true;
	}
}
