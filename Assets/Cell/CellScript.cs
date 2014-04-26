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


	void FixedUpdate () {
		_divideChance = 1 / (_divideDelayInSeconds * (1 / Time.fixedDeltaTime));
		_deathChance = 1 / (_deathDelayInSeconds * (1 / Time.fixedDeltaTime));
		if (!_mutated) _mutateChance = (_mutationFactor * Time.fixedDeltaTime) / 100;

		if (Die ()) {
			return;
		}


		Divide ();

		//Mutate ();
	}

	bool Die () {
		float rng = Random.Range (0.0f,1.0f);

		if (_deathChance > rng) {

			// do a death animation and Destroy at the end
			Destroy (this.gameObject);
		}

		return false;
	}

	void Divide () {
		float rng = Random.Range (0.0f,1.0f);
		
		if (_divideChance > rng) {

			foreach (FlatHexPoint direction in grid.GetNeighborDirections()) {
				FlatHexPoint neighbour = hexPoint + direction;
				if (grid.Contains(neighbour) && grid[neighbour] == null) {
					// send ourselves as the prefab!
					area.SpawnCell(this, neighbour, direction);
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
				area.SpawnCell(this, hexPoint + dir, dir);
			}
		}
	}

	void Mutate () {
		float rng = Random.Range (0.0f,1.0f);

		if (_mutateChance > rng) {
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

	bool IsMutated () {
		return _mutated;
	}


	bool IsEmpty (FlatHexPoint point) {
		return grid[point] == null;
	}

}
