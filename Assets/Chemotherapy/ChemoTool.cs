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

						StartCoroutine (Chemo ());
				}

		if (iskillingstuff) {

				}

	}

	IEnumerator Chemo() {
		yield return new WaitForSeconds(duration);
		
		GetComponent<SpriteRenderer> ().enabled = false;
		iskillingstuff = false;
		gameObject.SetActive (false);
	}

}
