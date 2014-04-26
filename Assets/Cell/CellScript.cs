using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Grids;

public class CellScript : MonoBehaviour {

	public float _divideDelayInSeconds = 200;
	float _divideChance;
	
	public float _deathDelayInSeconds = 200;
	float _deathChance;
	

	float mutateChance;
	
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
		if (Die ()) {
			return;
		}


		Divide ();
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
		}
	}


	bool IsEmpty (FlatHexPoint point) {
		return grid[point] == null;
	}

}
