using UnityEngine;
using System.Collections;

using Gamelogic.Grids;

[RequireComponent (typeof(LineRenderer))]
public class LaserTool : Treatment {

	public bool startPositionSet;

	public Vector3 startPoint;
	public Vector3 endPoint;

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
			endPoint = ExampleUtils.ScreenToWorld(area.root, Input.mousePosition);
			endPoint = new Vector3(endPoint.x, endPoint.y, -1);

			lineRenderer.SetPosition(1, endPoint);

			if (Input.GetMouseButtonDown(0)) {
				StartCoroutine(Zap());
			}
		}
	}

	IEnumerator Zap () {
		lineRenderer.enabled = false;
		TreatmentGUI.AddCooldown(type, cooldown);

		float dist = Vector3.Distance(startPoint, endPoint);
		for (int i = 0; i < dist; i++) {
			float t = (float) i / dist;
			Vector3 test = Vector3.Lerp(startPoint, endPoint, t);
			FlatHexPoint testPos = area.map[test];
			if (area.grid[testPos] != null) {
				Destroy(area.grid[testPos].gameObject);
			}
			yield return new WaitForSeconds(0.1f);
		}

		gameObject.SetActive(false);
	}
}
