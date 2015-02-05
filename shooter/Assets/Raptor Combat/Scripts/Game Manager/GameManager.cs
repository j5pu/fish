using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameObject pauseMenu;				// The pause menu UI element to be activated on pause
	private bool paused = false;				// The boolean value to keep track of whether or not the game is currently paused

	public float maxSlowMoTime = 2.0f;			// The maximum time the player can spend in slow motion
	private float slowMoTimer = 0.0f;			// Timer used to keep track of the player's slowmo time
	private bool canSlowmMotion = true;			// Whether or not the player can currently use slowmo


	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		// Make sure the player doesn't use slow motion too much
		if (slowMoTimer >= maxSlowMoTime)
			canSlowmMotion = false;
		if (slowMoTimer <= 0)
			canSlowmMotion = true;
		if (slowMoTimer < 0.0f)
			slowMoTimer = 0.0f;

		// Slow motion
		if (Input.GetButton("Slow Motion") && canSlowmMotion && !paused)
		{
			Time.timeScale = 0.2f;
			slowMoTimer += Time.deltaTime * 5.0f;
		}
		else if (!paused)
		{
			Time.timeScale = 1.0f;
			slowMoTimer -= Time.deltaTime;
		}

		// Pause game
		if (Input.GetButtonDown("Pause"))
		{
			paused = !paused;
			if (paused)
				Pause();
			else
				Play();
		}
	}

	public void RestartLevel()
	{
		// Restart this level
		Application.LoadLevel(Application.loadedLevel);
	}

	public void ReturnToMainMenu()
	{
		// Load the main menu scene
		Application.LoadLevel("MainMenu");
	}

	public void Pause()
	{
		// Pause the game
		Time.timeScale = 0.0f;

		// Activate the pause menu UI element
		if (pauseMenu != null)
			pauseMenu.SetActive(true);

		paused = true;
	}

	public void Play()
	{
		// Play, so the game is no longer paused
		Time.timeScale = 1.0f;

		// Deactivate the pause menu UI element
		if (pauseMenu != null)
			pauseMenu.SetActive(false);

		paused = false;
	}
}
