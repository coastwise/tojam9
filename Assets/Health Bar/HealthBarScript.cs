using UnityEngine;
using System.Collections;

public class HealthBarScript : MonoBehaviour {

	public PlayArea area;

	private int minHealthy = 1000;
	private int dangerZone = 1000;
	private int minCancerToStart = 60;
	
	private float lastBlink = 0;
	private bool blinking = false;
	
	public Texture2D healthyPic;
	public Texture2D cancerPic;
	
	public GUISkin healthyBarSkin;
	public GUISkin cancerBarSkin;
	public GUISkin emptyBarSkin;
	public GUISkin cellIconSkin;
	
	private Color cancerColor;
	private Color healthyColor;
	private Color blinkColor;

	
	private TreatmentGUI gui;
	
	public AudioClip healthyMusic;
	public AudioClip cancerMusic;
	
	private bool healthyMusicPlaying = true;
	
	private bool started = false;
	private bool justStarted = false;
	private float detectedMessageTime = 0;

	// Use this for initialization
	void Start () {
		area = FindObjectOfType<PlayArea>();

		StartCoroutine (WaitThenEnableTreatments ());


		
		cancerColor = new Color((255/(float)255), (255/(float)255), (255/(float)255), (float)0.8);
		healthyColor = new Color ((249 / (float)255), (172 / (float)255), (138 / (float)255), (float)0.8);
		blinkColor = new Color ((255 / (float)255), (0 / (float)255), (0 / (float)255), (float)0.4);
	}

	IEnumerator WaitThenEnableTreatments() {
		gui = GameObject.FindObjectOfType<TreatmentGUI> ();
		gui.gameObject.SetActive (false);
		Time.timeScale = 10;
		while (CellScript.cancerCount < minCancerToStart)
			yield return null;
		Time.timeScale = 1;
		gui.gameObject.SetActive (true);
		started = true;
		justStarted = true;
		detectedMessageTime = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {

		var barWidth = Screen.width / 10;
		var barPad = Screen.width / 100;
		var barHeight = Screen.height - (barPad * 2) - 175;	// hardcoded gap for the gamelogic icon

		GUI.Box(new Rect(Screen.width - barWidth - barPad, Screen.height - barHeight - barPad, barWidth, barHeight), "");

		var totalHeight = CellScript.cancerCount + area.emptyCount + CellScript.healthyCount - minHealthy;

		var cancerHeight = (CellScript.cancerCount / (float)totalHeight) * (barHeight - barPad*2);
		var emptyHeight = (area.emptyCount / (float)totalHeight) * (barHeight - barPad*2);
		var healthyHeight = ((CellScript.healthyCount - minHealthy) / (float)totalHeight) * (barHeight - barPad*2);

		Color defaultColor = GUI.color;
		GUISkin defaultSkin = GUI.skin;

		if (healthyHeight < 0) {
			healthyHeight = 0;
		}


		if (!started) {
			GUI.Box (new Rect((float)(Screen.width / 2) - 100, (float)(Screen.height / 2) - 20, 200, 40), "10x Time Lapse\nWaiting...");
		}



		if (justStarted)
		{
			GUI.Box (new Rect((float)(Screen.width / 2) - 100, (float)(Screen.height / 2) - 20, 200, 40), "Cancer Detected!\nBegin Treatment!");
		}

		if (detectedMessageTime + 5 < Time.realtimeSinceStartup) {
			justStarted = false;
		}



		GUI.skin = emptyBarSkin;
		GUI.Box (new Rect (Screen.width - barWidth, (float)(Screen.height - barHeight + (barPad * 0)), barWidth - barPad * 2, emptyHeight), "");

		GUI.skin = cancerBarSkin;
		GUI.color = cancerColor;
		GUI.Box (new Rect (Screen.width - barWidth, (float)(Screen.height - barHeight + (barPad * 0)) + (emptyHeight / 2), (float)(barWidth - barPad * 2), cancerHeight), "");

		GUI.skin = healthyBarSkin;
		GUI.color = healthyColor;
		GUI.Box (new Rect (Screen.width - barWidth, (float)(Screen.height - barHeight + (barPad * 0)) + cancerHeight + (emptyHeight / 2), barWidth - barPad * 2, healthyHeight), "");

		if ((CellScript.healthyCount - minHealthy) < dangerZone) {
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

		if (CellScript.cancerCount > 0) {
			GUI.Box (new Rect (Screen.width - ((barWidth) / 2) - barPad * 3, (float)(Screen.height - barHeight - (barPad * 0.5) + (emptyHeight / 2)), barPad * 4, barPad * 4), cancerPic); 
		}
		if (CellScript.healthyCount - minHealthy > 0) {
			GUI.Box (new Rect (Screen.width - ((barWidth) / 2) - barPad * 3, (float)(Screen.height - barPad * 5.5) - (emptyHeight / 2), barPad * 4, barPad * 4), healthyPic);
		}



		// check for chemo'd or radiation'd
		// make bar green or radioative logo


		Event e = Event.current;



		if (CellScript.healthyCount - minHealthy < 1) {
			// game over

			gui.gameObject.SetActive (false);

			GUI.skin = defaultSkin;
			GUI.Box (new Rect((float)(Screen.width / 2) - 100, (float)(Screen.height / 2) - 20, 200, 40), "The patient has died.\nClick to try again.");

			if (e.type == EventType.MouseDown)
			{
				Application.LoadLevel(1);
			}
		}


	}
}
