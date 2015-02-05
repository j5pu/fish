using UnityEngine;
using System.Collections;

public class LockPositionOnAxis : MonoBehaviour
{
	public bool lockX = false;
	public bool lockY = true;
	public bool lockZ = false;

	public float lockXPosition = 0.0f;
	public float lockYPosition = 0.0f;
	public float lockZPosition = 0.0f;


	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void LateUpdate()
	{
		Vector3 newPosition = transform.position;
		
		if (lockX)
		{
			newPosition = new Vector3(lockXPosition, newPosition.y, newPosition.z);
		}

		if (lockY)
		{
			newPosition = new Vector3(newPosition.x, lockYPosition, newPosition.z);
		}

		if (lockZ)
		{
			newPosition = new Vector3(newPosition.x, newPosition.y, lockZPosition);
		}

		transform.position = newPosition;
	}

}
