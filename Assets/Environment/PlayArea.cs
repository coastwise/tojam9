using UnityEngine;
using System.Collections;
using System.Linq;

using Gamelogic.Grids;

public class PlayArea : GLMonoBehaviour {

	private readonly Vector2 CellDimensions = new Vector2(1,1);
	
	public GameObject healthyCellPrefab;
	public GameObject cancerCellPrefab;

	public GameObject root;
	
	private FlatHexGrid<GameObject> grid;
	private IMap3D<FlatHexPoint> map;

	public Vector2 gridSize;

	public void Start () {
		BuildGrid();
	}

	public void MoveAndBump (GameObject incoming, FlatHexPoint point, FlatHexPoint dir) {
		if (!grid.Contains(point)) {
			Destroy (incoming);
			return;
		}
		GameObject bumped = grid[point];
		if (bumped != null) MoveAndBump(bumped, point+dir, dir);
		grid[point] = incoming;
		iTween.MoveTo(incoming, iTween.Hash("position", map[point],
		                                    "islocal", true,
		                                    "time", 0.4f));
	}
	
	private void BuildGrid () {
		grid = FlatHexGrid<GameObject>.FatRectangle((int)gridSize.x, (int)gridSize.y);
		
		map = new FlatHexMap(CellDimensions)
			.AnchorCellMiddleCenter()
			.WithWindow(ExampleUtils.ScreenRect)
			.AlignMiddleCenter(grid)
			.To3DXY();
		
		foreach(FlatHexPoint point in grid) {
			SpawnCell(healthyCellPrefab, point);
		}

		int x = (int)Random.Range(gridSize.x/3, 2*gridSize.x/3);
		int y = (int)Random.Range(gridSize.y/3, 2*gridSize.y/3);
		Debug.Log(x+","+y);
		FlatHexPoint cancerSpawnPoint = new FlatHexPoint(x, y);
		if (grid[cancerSpawnPoint] != null) Destroy (grid[cancerSpawnPoint]);
		SpawnCell(cancerCellPrefab, cancerSpawnPoint);
	}
	
	private void SpawnCell (GameObject prefab, FlatHexPoint point) {
		GameObject cell = Instantiate(prefab);
		Vector3 worldPoint = map[point];
		
		cell.transform.parent = root.transform;
		cell.transform.localScale = Vector3.one;
		cell.transform.localPosition = worldPoint;
		
		grid[point] = cell;
	}

}
