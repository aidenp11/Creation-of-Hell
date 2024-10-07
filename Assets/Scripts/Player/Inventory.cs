using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Weapons")]
    [SerializeField] GameObject startingWeapon;
    public GameObject activeWeapon;
    private GameObject[] currentWeapons;

    [Header("Other Stuff")]
    [SerializeField] int score;

	private void Start()
	{
        activeWeapon = startingWeapon;
        activeWeapon.SetActive(true);
	}
}
