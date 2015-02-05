using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{
	public float violence = 2.0f;				// The force of the explosion - this value will be used to determine camera shaking effects
	private float rotationViolence;				// This secondary value will be used to determine the rotation values for the camera shaking effects
	public GameObject[] debris;					// Some of these prefabs will be spawned as debris fragments at the explosion
	public int minDebrisFragments = 3;			// The minimum number of fragments to be spawned
	public int maxDebrisFragments = 6;			// The maximum number of fragments to be spawned

	// Use this for initialization
	void Start()
	{
		// Play the explosion sound
		audio.Play();

		// Determine the rotation violence
		rotationViolence = violence / 200.0f;

		// Make the camera shake
		Camera.main.GetComponent<Vibration>().StartShakingRandom(-violence, violence, -rotationViolence, rotationViolence);

		
		// Instantiate debris at the explosion spot
		if (debris.Length > 0)
		{
			int numFrags = Random.Range(minDebrisFragments, maxDebrisFragments);

			for (int i = 0; i <= numFrags; i++)
			{
				int indexToSpawn = Random.Range(0, debris.Length);
				if (debris[indexToSpawn] != null)
				{
					GameObject fragment = Instantiate(debris[indexToSpawn], transform.position, transform.rotation) as GameObject;

					// Add force and torque to the fragments
					if (fragment.rigidbody != null)
					{
						fragment.rigidbody.AddRelativeForce(Random.Range(1000.0f, -1000.0f), Random.Range(1000.0f, -1000.0f), Random.Range(1000.0f, -1000.0f));
						fragment.rigidbody.AddRelativeTorque(Random.Range(1000.0f, -1000.0f), Random.Range(1000.0f, -1000.0f), Random.Range(1000.0f, -1000.0f));
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
}
