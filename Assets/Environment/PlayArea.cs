using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Gamelogic.Grids;

public class PlayArea : GLMonoBehaviour {

	private readonly Vector2 CellDimensions = new Vector2(1,1);
	
	public GameObject cellPrefab;
	public GameObject root;
	
	private FlatHexGrid<CellScript> grid;
	private IMap3D<FlatHexPoint> map;

	public Vector2 gridSize;
	
	public void Start () {
		BuildGrid();
	}
	
	public void Update () {
		if(Input.GetMouseButtonDown(0)) {
			Vector3 worldPosition = ExampleUtils.ScreenToWorld(root, Input.mousePosition);
			
			FlatHexPoint hexPoint = map[worldPosition];
			
			if (grid.Contains(hexPoint) && grid[hexPoint] != null) {
				MoveAndBump(grid[hexPoint], hexPoint+FlatHexPoint.North, FlatHexPoint.North);
				grid[hexPoint] = null;
			}

			else { // if cell is empty, create a sphere at that point

				CreateCell(hexPoint);

			}
		}
	}

	public void MoveAndBump (CellScript incoming, FlatHexPoint point, FlatHexPoint dir) {
		if (!grid.Contains(point)) {
			Destroy (incoming.gameObject);
			return;
		}
		CellScript bumped = grid[point];
		if (bumped != null) MoveAndBump(bumped, point+dir, dir);
		grid[point] = incoming;
		incoming.transform.localPosition = map[point];
	}
	
	private void BuildGrid () {
		grid = (FlatHexGrid<CellScript>)FlatHexGrid<CellScript>.FatRectangle((int)gridSize.x, (int)gridSize.y);
		
		map = new FlatHexMap(CellDimensions)
			.AnchorCellMiddleCenter()
			.WithWindow(ExampleUtils.ScreenRect)
			.AlignMiddleCenter(grid)
			.To3DXY();
		
		foreach(FlatHexPoint point in grid) {
			CreateCell (point);
		}
	}

	public void CreateCell (FlatHexPoint point) {
		CellScript cell = Instantiate(cellPrefab).GetComponent<CellScript>();
		Vector3 worldPoint = map[point];
		
		cell.transform.parent = root.transform;
		cell.transform.localScale = Vector3.one;
		cell.transform.localPosition = worldPoint;
		cell.grid = grid;
		cell.hexPoint = point;
		cell.area = this;

		cell.renderer.material.color = ExampleUtils.colors[point.GetColor3_7()];
		cell.name = "(" + point.X + ", " + point.Y + ")";
		
		grid[point] = cell;
	}

	public List<FlatHexPoint> GetNeighbors(FlatHexPoint point) {
		List<FlatHexPoint> neighbors = new List<FlatHexPoint> ();

		neighbors.Add(new FlatHexPoint(point.X,point.Y + 1));
		neighbors.Add(new FlatHexPoint(point.X,point.Y - 1));
		neighbors.Add(new FlatHexPoint(point.X + 1,point.Y + 1));
		neighbors.Add(new FlatHexPoint(point.X - 1,point.Y + 1));
		neighbors.Add(new FlatHexPoint(point.X + 1,point.Y));
		neighbors.Add(new FlatHexPoint(point.X - 1,point.Y));

		return neighbors;
	}
}
