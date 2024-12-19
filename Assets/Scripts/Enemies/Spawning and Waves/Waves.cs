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
	private GameObject spawnedJumpster;
	private GameObject spawnedHuggyBear;
	private int mmfHealth;
	private int jumpsterHealth;
	private int huggyBearHealth;

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

	public int amountSpawned;

	private float timeBetweenSpawns = 4f;
	public float ogTimeBetweenSpawns;

	[SerializeField] GameObject roundChangeText;
	[SerializeField] GameObject roundText;
	[SerializeField] GameObject monstersLeftText;
	[SerializeField] AudioSource roundChangeJingle;

	private void Start()
	{
		roundNumber = 1;
		roundChangeJingle.Play();
		roundText.GetComponent<TextMeshProUGUI>().text = roundNumber.ToString();
		roundChangeText.GetComponent<TextMeshProUGUI>().text = "Round: " + roundNumber;
		roundChangeText.SetActive(true);
		Invoke("RoundChangeNumber", 3.5f);
		totalToSpawn = mmfCount + huggyBearCount + jumpsterCount;
		originalmmfCount = mmfCount;
		ogTimeBetweenSpawns = timeBetweenSpawns;
		timeBetweenSpawns = 10;
		mmfHealth = mmf.GetComponent<EnemyBase>().Health;
		jumpsterHealth = jumpster.GetComponent<EnemyBase>().Health;
		huggyBearHealth = huggyBear.GetComponent<EnemyBase>().Health;
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
		monstersLeftText.GetComponent<TextMeshProUGUI>().text = "Monsters Left: " + (mmfCount + jumpsterCount + huggyBearCount).ToString();
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
					roundChangeJingle.Play();
					Invoke("RoundChangeNumber", 3.5f);
					Invoke("RoundChange", 10);
					break;
				}
			}
		}

		if (amountSpawned < 30)
		{
			for (int i = 0; i < spawnPoints.Count; i++)
			{
				if (Mathf.Abs(spawnPoints.ElementAt(i).transform.position.x - playerTransform.position.x) <= 13.5f && timeBetweenSpawns <= 0)
				{
					if ((mmfCount > 0 && jumpsterCount <= 0 && huggyBearCount <= 0) && spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid)
					{
						spawnedmmf = Instantiate(mmf, spawnPoints.ElementAt(i).transform);
						spawnedmmf.GetComponent<EnemyBase>().Health = mmfHealth;
						spawnedEnemies.Add(spawnedmmf);
						mmfCount--;
						totalSpawned++;
						timeBetweenSpawns = ogTimeBetweenSpawns;
						spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
					}
					else if ((jumpsterCount > 0 && mmfCount <= 0 && huggyBearCount <= 0) && spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid)
					{
						spawnedJumpster = Instantiate(jumpster, spawnPoints.ElementAt(i).transform);
						spawnedJumpster.GetComponent<EnemyBase>().Health = jumpsterHealth;
						spawnedEnemies.Add(spawnedJumpster);
						jumpsterCount--;
						totalSpawned++;
						timeBetweenSpawns = ogTimeBetweenSpawns;
						spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
					}
					else if ((huggyBearCount > 0 && mmfCount <= 0 && jumpsterCount <= 0) && spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid)
					{
						spawnedHuggyBear = Instantiate(huggyBear, spawnPoints.ElementAt(i).transform);
						spawnedHuggyBear.GetComponent<EnemyBase>().Health = huggyBearHealth;
						spawnedEnemies.Add(spawnedHuggyBear);
						huggyBearCount--;
						totalSpawned++;
						timeBetweenSpawns = ogTimeBetweenSpawns;
						spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
					} 
					else if ((mmfCount > 0 && jumpsterCount > 0 && huggyBearCount > 0) && spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid)
					{
						int random = Random.Range(1, mmfCount + jumpsterCount + huggyBearCount);
						if (random <= mmfCount)
						{
							spawnedmmf = Instantiate(mmf, spawnPoints.ElementAt(i).transform);
							spawnedmmf.GetComponent<EnemyBase>().Health = mmfHealth;
							spawnedEnemies.Add(spawnedmmf);
							mmfCount--;
							totalSpawned++;
							timeBetweenSpawns = ogTimeBetweenSpawns;
							spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
						} else if (random <= mmfCount + jumpsterCount)
						{
							spawnedJumpster = Instantiate(jumpster, spawnPoints.ElementAt(i).transform);
							spawnedJumpster.GetComponent<EnemyBase>().Health = jumpsterHealth;
							spawnedEnemies.Add(spawnedJumpster);
							jumpsterCount--;
							totalSpawned++;
							timeBetweenSpawns = ogTimeBetweenSpawns;
							spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
						} else if (random <= mmfCount + jumpsterCount + huggyBearCount)
						{
							spawnedHuggyBear = Instantiate(huggyBear, spawnPoints.ElementAt(i).transform);
							spawnedHuggyBear.GetComponent<EnemyBase>().Health = huggyBearHealth;
							spawnedEnemies.Add(spawnedHuggyBear);
							huggyBearCount--;
							totalSpawned++;
							timeBetweenSpawns = ogTimeBetweenSpawns;
							spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
						}
					}
					else if ((mmfCount > 0 && jumpsterCount > 0 && huggyBearCount <= 0) && spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid)
					{
						int random = Random.Range(1, mmfCount + jumpsterCount);
						if (random <= mmfCount)
						{
							spawnedmmf = Instantiate(mmf, spawnPoints.ElementAt(i).transform);
							spawnedmmf.GetComponent<EnemyBase>().Health = mmfHealth;
							spawnedEnemies.Add(spawnedmmf);
							mmfCount--;
							totalSpawned++;
							timeBetweenSpawns = ogTimeBetweenSpawns;
							spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
						}
						else if (random <= mmfCount + jumpsterCount)
						{
							spawnedJumpster = Instantiate(jumpster, spawnPoints.ElementAt(i).transform);
							spawnedJumpster.GetComponent<EnemyBase>().Health = jumpsterHealth;
							spawnedEnemies.Add(spawnedJumpster);
							jumpsterCount--;
							totalSpawned++;
							timeBetweenSpawns = ogTimeBetweenSpawns;
							spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
						}
					}
					else if ((mmfCount > 0 && jumpsterCount <= 0 && huggyBearCount > 0) && spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid)
					{
						int random = Random.Range(1, mmfCount + huggyBearCount);
						if (random <= mmfCount)
						{
							spawnedmmf = Instantiate(mmf, spawnPoints.ElementAt(i).transform);
							spawnedmmf.GetComponent<EnemyBase>().Health = mmfHealth;
							spawnedEnemies.Add(spawnedmmf);
							mmfCount--;
							totalSpawned++;
							timeBetweenSpawns = ogTimeBetweenSpawns;
							spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
						}
						else if (random <= mmfCount + huggyBearCount)
						{
							spawnedHuggyBear = Instantiate(huggyBear, spawnPoints.ElementAt(i).transform);
							spawnedHuggyBear.GetComponent<EnemyBase>().Health = huggyBearHealth;
							spawnedEnemies.Add(spawnedHuggyBear);
							huggyBearCount--;
							totalSpawned++;
							timeBetweenSpawns = ogTimeBetweenSpawns;
							spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
						}
					}
					else if ((mmfCount <= 0 && jumpsterCount > 0 && huggyBearCount > 0) && spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid)
					{
						int random = Random.Range(1, jumpsterCount + huggyBearCount);
						if (random <= jumpsterCount)
						{
							spawnedJumpster = Instantiate(jumpster, spawnPoints.ElementAt(i).transform);
							spawnedJumpster.GetComponent<EnemyBase>().Health = jumpsterHealth;
							spawnedEnemies.Add(spawnedJumpster);
							jumpsterCount--;
							totalSpawned++;
							timeBetweenSpawns = ogTimeBetweenSpawns;
							spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
						}
						else if (random <= jumpsterCount + huggyBearCount)
						{
							spawnedHuggyBear = Instantiate(huggyBear, spawnPoints.ElementAt(i).transform);
							spawnedHuggyBear.GetComponent<EnemyBase>().Health = huggyBearHealth;
							spawnedEnemies.Add(spawnedHuggyBear);
							huggyBearCount--;
							totalSpawned++;
							timeBetweenSpawns = ogTimeBetweenSpawns;
							spawnPoints.ElementAt(i).GetComponent<SpawnPoint>().valid = false;
						}
					}
					else continue;
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

	private void FixedUpdate()
	{
		int enemiesSpawned = 0;
		for (int i = 0; i < spawnedEnemies.Count; i++)
		{
			if (spawnedEnemies.ElementAt(i).IsDestroyed())
			{
				continue;
			}
			else
			{
				enemiesSpawned++;
			}
		}
		amountSpawned = enemiesSpawned;
	}

	private void RoundChange()
	{
		if (roundNumber == 5)
		{
			jumpsterCount = 12;
			originalJumpsterCount = jumpsterCount;
		}
		if (roundNumber == 8)
		{
			huggyBearCount = 22;
			originalHuggyBearCount = huggyBearCount;
		}
		float randommmf = Random.Range(1.15f, 1.35f);
		originalmmfCount = (int)((float)originalmmfCount * randommmf);
		mmfCount = originalmmfCount;
		mmfHealth = (int)((float)mmfHealth * 1.25f);
		if (roundNumber > 5)
		{
			float randomjumpster = Random.Range(1.15f, 1.35f);
			originalJumpsterCount = (int)((float)originalJumpsterCount * randomjumpster);
			jumpsterCount = originalJumpsterCount;
			jumpsterHealth = (int)((float)jumpsterHealth * 1.25f);
		}
		if (roundNumber > 8)
		{
			float randomhuggy = Random.Range(1.15f, 1.35f);
			originalHuggyBearCount = (int)((float)originalHuggyBearCount * randomhuggy);
			huggyBearCount = originalHuggyBearCount;
			huggyBearHealth = (int)((float)huggyBearHealth * 1.25f);
		}
		totalToSpawn = mmfCount + jumpsterCount + huggyBearCount;
		if (ogTimeBetweenSpawns > 0.35f)
		{
			ogTimeBetweenSpawns = ogTimeBetweenSpawns * 0.85f;
		}
		else ogTimeBetweenSpawns = 0.35f;
	}

	private void RoundChangeNumber()
	{
		roundChangeText.SetActive(false);
	}
}
