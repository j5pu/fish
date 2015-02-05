using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour
{
	public bool healthBonus = true;					// Whether or not this bonus gives the player extra health
	public float healthAmount = 10.0f;				// How much extra health to give the player
	public bool weaponBonus = false;				// Whether or not this bonus gives the player extra missiles
	public int weaponAmount = 1;					// How many extra missiles to give the player
	public bool shieldBonus = false;				// Whether or not this bonus gives the player a wrecking shield


	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.collider.transform.parent)		// First make sure there's a parent
		{
			if (col.collider.transform.parent.gameObject.GetComponent<PlayerNavigation>() != null)		// If the object collided with is the player
			{
				if (healthBonus)
				{
					col.collider.transform.parent.gameObject.GetComponent<Health>().ChangeHealth(healthAmount);
				}
				if (weaponBonus)
				{
					col.collider.transform.parent.FindChild("missile_launcher").gameObject.GetComponent<MissileLauncher>().ChangeMissilesRemaining(1);
				}
				if (shieldBonus)
				{
					col.collider.transform.parent.gameObject.GetComponent<Health>().ActivateShield();
				}

				// Destroy the bonus
				Destroy(gameObject);
			}
		}
	}
}
