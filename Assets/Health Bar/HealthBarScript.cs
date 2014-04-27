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


	private int minHealthy = 300;

	public Texture2D healthyPic;
	public Texture2D cancerPic;

	public GUISkin healthyBarSkin;
	public GUISkin cancerBarSkin;
	public GUISkin emptyBarSkin;
	public GUISkin cellIconSkin;

	void OnGUI () {



		var barWidth = Screen.width / 10;
		var barPad = Screen.width / 100;
		var barHeight = Screen.height - (barPad * 2) - 175;	// hardcoded gap for the gamelogic icon

		GUI.Box(new Rect(Screen.width - barWidth - barPad, Screen.height - barHeight - barPad, barWidth, barHeight), "");

		var totalHeight = area.cancerCount + area.emptyCount + area.healthyCount - minHealthy;

		var cancerHeight = (area.cancerCount / (float)totalHeight) * (barHeight - barPad*2);
		var emptyHeight = (area.emptyCount / (float)totalHeight) * (barHeight - barPad*2);
		var healthyHeight = ((area.healthyCount - minHealthy) / (float)totalHeight) * (barHeight - barPad*2);

		if (healthyHeight < 0) {
			healthyHeight = 0;
		}

		GUI.skin = emptyBarSkin;
		GUI.Box (new Rect (Screen.width - barWidth, (float)(Screen.height - barHeight + (barPad * 0)), barWidth - barPad * 2, emptyHeight), "");
		GUI.skin = cancerBarSkin;
		GUI.Box (new Rect (Screen.width - barWidth, (float)(Screen.height - barHeight + (barPad * 0)) + emptyHeight, (float)(barWidth - barPad * 2), cancerHeight), "");
		GUI.skin = healthyBarSkin;
		GUI.Box (new Rect (Screen.width - barWidth, (float)(Screen.height - barHeight + (barPad * 0)) + cancerHeight + emptyHeight, barWidth - barPad * 2, healthyHeight), "");

		GUI.skin = cellIconSkin;
		GUI.Box (new Rect (Screen.width - ((barWidth) / 2) - barPad*2, (float)(Screen.height - barHeight - (barPad * 0.5)), barPad*2, barPad*2), cancerPic); 
		GUI.Box (new Rect (Screen.width - ((barWidth) / 2) - barPad*2, (float)(Screen.height - barPad*3.5), barPad*2, barPad*2), healthyPic);




	}
}
