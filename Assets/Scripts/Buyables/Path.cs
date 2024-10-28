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

	[SerializeField] RemoveType removeType;
	[SerializeField] float secondsToDo;

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
					Invoke("DestroySelf", secondsToDo);
					break;
				case RemoveType.ANIMATE:
					break;
				case RemoveType.MOVEVERTICAL:
					break;
				case RemoveType.MOVEROTATE:
					break;
			}
		}
	}

	private void DestroySelf()
	{
		Destroy(gameObject);
	}
}
