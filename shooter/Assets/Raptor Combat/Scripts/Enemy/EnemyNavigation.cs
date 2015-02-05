/*
 * EnemyNavigation.cs
 * Author: MutantGopher
 * 9/8/2014
 * 
 * This script handles the enemy movement
 */

using UnityEngine;
using System.Collections;

public class EnemyNavigation : MonoBehaviour
{
	public float speed = 10.0f;					// The speed at which the aircraft moves
	public float acceleration = 0.5f;			// The acceleration speed of the player's aircraft
	public float tiltSpeed = 0.6f;				// The speed at which the aircraft will tilt (right/left).  This does not affect movement speed.
	public float minTurnTime = 1.5f;			// The minimum time between turns (horizontal movements)
	public float maxTurnTime = 2.5f;			// The maximum time between turns (horizontal movements)
	private float nextTurn;						// The time to pass before the next turn
	private float turnTimer = 0;				// The timer used to track turn intervals
	public float turnDistance = 2.0f;			// The distance to move horizontally
	private bool turningLeft = false;			// Whether or not the aircraft is currently moving left
	private bool turningRight = false;			// Whether or not the aircraft is currently moving right
	private float xTarget;						// Used to hold the target x position each time the aircraft moves horizontally
	private bool tilt = false;					// Determines whether or not the aircraft can currently tilt
	private Vector3 startingRotation;			// The starting rotation of the aircraft
	

	// Use this for initialization
	void Start()
	{
		// Get the proper rotations for this plane GameObject to be used for tilting
		startingRotation = transform.eulerAngles;
		
		// Initialize the next turn variable
		nextTurn = Random.Range(minTurnTime, maxTurnTime);

	}
	
	// Update is called once per frame
	void Update()
	{
		// Handle aircraft navigation
		Navigate();

	}

	void Navigate()
	{
		
		// Move forward
		transform.Translate(0, 0, speed * Time.deltaTime);

		// Update the turn timer
		turnTimer += Time.deltaTime;

		// Randomly move horizontally
		if (turnTimer >= nextTurn)
		{
			if (Random.Range(0, 2) == 0)		// This random.range uses 2 as the max number because it's exclusive - only values of 0 and 1 can be generated
				StartTurning(false);
			else
				StartTurning(true);
		}

		// Keep turning
		if (turningLeft || turningRight)
		{
			Turning();
		}

		// Tilt the aircraft based on horizontal movement
		float angle = 0.0f;
		if (turningRight && tilt)
			angle = -80.0f;
		if (turningLeft && tilt)
			angle = 80.0f;
		transform.eulerAngles = new Vector3(startingRotation.x, startingRotation.y, Mathf.LerpAngle(transform.eulerAngles.z, angle, tiltSpeed * Time.deltaTime));
	}

	void StartTurning(bool left)
	{
		// Allow the aircraft to tilt
		tilt = true;

		// Make sure the aircraft doesn't move out of the camera view
		// The leftEdge is the position on the left side of the camera view at the same height as the aircraft
		Vector3 leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.y - transform.position.y));
		// The rightEdge is the position on the right side of the camera view at the same height as the aircraft
		Vector3 rightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, Camera.main.transform.position.y - transform.position.y));

		float leftDistance = Mathf.Abs(transform.position.x - leftEdge.x);		// The distance between the aircraft and the left side of the screen
		float rightDistance = Mathf.Abs(transform.position.x - rightEdge.x);	// The distance between the aircraft and the right side of the screen
		
		if (leftDistance <= turnDistance)
			left = true;
		if (rightDistance <= turnDistance)
			left = false;

		// Set variables to show that the aircraft is turning and in which direction
		if (left)
		{
			turningLeft = true;
			xTarget = transform.position.x + turnDistance;
		}
		else
		{
			turningRight = true;
			xTarget = transform.position.x - turnDistance;
		}

		turnTimer = 0;
	}

	// Called continuously while the aircraft is turning
	void Turning()
	{
		// Move horizontally
		transform.position = new Vector3(Mathf.Lerp(transform.position.x, xTarget, tiltSpeed * Time.deltaTime), transform.position.y, transform.position.z);

		// Stop turning if the aircraft has reached the target area
		if (xTarget - transform.position.x < 0.1f && xTarget - transform.position.x > -0.1f)
		{
			StopTurning();
		}

		// Stop the tilting if the aircraft is half-way through the horizontal movement
		if (xTarget - transform.position.x < turnDistance / 2 && xTarget - transform.position.x > -turnDistance / 2)
		{
			tilt = false;
		}
	}

	void StopTurning()
	{
		// Get a new wait time for the next turn
		nextTurn = Random.Range(minTurnTime, maxTurnTime);

		// Set turningLeft and turningRight to false
		turningLeft = false;
		turningRight = false;

		// Make sure the aircraft can no longer be tilting
		tilt = false;
	}

	void OnCollisionEnter(Collision col)
	{
		// Cause crash damage
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy")
		{
			col.gameObject.GetComponent<Health>().ChangeHealth(-200);
			GetComponent<Health>().ChangeHealth(-200);
		}
	}

}
