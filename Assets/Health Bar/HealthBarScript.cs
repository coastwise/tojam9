using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {

	public PlayArea area;

	// Use this for initialization
	void Start () {
		area = FindObjectOfType<PlayArea>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	private int minHealthy = 500;
	private int dangerZone = 500;
	private int minCancerToStart = 20;

	private float lastBlink = 0;
	private bool blinking = false;

	public Texture2D healthyPic;
	public Texture2D cancerPic;

	public GUISkin healthyBarSkin;
	public GUISkin cancerBarSkin;
	public GUISkin emptyBarSkin;
	public GUISkin cellIconSkin;

	private Color cancerColor = new Color((255/(float)255), (255/(float)255), (255/(float)255), (float)0.8);
	private Color healthyColor = new Color ((249 / (float)255), (172 / (float)255), (138 / (float)255), (float)0.8);
	private Color blinkColor = new Color ((255 / (float)255), (0 / (float)255), (0 / (float)255), (float)0.4);


	public AudioClip healthyMusic;
	public AudioClip cancerMusic;

	private bool healthyMusicPlaying = true;

	void OnGUI () {



		var barWidth = Screen.width / 10;
		var barPad = Screen.width / 100;
		var barHeight = Screen.height - (barPad * 2) - 175;	// hardcoded gap for the gamelogic icon

		GUI.Box(new Rect(Screen.width - barWidth - barPad, Screen.height - barHeight - barPad, barWidth, barHeight), "");

		var totalHeight = area.cancerCount + area.emptyCount + area.healthyCount - minHealthy;

		var cancerHeight = (area.cancerCount / (float)totalHeight) * (barHeight - barPad*2);
		var emptyHeight = (area.emptyCount / (float)totalHeight) * (barHeight - barPad*2);
		var healthyHeight = ((area.healthyCount - minHealthy) / (float)totalHeight) * (barHeight - barPad*2);

		Color defaultColor = GUI.color;

		if (healthyHeight < 0) {
			healthyHeight = 0;
		}

		GUI.skin = emptyBarSkin;
		GUI.Box (new Rect (Screen.width - barWidth, (float)(Screen.height - barHeight + (barPad * 0)), barWidth - barPad * 2, emptyHeight), "");

		GUI.skin = cancerBarSkin;
		GUI.color = cancerColor;
		GUI.Box (new Rect (Screen.width - barWidth, (float)(Screen.height - barHeight + (barPad * 0)) + (emptyHeight / 2), (float)(barWidth - barPad * 2), cancerHeight), "");

		GUI.skin = healthyBarSkin;
		GUI.color = healthyColor;
		GUI.Box (new Rect (Screen.width - barWidth, (float)(Screen.height - barHeight + (barPad * 0)) + cancerHeight + (emptyHeight / 2), barWidth - barPad * 2, healthyHeight), "");

		if ((area.healthyCount - minHealthy) < dangerZone) {
			// flash bar
			if (healthyMusicPlaying) {
				audio.clip = cancerMusic;
				audio.Play ();
				healthyMusicPlaying = false;
			}

			if (Time.realtimeSinceStartup > lastBlink + 0.2) {

				lastBlink = Time.realtimeSinceStartup;
				if (blinking == true) {
					blinking = false;
				} else {
					blinking = true;
				}
			}

		} else {
			if (!healthyMusicPlaying)
			{
				audio.clip = healthyMusic;
				audio.Play ();
				healthyMusicPlaying = true;
			}
		}

		if (blinking == true)
		{
			GUI.color = blinkColor;
			GUI.Box (new Rect (Screen.width - barWidth, (float)(Screen.height - barHeight + (barPad * 0)) + (emptyHeight/2), barWidth - barPad * 2, cancerHeight + healthyHeight), "");
		}




		GUI.color = defaultColor;
		GUI.skin = cellIconSkin;

		if (area.cancerCount > 0) {
			GUI.Box (new Rect (Screen.width - ((barWidth) / 2) - barPad * 3, (float)(Screen.height - barHeight - (barPad * 0.5) + (emptyHeight / 2)), barPad * 4, barPad * 4), cancerPic); 
		}
		if (area.healthyCount - minHealthy > 0) {
			GUI.Box (new Rect (Screen.width - ((barWidth) / 2) - barPad * 3, (float)(Screen.height - barPad * 5.5) - (emptyHeight / 2), barPad * 4, barPad * 4), healthyPic);
		}



		// check for chemo'd or radiation'd
		// make bar green or radioative logo

		if (area.healthyCount - minHealthy < 1) {
			// game over
			print("Game Over!");

		}


	}
}
