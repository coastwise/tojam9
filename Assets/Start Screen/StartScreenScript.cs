using UnityEngine;
using System.Collections;

public class StartScreenScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public GUISkin splashScreenSkin;

	void OnGUI () {

		GUI.skin = splashScreenSkin;
		GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

		Event e = Event.current;

		if (e.type == EventType.MouseDown)
		{
			Application.LoadLevel(1);
		}
	}

}
