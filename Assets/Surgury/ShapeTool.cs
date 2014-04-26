using UnityEngine;
using System.Collections;

using Gamelogic.Grids;

public class ShapeTool : MonoBehaviour {

	public PlayArea area;

	public Material hilight;

	PolygonCollider2D polygon;

	void Start () {
		polygon = GetComponent<PolygonCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 worldPosition = ExampleUtils.ScreenToWorld(area.root, Input.mousePosition);
		this.transform.localPosition = worldPosition;

		if (area.grid.Contains(area.map[worldPosition])) {
			Debug.LogWarning(worldPosition);
		} else return;

		Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
		Vector2 max = new Vector2(float.MinValue, float.MinValue);
		foreach(Vector2 point in polygon.points) {
			min.x = Mathf.Min(min.x, point.x);
			min.y = Mathf.Min(min.y, point.y);
			max.x = Mathf.Max(max.x, point.x);
			max.y = Mathf.Max(max.y, point.y);
		}

		for (float i = min.x; i < max.x; i += area.CellDimensions.x / 2f) {
			for (float j = min.y; j < max.y; j += area.CellDimensions.y/ 2f) {
				Vector3 test = transform.position + new Vector3(i, j, 0);

				if (!polygon.OverlapPoint(test)) {
					continue;
				}

				FlatHexPoint point = area.map[test];
				Debug.Log(point);
				if (area.grid.Contains(point) && area.grid[point] != null) {
					// TODO: hilight instead of destroy (until they click)
					//area.grid[point].renderer.materials[1] = hilight;
					Destroy(area.grid[point].gameObject);
				}
			}
		}
	}
}
