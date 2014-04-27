using UnityEngine;
using System.Collections;

using Gamelogic.Grids;

[RequireComponent (typeof(LineRenderer))]
public class LaserTool : Treatment {

	public bool startPositionSet;

	public Vector3 startPoint;
	public Vector3 endPoint;

	public float length = 10;

	public GameObject effect;

	protected LineRenderer lineRenderer;
	
	void OnEnable () {
		startPositionSet = false;
		lineRenderer = gameObject.GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
	}

	void Update () {
		if (!startPositionSet) {
			// user hasn't set the start point yet
			if (Input.GetMouseButtonDown(0)) {
				startPoint = ExampleUtils.ScreenToWorld(area.root, Input.mousePosition);
				startPoint = new Vector3(startPoint.x, startPoint.y, -1);
				lineRenderer.SetPosition(0, startPoint);
				lineRenderer.enabled = true;
				startPositionSet = true;
			}
		} else {
			Vector3 mouse = ExampleUtils.ScreenToWorld(area.root, Input.mousePosition);

			// fixed length lazor beam
			Vector3 delta = mouse - startPoint;
			delta = new Vector3(delta.x, delta.y, 0);
			delta.Normalize();
			delta = delta * length;
			endPoint = startPoint + delta;

			endPoint = new Vector3(endPoint.x, endPoint.y, -1);

			lineRenderer.SetPosition(1, endPoint);

			if (Input.GetMouseButtonDown(0)) {
				lineRenderer.enabled = false;

				TreatmentGUI.AddCooldown(type, cooldown);

				effect.SetActive(true);
				effect.transform.position = startPoint;
				iTween.MoveTo(effect, iTween.Hash("position", endPoint,
				                                  "time", 2f,
				                                  "easetype", iTween.EaseType.linear,
				                                  "oncompletetarget", this.gameObject,
				                                  "oncomplete", "ZapComplete"));
			}
		}

		if (effect.activeSelf) {
			FlatHexPoint testPos = area.map[effect.transform.position];
			if (area.grid[testPos] != null) {
				Destroy(area.grid[testPos].gameObject);
			}
		}
	}

	public void ZapComplete () {
		effect.SetActive(false);
		gameObject.SetActive(false);
	}
}
