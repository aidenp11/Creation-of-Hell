using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waves : MonoBehaviour
{
	[SerializeField] List<GameObject> spawnPoints;
	[SerializeField] GameObject mmf;
	[SerializeField] GameObject huggyBear;
	[SerializeField] GameObject jumpster;

	private List<GameObject> spawnedEnemies = new List<GameObject>();

	private GameObject spawnedmmf;
	private int mmfHealth;

	private GameObject player;
	private Transform playerTransform;

	private int roundNumber;
	public int mmfCount;
	private int originalmmfCount;
	public int huggyBearCount;
	private int originalHuggyBearCount;
	public int jumpsterCount;
	private int originalJumpsterCount;

	public int totalToSpawn;
	public int totalSpawned;

	private float timeBetweenSpawns = 4.5f;
	public float ogTimeBetweenSpawns;

	[SerializeField] GameObject roundChangeText;
	[SerializeField] GameObject roundText;

	private void Start()
	{
		roundNumber = 1;
		roundText.GetComponent<TextMeshProUGUI>().text = roundNumber.ToString();
		roundChangeText.GetComponent<TextMeshProUGUI>().text = "Round: " + roundNumber;
		roundChangeText.SetActive(true);
		Invoke("RoundChangeNumber", 3.5f);
		totalToSpawn = mmfCount + huggyBearCount + jumpsterCount;
		originalmmfCount = mmfCount;
		ogTimeBetweenSpawns = timeBetweenSpawns;
		timeBetweenSpawns = 10;
		mmfHealth = mmf.GetComponent<EnemyBase>().Health;
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

		if (totalSpawned == totalToSpawn && spawnedEnemies.Count > 0)
		{
			foreach (GameObject se in spawnedEnemies)
			{
				if (!se.IsDestroyed()) break;
				else if (se == spawnedEnemies.Last() && se.IsDestroyed())
				{
					totalSpawned = 0;
					spawnedEnemies.Clear();
					roundNumber++;
					roundChangeText.GetComponent<TextMeshProUGUI>().text = "Round " + roundNumber;
					roundChangeText.SetActive(true);
					roundText.GetComponent<TextMeshProUGUI>().text = roundNumber.ToString();
					Invoke("RoundChangeNumber", 3.5f);
					Invoke("RoundChange", 10);
					break;
				}
			}
		}

		for (int i = 0; i < spawnPoints.Count; i++)
		{
			if (Mathf.Abs(spawnPoints.ElementAt(i).transform.position.x - playerTransform.position.x) <= 20 && timeBetweenSpawns <= 0)
			{
				if (mmfCount > 0 && spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid)
				{
					spawnedmmf = Instantiate(mmf, spawnPoints.ElementAt(i).transform);
					spawnedmmf.GetComponent<EnemyBase>().Health = mmfHealth;
					spawnedEnemies.Add(spawnedmmf);
					mmfCount--;
					totalSpawned++;
					timeBetweenSpawns = ogTimeBetweenSpawns;
					spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
				}
				else continue;
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
		originalmmfCount = (int)((float)originalmmfCount * 1.2f);
		mmfCount = originalmmfCount;
		mmfHealth = (int)((float)mmfHealth * 1.1f);
		totalToSpawn = mmfCount + jumpsterCount + huggyBearCount;
		if (ogTimeBetweenSpawns > 0.5f)
		{
			ogTimeBetweenSpawns = ogTimeBetweenSpawns * 0.975f;
		}
		else ogTimeBetweenSpawns = 0.5f;
	}

	private void RoundChangeNumber()
	{
		roundChangeText.SetActive(false);
	}
}
