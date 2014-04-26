using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Grids;

public class CellScript : MonoBehaviour {

	public float _divideDelayInSeconds = 2;
	float _divideChance = 5;
	float mutateChance;
	
	FlatHexGrid<CellScript> grid;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate () {
		_divideChance = 1 / (_divideDelayInSeconds * (1 / Time.fixedDeltaTime));

		DivideCell ();

	}

	void DivideCell () {
		float rng = Random.Range (0.0f,1.0f);

		if (_divideChance > rng) {
			Debug.Log ("Success");
			//List<FlatHexPoint> neighbours = grid.GetNeighbors(point);
			//List<FlatHexPoint> free = neighbours.Where ((point) => grid[point] == null);

			//pointToSpawn = free[Random.Range(0,free.Count)];
			//grid.create(pointToSpawn);
		}
	}
	bool IsEmpty (FlatHexPoint point) {
		return grid[point] == null;
	}

}
