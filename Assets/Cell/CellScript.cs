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
	// Use this for initialization
	void Start () {
		neighbors = new List<FlatHexPoint>();
		neighbors.AddRange(area.GetNeighbors(hexPoint));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		_divideChance = 1 / (_divideDelayInSeconds * (1 / Time.fixedDeltaTime));
		_deathChance = 1 / (_deathDelayInSeconds * (1 / Time.fixedDeltaTime));
		if (!_mutated) _mutateChance = (_mutationFactor * Time.fixedDeltaTime) / 100;

		if (Die ()) {
			return;
		}


		Divide ();

		Mutate ();
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
			
			List<FlatHexPoint> freeNeighbors = neighbors.Where ((point) => grid[point] == null).ToList();
			
			foreach (FlatHexPoint point in freeNeighbors) {
				if (grid[point] == null) {
					// send ourselves as the prefab!
					area.SpawnCell(this, point);
					return;
				}
			}

			if (freeNeighbors.Count == 0 && !onlyDivideIntoEmptyNeighbour) {
				// divide anyway because cancer!

				FlatHexPoint[] directions = grid.GetNeighborDirections().ToArray();
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


	bool IsEmpty (FlatHexPoint point) {
		return grid[point] == null;
	}

}
