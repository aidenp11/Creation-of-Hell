using Unity.Cinemachine;
using Unity.Properties;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Path : MonoBehaviour
{
	public bool remove;
	public enum RemoveType
	{
		DESTROY,
		ANIMATE,
		MOVEVERTICAL,
		MOVEROTATE
	}

	[Header("For removable obstacles")]
	[SerializeField] RemoveType removeType;
	[SerializeField] float secondsToDo;

	[Header("For Moving Obstacles")]
	[SerializeField] float distanceToMove;
	[SerializeField] float moveRate;
	private float distanceMoved = 0;

	[Header("For Rotating Obstacles")]
	[SerializeField] float distanceToRotate;
	[SerializeField] float rotateRate;
	private float distanceRotated = 0;

	[SerializeField] Animator animator;

	private void Start()
	{
		remove = false;
	}

	private void Update()
	{
		if (remove)
		{
			switch (removeType)
			{
				case RemoveType.DESTROY:
					animator.SetTrigger("Destroy");
					Invoke("DestroySelf", secondsToDo);
					break;
				case RemoveType.ANIMATE:
					break;
				case RemoveType.MOVEVERTICAL:
					if (distanceToMove < 0)
					{
						if (distanceToMove < distanceMoved)
						{
							transform.position += new Vector3(0, -moveRate, 0);
						}
						distanceMoved -= moveRate;
					}
					else
					{
						if (distanceToMove > distanceMoved)
						{
							transform.position += new Vector3(0, moveRate, 0);
						}
						distanceMoved += moveRate;
					}
					break;
				case RemoveType.MOVEROTATE:
					if (distanceToRotate < 0)
					{
						if (distanceToRotate < distanceRotated)
						{
							transform.eulerAngles += new Vector3(0, 0, -rotateRate);
						}
						distanceRotated -= rotateRate;
					}
					else
					{
						if (distanceToRotate > distanceRotated)
						{
							transform.eulerAngles += new Vector3(0, 0, rotateRate);
						}
						distanceRotated += rotateRate;
					}
					break;
			}
		}
	}

	private void DestroySelf()
	{
		Destroy(gameObject);
	}
}
