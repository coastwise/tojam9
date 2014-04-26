﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Gamelogic.Grids;

public class PlayArea : GLMonoBehaviour {

	public readonly Vector2 CellDimensions = new Vector2(1,1);
	
	public CellScript healthyCellPrefab;
	public CellScript cancerCellPrefab;

	public GameObject root;
	
	public FlatHexGrid<CellScript> grid;
	public IMap3D<FlatHexPoint> map;

	public Vector2 gridSize;

	public void Start () {
		BuildGrid();
	}

	public void MoveAndBump (CellScript incoming, FlatHexPoint point, FlatHexPoint dir) {
		if (!grid.Contains(point)) {
			Destroy (incoming.gameObject);
			return;
		}
		CellScript bumped = grid[point];
		if (bumped != null) MoveAndBump(bumped, point+dir, dir);
		grid[point] = incoming;
		incoming.hexPoint = point;
		incoming.animationTarget = map[point];
		incoming.animating = true;
	}
	
	private void BuildGrid () {
		grid = (FlatHexGrid<CellScript>)FlatHexGrid<CellScript>.FatRectangle((int)gridSize.x, (int)gridSize.y);
		
		map = new FlatHexMap(CellDimensions)
			.AnchorCellMiddleCenter()
			.WithWindow(ExampleUtils.ScreenRect)
			.AlignMiddleCenter(grid)
			.To3DXY();
		
		foreach(FlatHexPoint point in grid) {
			if (Random.value < 0.5f) SpawnCell(healthyCellPrefab, point);
		}

		int x = (int)Random.Range(gridSize.x/3, 2*gridSize.x/3);
		int y = (int)Random.Range(gridSize.y/3, 2*gridSize.y/3);
		Debug.Log(x+","+y);
		FlatHexPoint cancerSpawnPoint = new FlatHexPoint(x, y);
		if (grid[cancerSpawnPoint] != null) Destroy (grid[cancerSpawnPoint]);
		SpawnCell(cancerCellPrefab, cancerSpawnPoint);
	}

	public void SpawnCell (CellScript prefab, FlatHexPoint point) {
		SpawnCell (prefab, point, FlatHexPoint.Zero);
	}

	public void SpawnCell (CellScript prefab, FlatHexPoint point, FlatHexPoint bumpDirection) {
		if (!grid.Contains(point)) {
			Debug.LogError("tried to spawn cell outside of grid");
			return;
		}

		CellScript cell = Instantiate(prefab.gameObject).GetComponent<CellScript>();
		Vector3 worldPoint = map[point - bumpDirection];
		
		cell.transform.parent = root.transform;
		cell.transform.localScale = Vector3.one;
		cell.transform.localPosition = worldPoint;
		cell.animationTarget = worldPoint;
		cell.animating = true;

		cell.grid = grid;
		cell.hexPoint = point;
		cell.area = this;

		if (bumpDirection != FlatHexPoint.Zero) {
			MoveAndBump(cell, point, bumpDirection);
		} else {
			if (grid[point] != null) {
				Destroy (grid[point].gameObject);
			}
			grid[point] = cell;
		}
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
