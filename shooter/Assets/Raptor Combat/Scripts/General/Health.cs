using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
	public bool player = false;						// Whether this is the player or an enemy
	public float startingHealth = 100.0f;			// The amount of health to start with
	public float maxHealth = 100.0f;				// The maximum amount of health 
	public int killPoints = 10;						// The number of points the player will get for killing this health, if it's an enemy
	private float currentHealth;					// The current amount of health

	public GameObject deathEffects;					// The death effects (explosions) to be instantiated on death
	public GameObject deathUI;						// The UI to be activated upon death
	public GameObject inGameHUD;					// The UI to be deactivated upon death
	public Text healthUI;							// The UI text element that shows how much health is remaining

	public GameObject healthBonus;					// The health bonus to be instantiated on death if this is an enemy
	public GameObject weaponBonus;					// The weapon bonus to be instantiated on death if this is an enemy
	public GameObject shieldBonus;					// The shield bonus to be instantiated on death if this is an enemy

	public float shieldTime = 10.0f;				// The amount of time the user gets to keep the shield
	private float shieldTimer = 0.0f;				// The timer used to keep track of how long the player has had the shield
	public GameObject shield;						// The physical shield gameobject to activate/deactivate
	private bool lockHealth = false;				// Used to lock the health so it can't be changed when the shield is activated
	private bool flashing = false;					// Whether or not the shield should currently be flashing
	private float shieldFlashTimer = 0.0f;			// The timer to keep track of flashing
	public float shieldFlashTime = 0.2f;			// The time for the shield to flash when it's about to dissapear
	private bool shieldIsCurrentlyActive = false;	// Whether or not the shield gameobject is currently active


	// Use this for initialization
	void Start()
	{
		currentHealth = startingHealth;

		// Update the UI element that represents the health remaining
		if (player && healthUI != null)
		{
			healthUI.text = "" + currentHealth;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		// Die when the player runs out of health
		if (currentHealth <= 0)
			Die();

		// Shield mechanics for the player
		if (player)
		{
			// Update the shield timer
			shieldTimer += Time.deltaTime;

			// Deactivate the shield when the timer runs out
			if (shieldTimer >= shieldTime)
			{
				DeactivateShield();
			}
			// Make the shield flash when the timer is about to run out
			else if (shieldTimer > shieldTime - 3.0f && lockHealth)
			{
				flashing = true;
			}
			if (flashing)
			{
				shieldFlashTimer += Time.deltaTime;

				if (shieldFlashTimer > shieldFlashTime)
				{
					shieldFlashTimer = 0.0f;
					shieldIsCurrentlyActive = !shieldIsCurrentlyActive;
					shield.SetActive(shieldIsCurrentlyActive);
				}
			}
		}
	}

	public void ChangeHealth(float amount)
	{
		// Don't allow the health to be reduced while it's locked
		if (amount < 0 && lockHealth)
			return;

		currentHealth += amount;

		// Don't let the heatlh get below 0
		if (currentHealth < 0)
			currentHealth = 0;

		// Update the UI element that represents the health remaining
		if (player && healthUI != null)
		{
			healthUI.text = "" + currentHealth;
		}
	}

	public void ActivateShield()
	{
		// Reset the shield timer to 0
		shieldTimer = 0;

		// Activate the physical shield
		shield.SetActive(true);

		// Lock the player's health
		lockHealth = true;

		// Make sure the shield is not flashing
		flashing = false;
	}

	void DeactivateShield()
	{
		// Make sure the shield is not flashing
		flashing = false;

		// Deactivate the physical shield
		shield.SetActive(false);

		// Unlock the player's health so it can be changed again
		lockHealth = false;
	}

	public void Die()
	{
		// Instantiate death effects where this thing was killed
		if (deathEffects != null)
		{
			Instantiate(deathEffects, transform.position, Quaternion.identity);
		}

		
		if (player)
		{
			// Activate the death UI and deactivate the in-game HUD
			if (deathUI != null)
				deathUI.SetActive(true);
			if (inGameHUD != null)
				inGameHUD.SetActive(false);

			// The GameOver function on the scorekeeper GameObject deals with the final score for the player
			Scorekeeper sk = (Scorekeeper)Object.FindObjectOfType(typeof(Scorekeeper));
			sk.GameOver();
		}
		else
		{
			// Increase the player's score if this was an enemy
			GameObject.FindGameObjectWithTag("Scorekeeper").GetComponent<Scorekeeper>().ChangeScore(killPoints);

			// Randomly spawn a health or weapon bonus
			int random = Random.Range(0, 25);
			if (random < 4)
			{
				// Instantiate a health bonus
				if (healthBonus != null)
				{
					Instantiate(healthBonus, transform.position, Quaternion.identity);
				}
			}
			else if (random >= 4 && random < 8)
			{
				// Instantiate a weapon bonus
				if (weaponBonus != null)
				{
					Instantiate(weaponBonus, transform.position, Quaternion.identity);
				}
			}
			else if (random == 10)
			{
				// Instantiate a shield bonus
				if (shieldBonus != null)
				{
					Instantiate(shieldBonus, transform.position, Quaternion.identity);
				}
			}
		}

		// Destroy the gameObject
		Destroy(gameObject);
	}

}
