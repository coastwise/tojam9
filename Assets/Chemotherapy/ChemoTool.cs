using UnityEngine;
using System.Collections;

public class ChemoTool : Treatment {

	public float duration;

	bool iskillingstuff = false;

	void Update() {
		if (Input.GetMouseButtonDown (0) && !iskillingstuff) {
				//Use ();
				Debug.Log ("Something!!!");


				TreatmentGUI.AddCooldown (type, cooldown + duration);

				iskillingstuff = true;

				GetComponent<SpriteRenderer>().enabled = true;

				StartCoroutine (ApplyChemo ());

				
		}


	}

	
	IEnumerator ApplyChemo() {
		Debug.Log ("start chemo");
		CellScript.chemoHealthyDeath += 2.0f;
		CellScript.chemoCancerDeath += 16.0f;

		yield return new WaitForSeconds (duration);
		Debug.Log ("end chemo");
		CellScript.chemoHealthyDeath -= 2.0f;
		CellScript.chemoCancerDeath -= 16.0f;
		
		GetComponent<SpriteRenderer> ().enabled = false;
		iskillingstuff = false;
		gameObject.SetActive (false);
	}


}
