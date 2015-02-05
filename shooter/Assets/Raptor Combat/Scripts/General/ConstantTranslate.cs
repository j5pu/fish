using UnityEngine;
using System.Collections;

public class ConstantTranslate : MonoBehaviour
{
	public Vector3 movement = Vector3.zero;
	public Vector3 rotate = Vector3.zero;


	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		// Move the gameobject
		transform.Translate(movement.x * Time.deltaTime, movement.y * Time.deltaTime, movement.z * Time.deltaTime);

		// Rotate the gameobject
		transform.Rotate(rotate.x * Time.deltaTime, rotate.y * Time.deltaTime, rotate.z * Time.deltaTime);
	}
}
