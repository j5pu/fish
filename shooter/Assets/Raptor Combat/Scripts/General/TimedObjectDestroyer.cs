using UnityEngine;
using System.Collections;

public class TimedObjectDestroyer : MonoBehaviour
{
	public float lifetime = 5.0f;
	private float timer = 0.0f;
	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;

		if (timer >= lifetime)
			Destroy(gameObject);
	}
}
