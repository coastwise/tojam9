using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TreatmentType {
	Surgery,
	Chemo,
	Radiation,
	Targeted,
	Genetics,
	FutureTech
}

public class Treatment : MonoBehaviour {



	public float cooldown;

	public TreatmentType type;

	virtual public void Use () {
		TreatmentGUI.Cooldown[type] += cooldown;
		gameObject.SetActive(false);
	}
	
}
