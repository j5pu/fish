using UnityEngine;
using System.Collections;

public class EnemySpawning : MonoBehaviour
{
	public bool spawn = true;						// Determines whether or not enemies should be spawned - useful for quick testing in the editor
	public float spawnTime = 2.0f;					// The time between spawnings
	public float variation = 1.0f;					// The maximum positive or negative variation between spawn times
	public float minimumTime = 0.2f;				// The minimum time between spawnings
	private float timeToWait = 3.0f;				// The current time to wait (varies) before the next spawn
	private float spawnTimer = 0.0f;				// Tracks amount of time between spawnings
	private float distanceFromCamera;				// The height difference between the spawn point and the camera

	public GameObject[] enemies;					// An array of enemy aircraft prefabs, in order of least deadly to most

	public float difficultyIncreaseTime = 10.0f;	// The time, in seconds, between difficulty increases
	private float difficultyTimer = 0.0f;			// The timer used to keep track of increasing difficulty

	public Scorekeeper scorekeeper;					// The reference to the scorekeeper - used to make sure enemies don't continue to spawn after the game is over


	// Use this for initialization
	void Start()
	{
		// Determine the amount of time to wait before spawning for this level
		timeToWait = spawnTime;
		
		// Initialize the distanceFromCamera variable
		distanceFromCamera = Camera.main.transform.position.y - transform.position.y;
	}
	
	// Update is called once per frame
	void Update()
	{
		// Update the difficulty timer
		difficultyTimer += Time.deltaTime;

		// Increase the difficulty level when the timer runs out
		if (difficultyTimer >= difficultyIncreaseTime)
		{
			IncreaseDifficulty();
		}

		// Update the spawning timer
		spawnTimer += Time.deltaTime;

		// Spawn an enemy aircraft when the timer runs out, assuming the game is not yet over
		if (spawnTimer >= timeToWait && spawn && !scorekeeper.gameOver)
		{
			Spawn();
		}
	}

	// Increase the difficulty level
	void IncreaseDifficulty()
	{
		// Reset the difficulty timer
		difficultyTimer = 0.0f;
		
		spawnTime -= spawnTime / 10.0f;
		if (spawnTime < 0)
			spawnTime = 0;
	}

	// Spawn an enemy
	void Spawn()
	{
		// Reset the spawn timer
		spawnTimer = 0.0f;

		// Determine the amount of time to wait before the next spawn
		timeToWait = spawnTime + Random.Range(-variation, variation);
		if (timeToWait < minimumTime)
			timeToWait = minimumTime;

		// Find the position where the enemy prefab should be spawned
		float xRange = transform.position.x - Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distanceFromCamera)).x;
		float xPos = Random.Range(transform.position.x - xRange, transform.position.x + xRange);
		Vector3 spawnPoint = new Vector3(xPos, transform.position.y, transform.position.z);

		// Determine which enemy aircraft prefab to spawn
		int enemyToSpawn = Random.Range(0, enemies.Length);

		// Spawn an enmy prefab
		if (enemies[0] != null)
		{
			Instantiate(enemies[enemyToSpawn], spawnPoint, transform.rotation);
		}

	}

}
