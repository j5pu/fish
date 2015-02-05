/*
 * PlayerNavigation.cs
 * Author: MutantGopher
 * 9/8/2014
 * 
 * This script handles the player's movement control
 */

using UnityEngine;
using System.Collections;

public class PlayerNavigation : MonoBehaviour
{
	public float speed = 10.0f;						// The speed at which the aircraft moves
	public float tiltSpeed = 0.6f;					// The speed at which the aircraft will tilt (right/left).  This does not affect movement speed.

	public GameObject cameraGO;						// Reference to the camera GameObject in the scene
	public float distanceFromCamera = 40.0f;		// The constant distance the player aircraft should be away from the camera at all times

	private Vector3 startingRotation;				// The starting rotation of the aircraft
	


	// Use this for initialization
	void Start()
	{
		// Get the proper rotations for this plane GameObject to be used for tilting
		startingRotation = transform.eulerAngles;

	}
	
	// Update is called once per frame
	void Update()
	{
		// Handle player aircraft navigation and control
		Navigate();

	}

	void Navigate()
	{
		// Get input from the user for aircraft navigation
		float vertical = 0;//Input.GetAxis("Vertical");
		float horizontal = Input.GetAxis("Horizontal");

		if (Input.touchCount > 0)
		{
			horizontal = Input.touches[0].deltaPosition.x;
//			pointer_y = Input.touches[0].deltaPosition.y;
		}
		
		// Move the aircraft
		transform.position += new Vector3(horizontal * speed * Time.deltaTime, 0, vertical * speed * Time.deltaTime);
		

		// Make sure the player doesn't move out of the camera view
		// - newPosition holds screen coordinates of the player - x is horizontal and y is vertical
		Vector3 screenPos = cameraGO.camera.WorldToScreenPoint(transform.position);
		
		Vector3 newPosition = new Vector3(screenPos.x, transform.position.y, screenPos.y);
		if (screenPos.x < 0.0f)
			newPosition = new Vector3(0, newPosition.y, newPosition.z);
		if (screenPos.x > Screen.width)
			newPosition = new Vector3(Screen.width, newPosition.y, newPosition.z);
		if (screenPos.y < 0.0f)
		{
			
			newPosition = new Vector3(newPosition.x, newPosition.y, 0.0f);
		}
		if (screenPos.y > Screen.height)
			newPosition = new Vector3(newPosition.x, newPosition.y, Screen.height);
		
		Vector3 worldPos = cameraGO.camera.ScreenToWorldPoint(new Vector3(newPosition.x, newPosition.z, distanceFromCamera));
		
		transform.position = worldPos;
		

		// Tilt the aircraft based on horizontal input
		float angle = 0.0f;
		if (horizontal > 0.5f)
			angle = -80.0f;
		if (horizontal < -0.5f)
			angle = 80.0f;
		transform.eulerAngles = new Vector3(startingRotation.x, startingRotation.y, Mathf.LerpAngle(transform.eulerAngles.z, angle, tiltSpeed * Time.deltaTime));
		
		
	}

}
