using UnityEngine;

public class Skins : MonoBehaviour
{
    [SerializeField] IntVariable skinNum;

    [SerializeField] GameObject arm;

    [SerializeField] Sprite soldier;
    [SerializeField] Sprite soldierHand;
    [SerializeField] Sprite miku;
    [SerializeField] Sprite mikuHand;
    [SerializeField] RuntimeAnimatorController soldierController;
    [SerializeField] RuntimeAnimatorController mikuController;

	private void Start()
	{
        if (skinNum == 0)
        {
            GetComponent<SpriteRenderer>().sprite = soldier;
            arm.GetComponent<SpriteRenderer>().sprite = soldierHand;
            GetComponent<Animator>().runtimeAnimatorController = soldierController;
        }
        else if (skinNum == 1)
        {
			GetComponent<SpriteRenderer>().sprite = miku;
			arm.GetComponent<SpriteRenderer>().sprite = mikuHand;
			GetComponent<Animator>().runtimeAnimatorController = mikuController;
		}
	}
}
