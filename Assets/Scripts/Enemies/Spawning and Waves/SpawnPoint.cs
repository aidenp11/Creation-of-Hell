using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject enemyToSpawn;
    [SerializeField] float secondsToSpawn;
	private float ogseconds;

	private void Start()
	{
		ogseconds = secondsToSpawn;
	}
	private void Update()
	{
		secondsToSpawn -= Time.deltaTime;
		if (secondsToSpawn <= 0 )
		{
			Instantiate(enemyToSpawn, transform);
			secondsToSpawn = ogseconds;
		}
	}
}
