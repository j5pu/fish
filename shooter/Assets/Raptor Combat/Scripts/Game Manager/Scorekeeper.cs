using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scorekeeper : MonoBehaviour
{
	public int startingScore = 0;					// The score with which the player starts (usually 0)
	private int currentScore;						// The player's current score
	private int finalScore = 0;						// The player's final score

	public Text scoreText;							// The text that displays the player's current score in-game
	public Text finalScoreText;						// The text that shows the final score on the game over UI

	public bool gameOver = false;					// Keeps track of whether or not the game is over yet - not really meant to be set from the inspector

	private float streakSpeed = 1.0f;				// Kills less than this amount of time apart will be counted for bonuses
	private float scoreTimer = 0.0f;				// Timer used to keep track of kill streaks for bonuses
	private int currentStreak = 0;					// The current streak of kills for the player - resets to 0 if there haven't been kills in the last [streakSpeed]
	public Text achievementUIText;					// UI element used to show what achievement the player has attained
	public float achievementUIFadeSpeed = 1.0f;		// The speed at which the achievement UI will fade out

	// Score achievements - these can be submitted to a web portal or similar service
	private bool wreckingMaster = false;			// Wrecking Master achievement - when the player scores 1,000 points
	private bool eliteGun = false;					// Elite Gun achievement - when the player scores 3,000 points
	private bool legend = false;					// Legend achievement - when the player scores 10,000 points
	private bool ghostPilot = false;				// Ghost Pilot achievement - when the player scores 25,000 points

	private GameObject kongregateAPI;				// The GameObject with the API component for high scores and/or achievements


	// Use this for initialization
	void Start()
	{
		// Initialize the current score variable
		currentScore = startingScore;

		kongregateAPI = GameObject.Find("Kongregate");
		if (kongregateAPI != null)
			Debug.Log("Found the kong GO");
	}
	
	// Update is called once per frame
	void Update()
	{
		// Handle kill streak bonuses (and achievments)
		scoreTimer += Time.deltaTime;

		// Update the score GUI text
		scoreText.text = "" + currentScore;

		if (scoreTimer > streakSpeed)
		{
			// Triple strike
			if (currentStreak == 3)
			{
				Bonus("TRIPLE STRIKE", 20);
			}
			// Rampage
			else if (currentStreak == 4)
			{
				Bonus("RAMPAGE", 50);
			}
			// Awesomeness
			else if (currentStreak == 5)
			{
				Bonus("AWESOMENESS", 100);
			}
			// Wicked Strike
			else if (currentStreak >= 6 && currentStreak < 10)
			{
				Bonus("WICKED STRIKE", 200);
			}
			// Mad Skills
			else if (currentStreak >= 10)
			{
				Bonus("MAD SKILLS", 500);
			}

			currentStreak = 0;
		}

		// For Kongregate achievements - scores
		if (currentScore >= 1000 && !wreckingMaster)
		{
			wreckingMaster = true;
			// Display the achievement in the HUD
			Bonus("WRECKING MASTER", 0);
		}
		if (currentScore >= 3000 && !eliteGun)
		{
			eliteGun = true;
			// Display the achievement in the HUD
			Bonus("ELITE GUN", 0);
		}
		if (currentScore >= 10000 && !legend)
		{
			legend = true;
			// Display the achievement in the HUD
			Bonus("LEGEND", 0);
		}
		if (currentScore >= 25000 && !ghostPilot)
		{
			ghostPilot = true;
			// Display the achievement in the HUD
			Bonus("GHOST PILOT!", 0);
		}
		
		// Make the achievement UI fade out
		achievementUIText.color = new Color(achievementUIText.color.r, achievementUIText.color.g, achievementUIText.color.b, achievementUIText.color.a - achievementUIFadeSpeed * Time.deltaTime);
	}

	void Bonus(string bonusType, int bonusScore)
	{
		// Give the player bonus score for the achievement
		currentScore += bonusScore;

		// Make a UI element that shows what the player has achieved
		if (achievementUIText != null)
		{
			achievementUIText.text = bonusType;
			achievementUIText.color = new Color(achievementUIText.color.r, achievementUIText.color.g, achievementUIText.color.b, 1.0f);
		}
	}

	public void ChangeScore(int amount)		// Used to change the current score (from other scripts)
	{
		// Add the specified amount to the score (negative parameters can be used to lower the score)
		currentScore += amount;
		
		// Make sure the score doesn't go below zero
		if (currentScore < 0)
		{
			currentScore = 0;
		}

		// Add a kill to the current streak and start the streak timer again
		currentStreak++;
		scoreTimer = 0.0f;
	}

	public void GameOver()
	{
		finalScore = currentScore;
		if (finalScoreText != null)
			finalScoreText.text = "" + finalScore;

		// Set gameOver to true
		gameOver = true;

		// Achievements...

	}
}
