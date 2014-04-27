using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Grids;

using System.Linq;

public class CellScript : MonoBehaviour {

	public float _divideDelayInSeconds = 200;
	float _divideChance;
	
	public float _deathDelayInSeconds = 200;
	float _deathChance;

	public float _mutationFactor = 0;
	public float _mutateChance;

	public bool onlyDivideIntoEmptyNeighbour = true;

	float mutateChance;
	public bool _mutated = false;

	public FlatHexGrid<CellScript> grid;
	public FlatHexPoint hexPoint;
	public PlayArea area;
	private List<FlatHexPoint> neighbors;

	public bool animating = false;
	public Vector3 animationTarget;
	
	void Update () {
		if (!animating) return;
		iTween.MoveUpdate(gameObject, iTween.Hash("position", animationTarget,
		                                          "islocal", true,
		                                          "time", 0.4f));
		if ( Vector3.Distance(gameObject.transform.localPosition, animationTarget) < 0.001f) animating = false;
	}

	void FixedUpdate () {
		_divideChance = 1 / (_divideDelayInSeconds * (1 / Time.fixedDeltaTime));
		_deathChance = 1 / (_deathDelayInSeconds * (1 / Time.fixedDeltaTime));
		if (!_mutated) _mutateChance = (_mutationFactor * Time.fixedDeltaTime) / 100;

		//if (Die ()) {
		//	return;
		//}


		Divide ();

		//Mutate ();
	}

	bool Die () {
		if (_deathChance > Random.Range (0.0f,1.0f) ||
			!_mutated && grid.GetNeighbors(hexPoint, (CellScript n) => n != null && n._mutated).Count() == 6) {
			ObjectPool.Recycle (this);
		}

		return false;
	}

	void Divide () {
		
		if (_divideChance > Random.Range (0.0f,1.0f)) {


			foreach (FlatHexPoint direction in grid.GetNeighborDirections()) {
				FlatHexPoint neighbour = hexPoint + direction;
				if (grid.Contains(neighbour) && grid[neighbour] == null) {
					// send ourselves as the prefab!
					area.SpawnCell(area.healthyCellPrefab, neighbour, direction).Mimic (this);
					return;
				}
			}

			if (grid.GetNeighbors(hexPoint, (CellScript n) => n._mutated).Count() == 6) {
				// don't split, totally surrounded by cancer.
				return;
			}

			if (!onlyDivideIntoEmptyNeighbour) {
				// divide anyway because cancer!
				
				FlatHexPoint[] directions = grid.GetNeighborDirections()
					.Where(d => grid.Contains(hexPoint + d))
					.ToArray();
				FlatHexPoint dir = directions[Random.Range(0, directions.Length)];
				area.SpawnCell(area.healthyCellPrefab, hexPoint + dir, dir).Mimic (this);
			}
		}
	}

	void Mutate () {
		if (_mutateChance > Random.Range (0.0f,1.0f)) {
			// become cancer cell

			_divideDelayInSeconds /= 20;
			_deathDelayInSeconds *= 20;

			_mutateChance = 0;
			_mutated = true;

			onlyDivideIntoEmptyNeighbour = false;

			Material newMat = Resources.Load ("CancerCellMaterial", typeof(Material)) as Material;
			gameObject.renderer.material = newMat;

			Debug.Log ("Mutated!");
		}
	}

	public void Mimic (CellScript c) {
		_divideDelayInSeconds = c._divideDelayInSeconds;
		_deathDelayInSeconds = c._deathDelayInSeconds;
		_divideChance = c._divideChance;
		_deathChance = c._deathChance;
		_mutationFactor = c._mutationFactor;
		onlyDivideIntoEmptyNeighbour = c.onlyDivideIntoEmptyNeighbour;
		_mutated = c._mutated;
		gameObject.renderer.material = c.gameObject.renderer.material;
		hexPoint = c.hexPoint;
	}

	public bool IsMutated () {
		return _mutated;
	}

	bool IsEmpty (FlatHexPoint point) {
		return grid[point] == null;
	}

}
