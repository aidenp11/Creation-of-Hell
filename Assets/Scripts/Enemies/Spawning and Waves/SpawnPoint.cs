using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public bool valid = true;
	private bool barrier = false;

	[SerializeField] List<GameObject> barriers = new List<GameObject>();

	private void Update()
	{
		foreach (GameObject b in barriers)
		{
			if (b.IsDestroyed()) { barrier = false; continue; }
			else { valid = false; barrier = true; break; }
		}
		if (!valid && !barrier) Invoke("ChangeValid", GetComponentInParent<Waves>().ogTimeBetweenSpawns);
	}

	private void ChangeValid()
	{
		valid = true;
	}
}
