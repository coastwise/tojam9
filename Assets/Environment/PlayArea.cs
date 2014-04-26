using UnityEngine;
using System.Collections;

using Gamelogic.Grids;

public class PlayArea : GLMonoBehaviour {

	private readonly Vector2 CellDimensions = new Vector2(1,1);
	
	public GameObject cellPrefab;
	public GameObject root;
	
	private FlatHexGrid<GameObject> grid;
	private IMap3D<FlatHexPoint> map;

	public Vector2 gridSize;
	
	public void Start () {
		BuildGrid();
	}
	
	public void Update () {
		if(Input.GetMouseButtonDown(0)) {
			Vector3 worldPosition = ExampleUtils.ScreenToWorld(root, Input.mousePosition);
			
			FlatHexPoint hexPoint = map[worldPosition];
			
			if(grid.Contains(hexPoint)) {
				foreach (FlatHexPoint n in grid.GetNeighbors(hexPoint)) {
					Destroy(grid[n]);
				}
			}
		}
	}
	
	private void BuildGrid () {
		grid = FlatHexGrid<GameObject>.FatRectangle((int)gridSize.x, (int)gridSize.y);
		
		map = new FlatHexMap(CellDimensions)
			.AnchorCellMiddleCenter()
			.WithWindow(ExampleUtils.ScreenRect)
			.AlignMiddleCenter(grid)
			.To3DXY();
		
		foreach(FlatHexPoint point in grid) {
			GameObject cell = Instantiate(cellPrefab);
			Vector3 worldPoint = map[point];
			
			cell.transform.parent = root.transform;
			cell.transform.localScale = Vector3.one;
			cell.transform.localPosition = worldPoint;
			
			cell.renderer.material.color = ExampleUtils.colors[point.GetColor3_7()];
			cell.name = "(" + point.X + ", " + point.Y + ")";
			
			grid[point] = cell;
		}
	}
}
