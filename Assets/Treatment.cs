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

	private PlayArea _area;
	protected PlayArea area {
		get {
			if (_area == null) {
				_area = GameObject.FindObjectOfType<PlayArea>();
			}
			return _area;
		}
	}

	virtual public void Use () {
		TreatmentGUI.AddCooldown(type, cooldown);
		gameObject.SetActive(false);
	}
	
}
