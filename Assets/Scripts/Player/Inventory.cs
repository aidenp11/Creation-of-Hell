using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] GameObject startingWeapon;
    [SerializeField] Transform handPosition;
    public GameObject activeWeapon;
    private List<GameObject> currentWeapons = new List<GameObject>();

    [Header("Other Stuff")]
    [SerializeField] int score;

	private void Start()
	{
        activeWeapon = startingWeapon;
        Instantiate(activeWeapon, handPosition);
        currentWeapons.Add(activeWeapon);
	}
}
