/*
 * MachineGun.cs
 * Author: MutantGopher
 * 10/13/2014
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MachineGun : MonoBehaviour
{
	// Auto
	public bool fullAuto = true;						// How does this weapon fire - semi-auto or full-auto

	// General
	public GameObject weaponModel;						// The actual mesh for this weapon
	public Transform raycastStartSpot;					// The spot that the raycasting weapon system should use as an origin for rays

	// Range
	public float range = 9999.0f;						// How far this weapon can shoot (for raycast and beam)

	// Power
	public float power = 10.0f;							// The power of this weapon (the amount of damage caused)

	// Rate of Fire
	public float rateOfFire = 10;						// The number of rounds this weapon fires per second
	private float actualROF;							// The frequency between shots based on the rateOfFire
	private float fireTimer;							// Timer used to fire at a set frequency

	// Ammo
	public bool infiniteAmmo = true;					// Whether or not this weapon should have unlimited ammo
	public int ammoCapacity = 12;						// The number of rounds this weapon can fire before it has to reload
	public int shotPerRound = 1;						// The number of "bullets" that will be fired on each round.  Usually this will be 1, but set to a higher number for things like shotguns with spread
	private int currentAmmo;							// How much ammo the weapon currently has
	public float reloadTime = 2.0f;						// How much time it takes to reload the weapon
	public bool showCurrentAmmo = true;					// Whether or not the current ammo should be displayed in the GUI

	// Accuracy
	public float accuracy = 80.0f;						// How accurate this weapon is on a scale of 0 to 100
	private float currentAccuracy;						// Holds the current accuracy.  Used for varying accuracy based on speed, etc.
	public float accuracyDropPerShot = 1.0f;			// How much the accuracy will decrease on each shot
	public float accuracyRecoverRate = 0.1f;			// How quickly the accuracy recovers after each shot (value between 0 and 1)

	// Effects
	public bool spitShells = false;						// Whether or not this weapon should spit shells out of the side
	public GameObject shell;							// A shell prop to spit out the side of the weapon
	public float shellSpitForce = 10.0f;				// The force with which shells will be spit out of the weapon
	public Transform shellSpitPosition;					// The spot where the weapon should spit shells from
	public bool makeMuzzleEffects = true;				// Whether or not the weapon should make muzzle effects
	public GameObject[] muzzleEffects =
		new GameObject[] { null };			            // Effects to appear at the muzzle of the gun (muzzle flash, smoke, etc.)
	public Transform muzzleEffectsPosition;				// The spot where the muzzle effects should appear from
	public bool makeHitEffects = true;					// Whether or not the weapon should make hit effects
	public GameObject[] hitEffects =
		new GameObject[] { null };						// Effects to be displayed where the "bullet" hit

	// Audio
	public AudioClip fireSound;							// Sound to play when the weapon is fired
	public AudioClip reloadSound;						// Sound to play when the weapon is reloading

	// Other
	private bool canFire = true;						// Whether or not the weapon can currently fire (used for semi-auto weapons)


	// Use this for initialization
	void Start()
	{
		// Calculate the actual ROF to be used in the weapon systems.  The rateOfFire variable is
		// designed to make it easier on the user - it represents the number of rounds to be fired
		// per second.  Here, an actual ROF decimal value is calculated that can be used with timers.
		if (rateOfFire != 0)
			actualROF = 1.0f / rateOfFire;
		else
			actualROF = 0.01f;

		// Make sure the fire timer starts at 0
		fireTimer = 0.0f;

		// Start the weapon off with a full magazine
		currentAmmo = ammoCapacity;

		// Initialize current accuracy
		currentAccuracy = accuracy;

		// Give this weapon an audio source component if it doesn't already have one
		if (GetComponent<AudioSource>() == null)
		{
			gameObject.AddComponent(typeof(AudioSource));
		}

		// Make sure raycastStartSpot isn't null
		if (raycastStartSpot == null)
			raycastStartSpot = gameObject.transform;

		// Make sure muzzleEffectsPosition isn't null
		if (muzzleEffectsPosition == null)
			muzzleEffectsPosition = gameObject.transform;

		// Make sure weaponModel isn't null
		if (weaponModel == null)
			weaponModel = gameObject;

	}

	// Update is called once per frame
	void Update()
	{
		// Calculate the current accuracy for this weapon
		currentAccuracy = Mathf.Lerp(currentAccuracy, accuracy, accuracyRecoverRate * Time.deltaTime);

		// Update the fireTimer
		fireTimer += Time.deltaTime;

		// Fire regularly on intervals of actualROF
		if (Input.GetButton("Fire1") && fireTimer >= actualROF && canFire)
		{
			Fire();
		}

		// Reload if the weapon is out of ammo
		if (currentAmmo <= 0)
			Reload();


		// If the weapon is semi-auto and the user lets up on the button, set canFire to true
		if (Input.GetButtonUp("Fire1"))
			canFire = true;
	}

	void OnGUI()
	{
		// Ammo Display
		if (showCurrentAmmo)
		{
			GUI.Label(new Rect(10, Screen.height - 30, 100, 20), "Ammo: " + currentAmmo);
			
		}
	}


	// Raycasting system
	void Fire()
	{
		// Reset the fireTimer to 0 (for ROF)
		fireTimer = 0.0f;

		// If this is a semi-automatic weapon, set canFire to false (this means the weapon can't fire again until the player lets up on the fire button)
		if (!fullAuto)
			canFire = false;

		// Subtract 1 from the current ammo
		if (!infiniteAmmo)
			currentAmmo--;

		// Determine the point from which the raycast will start
		Vector3 startSpot = raycastStartSpot.position;
		Vector3 endpoint = transform.position + (transform.forward * 1000000);

		// Fire once for each shotPerRound value
		for (int i = 0; i < shotPerRound; i++)
		{
			// Calculate accuracy for this shot
			float accuracyVary = (100 - currentAccuracy) / 1000;
			Vector3 direction = raycastStartSpot.forward;
			direction = new Vector3(direction.x + UnityEngine.Random.Range(-accuracyVary, accuracyVary), direction.y, direction.z);
			currentAccuracy -= accuracyDropPerShot;
			if (currentAccuracy <= 0.0f)
				currentAccuracy = 0.0f;

			endpoint = raycastStartSpot.position + (direction * 1000000);

			// The ray that will be used for this shot
			Ray ray = new Ray(startSpot, direction);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, range))
			{
				// Damage
				if (hit.collider.gameObject.GetComponent<Health>())
				{
					hit.collider.gameObject.GetComponent<Health>().ChangeHealth(-power);
				}
				else if (hit.collider.transform.parent != null)		// First check to make sure there is a parent
				{
					if (hit.collider.transform.parent.gameObject.GetComponent<Health>())
					{
						hit.collider.transform.parent.gameObject.GetComponent<Health>().ChangeHealth(-power);
					}
				}

				// Hit Effects
				if (makeHitEffects)
				{
					foreach (GameObject hitEffect in hitEffects)
					{
						if (hitEffect != null)
							Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
					}
				}
			}
		}

		// Muzzle flash effects
		if (makeMuzzleEffects)
		{
			foreach (GameObject fx in muzzleEffects)
			{
				if (fx != null)
				{
					// Instantiate the muzzle FX
					GameObject m = Instantiate(fx, muzzleEffectsPosition.position, muzzleEffectsPosition.rotation) as GameObject;

					// Set the line renderer to point where the gun is shooting
					if (m.GetComponent<LineRenderer>() != null)
					{
						m.GetComponent<LineRenderer>().SetPosition(1, endpoint);
					}
				}
			}
		}
		
		// Play the gunshot sound
		audio.PlayOneShot(fireSound);
	}


	// Reload the weapon
	void Reload()
	{
		currentAmmo = ammoCapacity;
		fireTimer = -reloadTime;
		audio.PlayOneShot(reloadSound);
	}

	
	// Find a mesh renderer in a specified gameobject, it's children, or its parents
	MeshRenderer FindMeshRenderer(GameObject go)
	{
		MeshRenderer hitMesh;

		// Use the MeshRenderer directly from this GameObject if it has one
		if (go.renderer != null)
		{
			hitMesh = go.GetComponent<MeshRenderer>();
		}

		// Try to find a child or parent GameObject that has a MeshRenderer
		else
		{
			// Look for a renderer in the child GameObjects
			hitMesh = go.GetComponentInChildren<MeshRenderer>();

			// If a renderer is still not found, try the parent GameObjects
			if (hitMesh == null)
			{
				GameObject curGO = go;
				while (hitMesh == null && curGO.transform != curGO.transform.root)
				{
					curGO = curGO.transform.parent.gameObject;
					hitMesh = curGO.GetComponent<MeshRenderer>();
				}
			}
		}

		return hitMesh;
	}
}


