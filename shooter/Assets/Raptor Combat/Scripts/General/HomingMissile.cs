/*
 * This is a homing missile script, but it can be used for other projectiles besides homing missiles.
 * 
 */

using UnityEngine;
using System.Collections;

public class HomingMissile : MonoBehaviour
{
	public float damage = 100.0f;										// The amount of damage to be applied (only for Direct damage type)
	public float speed = 10.0f;											// The speed at which this projectile will move
	public float lifetime = 10.0f;										// The maximum time (in seconds) before the projectile is destroyed

	public bool seekTargets = false;									// If set to true, this projectile will seek enemy targets automatically
	public float seekRate = 1.0f;										// The rate at which the projectile will turn to seek enemies
	public string seekTag = "Enemy";									// The projectile will seek gameobjects with this tag
	public GameObject explosion;										// The explosion to be instantiated when this projectile hits something
	public float targetListUpdateRate = 1.0f;							// The rate at which the projectile will update its list of all enemies to target

	private float lifeTimer = 0.0f;										// The timer to keep track of how long this projectile has been in existence
	private float targetListUpdateTimer = 0.0f;							// The timer to keep track of how long it's been since the enemy list was last updated
	private GameObject[] enemyList;										// An array to hold possible targets

	private bool firstIteration = true;									// Used in determining if a target has ever beeen found before when the missile is spawned

	void Start()
	{
		UpdateEnemyList();
	}

	// Update is called once per frame
	void Update()
	{
		// Update the timer
		lifeTimer += Time.deltaTime;

		// Destroy the projectile if the time is up
		if (lifeTimer >= lifetime)
		{
			Explode(transform.position);
		}

		// Make the projectile move
		//rigidbody.velocity = transform.forward * speed;
		transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));

		// Make the projectile seek nearby targets if the projectile type is set to seeker
		if (seekTargets)
		{
			Seek();
		}
	}

	void UpdateEnemyList()
	{
		enemyList = GameObject.FindGameObjectsWithTag(seekTag);
		targetListUpdateTimer = 0.0f;
	}

	void OnCollisionEnter(Collision col)
	{
		// If the projectile collides with something, call the Hit() function
		Hit(col);
	}

	void Hit(Collision col)
	{
		// Apply damage
		if (col.collider.transform.parent)
		{
			if (col.collider.transform.parent.gameObject.GetComponent<Health>() != null)
			{
				col.collider.transform.parent.gameObject.GetComponent<Health>().ChangeHealth(-damage);

				// Make the projectile explode
				Explode(col.contacts[0].point);
			}
		}
	}

	void Explode(Vector3 position)
	{
		// Instantiate the explosion
		if (explosion != null)
		{
			Instantiate(explosion, position, Quaternion.identity);
		}

		// Destroy this projectile
		Destroy(gameObject);
	}


	void Seek()
	{
		// Keep the timer updating
		targetListUpdateTimer += Time.deltaTime;

		// If the targetListUpdateTimer has reached the targetListUpdateRate, update the enemy list and restart the timer
		if (targetListUpdateTimer >= targetListUpdateRate)
		{
			UpdateEnemyList();
		}

		if (enemyList != null)
		{
			// Choose a target to "seek" or rotate toward
			float greatestDotSoFar = -1.0f;
			Vector3 target = transform.position + transform.forward * 1000;
			foreach (GameObject enemy in enemyList)
			{
				if (enemy != null)
				{
					Vector3 direction = enemy.transform.position - transform.position;
					float dot = Vector3.Dot(direction.normalized, transform.forward);
					if (dot > greatestDotSoFar && ((dot > 0.6f || dot < -0.6f) || firstIteration))
					{
						target = enemy.transform.position;
						greatestDotSoFar = dot;
					}
				}
				firstIteration = false;
			}

			// Rotate the projectile to look at the target
			Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
			transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * seekRate);
		}

	}


}