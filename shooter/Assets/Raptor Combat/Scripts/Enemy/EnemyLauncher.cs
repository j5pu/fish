using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyLauncher : MonoBehaviour
{
	public GameObject missile;				// The missile prefab to instantiate
	public AudioClip fireSound;				// The sound to be played when a missile is fired
	public float fireFrequency = 1.5f;		// The frequency at which this launcher will launch missiles
	private float fireTimer = 0.0f;


	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		// Update the fire timer
		fireTimer += Time.deltaTime;

		// Launch missile when the timer runs out
		if (fireTimer >= fireFrequency)
		{
			Launch();
		}
	}

	// Launch a missile
	void Launch()
	{
		// Reset the fire timer
		fireTimer = 0.0f;

		// Play the launch sound
		audio.PlayOneShot(fireSound);
		
		if (missile != null)
		{
			// Instantiate the missile prefab
			Instantiate(missile, transform.position, transform.rotation);
		}
	}

}
