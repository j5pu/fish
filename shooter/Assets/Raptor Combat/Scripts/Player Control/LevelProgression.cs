/**
 * LevelProgression.cs
 * Author: MutantGopher
 * 9/8/2014
 * 
 * This script handles the basic, constant movement of the player and camera rig.
 * It also handles the terrain generation.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelProgression : MonoBehaviour
{

	public float constantSpeed = 8.0f;							// Root speed that the player and camera will be forced to move forward
	public GameObject existingTerrain;							// The terrain already existin in the scene
	public GameObject[] terrainPrefabs;							// Array of terrain prefabs to be instantiated - one will be selected at random
	public float terrainLength = 400.0f;						// The length of the terrain prefabs that will be instantiated
	public float originalTriggerPoint;							// The point at which the first terrain will be instantiated and the last one will be destroyed
	private float nextInterval;									// The next point to reach before a new terrain prefab

	private List<GameObject> terrains = new List<GameObject>();	// A list of the terrains in the scene
	

	// Use this for initialization
	void Start()
    {
		// Initialize the next interval
		nextInterval = originalTriggerPoint;

		// Add the already existing terrain in the scene to the terrains list, with an empty gameobject before it
		// This ensures that the existing terrain doesn't get removed before the player leaves the view
		terrains.Add(new GameObject());
		terrains.Add(existingTerrain);

	}
	
	// Update is called once per frame
	void Update()
    {
		//  Move the player forward constantly
		transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + constantSpeed * Time.deltaTime);
		
		// Spawn a new terrain the player arrives at the next interval
		if (gameObject.transform.position.z >= nextInterval)
		{
			SpawnNewTerrain();
		}

	}

	void SpawnNewTerrain()
	{
		
		// Instantiate the next terrain
		if (terrainPrefabs.Length > 0 && terrainPrefabs[0] != null)
		{
			GameObject selectedPrefab = terrainPrefabs[Random.Range(0, terrainPrefabs.Length)];	//
			GameObject t = Instantiate(selectedPrefab, new Vector3(0, 0, nextInterval + (terrainLength / 2)), Quaternion.identity) as GameObject;
			terrains.Add(t);
		}

		// Remove the old terrain that the player has already passed
		Destroy(terrains[0]);
		terrains.RemoveAt(0);

		// Set the next interval
		nextInterval += terrainLength;
	}
}
