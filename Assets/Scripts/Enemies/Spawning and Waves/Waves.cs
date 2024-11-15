using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waves : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnPoints;
    [SerializeField] GameObject mmf;
    [SerializeField] GameObject huggyBear;
    [SerializeField] GameObject jumpster;

	private GameObject player;
	private Transform playerTransform;

	private int roundNumber;
    public int mmfCount;
    private int originalmmfCount;
    public int huggyBearCount;
    private int originalHuggyBearCount;
    public int jumpsterCount;
    private int originalJumpsterCount;
	private int totalSpawned;

	private float timeBetweenSpawns = 1;

	private int spawnNumber = 0;

	private void Start()
	{
		for (int i = 0; i < SceneManager.GetActiveScene().GetRootGameObjects().Length; i++)
		{
			if (SceneManager.GetActiveScene().GetRootGameObjects().ElementAt(i).GetComponent<PlayerMovement2D>())
			{
				player = SceneManager.GetActiveScene().GetRootGameObjects().ElementAt(i);
				break;
			}
		}
	}

	private void Update()
	{
		playerTransform = player.GetComponent<Transform>();

		for (int i = spawnNumber; i < spawnPoints.Count; i++)
		{ 
			if (Mathf.Abs(spawnPoints.ElementAt(i).transform.position.x - playerTransform.position.x) <= 30 && timeBetweenSpawns <= 0)
			{
				if (mmfCount > 0)
				{
					Instantiate(mmf, spawnPoints.ElementAt(i).transform);
					mmfCount--;
				}
				timeBetweenSpawns = 1;
				if (spawnNumber < spawnPoints.Count)
				{
					spawnNumber++;
				}
				else
				{
					spawnNumber = 0;
				}
			}
			else
			{
				if (spawnNumber < spawnPoints.Count)
				{
					spawnNumber++;
				}
				else
				{
					spawnNumber = 0;
				}
			}
		}

		if (timeBetweenSpawns >= 0)
		{
			timeBetweenSpawns -= Time.deltaTime;
		}
		else
		{
			timeBetweenSpawns = 0;
		}
	}

	private void RoundChange()
	{

	}
}
