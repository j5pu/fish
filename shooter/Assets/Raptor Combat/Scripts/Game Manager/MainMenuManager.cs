using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
	public Image controlsPanel;					// The UI panel that shows the controls

	public Image loadingPanel;					// The UI panel that shows the loading screen
	public Image fadePanel;						// The dark UI panel used to fade between panels
	public float fadeSpeed = 0.1f;				// The speed at which to fade

	private bool fadeOutToBlack = false;		// Whether or not the UI is fading out to black
	private bool fadeIn = false;				// Whether or not the UI is fading in from black


	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
		if (fadeOutToBlack)
		{
			fadePanel.color = Color.Lerp(fadePanel.color, new Color(0, 0, 0, 1), fadeSpeed * Time.deltaTime);
		}
		if (fadeIn)
		{
			fadePanel.color = Color.Lerp(fadePanel.color, new Color(0, 0, 0, 0), fadeSpeed * Time.deltaTime);
		}

		if (fadeOutToBlack && fadePanel.color.a > 0.99f)
		{
			// Activate the loading screen
			loadingPanel.gameObject.SetActive(true);

			// Begin fading in to the loading screen
			fadeIn = true;
			fadeOutToBlack = false;
		}

		if (fadeIn && fadePanel.color.a < 0.01f)
		{
			Application.LoadLevel("Level1");
		}
	}

	// Start playing the game
	public void Play()
	{
		fadePanel.gameObject.SetActive(true);
		fadeOutToBlack = true;
		controlsPanel.gameObject.SetActive(false);
	}

	// Quit the game
	public void Quit()
	{
		Application.Quit();
	}

	// Show/hide the controls UI element
	public void Controls()
	{
		if (!controlsPanel.gameObject.activeSelf)
		{
			controlsPanel.gameObject.SetActive(true);
		}
		else if (controlsPanel.gameObject.activeSelf)
		{
			controlsPanel.gameObject.SetActive(false);
		}
	}

}
