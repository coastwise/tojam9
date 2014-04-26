using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Grids;

public class CellScript : MonoBehaviour {

	public float _divideDelayInSeconds = 200;
	float _divideChance = 5;
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

		DivideCell ();

	}

	void DivideCell () {
		float rng = Random.Range (0.0f,1.0f);

		if (_divideChance > rng) {

			Debug.Log ("success");


			List<FlatHexPoint> freeNeighbors = neighbors.Where ((point) => grid[point] == null).ToList();
			//List<FlatHexPoint> freeNeighbors = neighbors.Where ((point) => !grid.Contains(point)).ToList();


			foreach (FlatHexPoint point in freeNeighbors) {

				if (grid[point] == null) {
					area.CreateCell(point);

					return;
				}

			}


		}
	}
	bool IsEmpty (FlatHexPoint point) {
		return grid[point] == null;
	}

}
