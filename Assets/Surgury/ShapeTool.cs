using UnityEngine;
using System.Collections;

using Gamelogic.Grids;

public class ShapeTool : Treatment {

	public Material hilight;

	PolygonCollider2D polygon;

	void Start () {
		polygon = GetComponent<PolygonCollider2D>();

		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(polygon.points);
		int[] indices = tr.Triangulate();

		// Create the Vector3 vertices
		Vector3[] vertices = new Vector3[polygon.points.Length];
		for (int i=0; i<polygon.points.Length; i++) {
			vertices[i] = new Vector3(polygon.points[i].x, polygon.points[i].y, 0);
		}
		
		// Create the mesh
		Mesh msh = new Mesh();
		msh.vertices = vertices;
		msh.triangles = indices;
		msh.RecalculateNormals();
		msh.RecalculateBounds();
		
		// Set up game object with mesh;
		gameObject.AddComponent(typeof(MeshRenderer));
		MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = msh;
		renderer.material = hilight;
	}

	// Update is called once per frame
	void Update () {
		Vector3 worldPosition = ExampleUtils.ScreenToWorld(area.root, Input.mousePosition);
		worldPosition = new Vector3(worldPosition.x, worldPosition.y, -1);
		this.transform.localPosition = worldPosition;

		if (!Input.GetMouseButtonDown(0)) return; // only destroy if we click

		if (!area.grid.Contains(area.map[worldPosition])) return;

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
				if (area.grid.Contains(point) && area.grid[point] != null) {
					Destroy(area.grid[point].gameObject);
				}
			}
		}

		Use (); // adds to cooldown
	}
}
