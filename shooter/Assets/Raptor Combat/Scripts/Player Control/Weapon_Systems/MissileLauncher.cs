using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MissileLauncher : MonoBehaviour
{
	public GameObject missile;				// The missile prefab to instantiate
	public int missilesRemaining;			// The number of missile remaining
	public Text missilesUI;					// The UI text element that shows how many missiles are remaining
	public AudioClip fireSound;				// The sound to be played when a missile is fired


	// Use this for initialization
	void Start()
	{
		// Update the UI element that represents the missiles remaining
		if (missilesUI != null)
		{
			missilesUI.text = "" + missilesRemaining;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		if (Input.GetButtonUp("Fire2") && missilesRemaining > 0)
		{
			Launch();
		}
	}

	// Launch a missile
	void Launch()
	{
		// Play the launch sound
		audio.PlayOneShot(fireSound);
		
		if (missile != null)
		{
			// Instantiate the missile prefab
			Instantiate(missile, transform.position, Quaternion.identity);
			
			// Decrease the missile count by 1
			ChangeMissilesRemaining(-1);
		}
	}

	// Change the number of missiles currently available
	public void ChangeMissilesRemaining(int num)
	{
		missilesRemaining += num;
		
		// Make sure the number of missiles doesn't go below zero
		if (missilesRemaining < 0)
			missilesRemaining = 0;

		// Update the UI element that represents the missiles remaining
		if (missilesUI != null)
		{
			missilesUI.text = "" + missilesRemaining;
		}
	}
}
