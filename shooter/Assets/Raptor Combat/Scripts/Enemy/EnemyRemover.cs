using UnityEngine;
using System.Collections;

public class EnemyRemover : MonoBehaviour
{


	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	void OnTriggerEnter(Collider col)
	{
		
		// Delte enemies that have left the player's view
		if (col.gameObject.GetComponent<EnemyNavigation>() != null)
		{
			Destroy(col.gameObject);
		}
		else if (col.transform.parent != null)
		{
			if (col.transform.parent.gameObject.GetComponent<EnemyNavigation>() != null)
			{
				Destroy(col.transform.parent.gameObject);
			}
		}
	}

}
