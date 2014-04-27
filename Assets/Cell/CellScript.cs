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

	public Animator anim;

	public float timeSinceMutation;
	public float animTime = 1.0f;
	public bool cancer;

	void Start(){
		anim = this.GetComponent<Animator>();
		StartCoroutine (StartCoroutineDie());
	}

	void Update () {

		if(gameObject.tag == "healthy" && _mutated){
			if(Time.time - timeSinceMutation >= animTime){
				//anim.SetBool("cancer", true);
				anim.runtimeAnimatorController = area.cancerCellPrefab.gameObject.GetComponent<Animator>().runtimeAnimatorController;
				gameObject.tag = "cancer";
			}
		}

		if (!animating) return;
		iTween.MoveUpdate(gameObject, iTween.Hash("position", animationTarget,
		                                          "islocal", true,
		                                          "time", 0.4f));
		if ( Vector3.Distance(gameObject.transform.localPosition, animationTarget) < 0.001f) animating = false;
	}

	void FixedUpdate () {

		_divideChance = 1 / (_divideDelayInSeconds * (1 / Time.fixedDeltaTime));
		//_deathChance = 1 / _deathDelayInSeconds;
		if (!_mutated) _mutateChance = (_mutationFactor * Time.fixedDeltaTime) / 100;
		
		Divide ();

		Mutate ();
	}

	bool Die () {

		Destroy (this.gameObject);

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

			if (grid.GetNeighbors(hexPoint, (CellScript n) => n._mutated).Count() == 
			    grid.GetNeighbors(hexPoint, (CellScript n) => n != null).Count()) {
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

			_divideDelayInSeconds /= 4;
			_deathDelayInSeconds *= 4;

			_mutateChance = 0;
			_mutated = true;

			onlyDivideIntoEmptyNeighbour = false;

			//Material newMat = Resources.Load ("CancerCellMaterial", typeof(Material)) as Material;
			//gameObject.renderer.material = newMat;

			anim.SetBool("mutate", true);
			timeSinceMutation = Time.time;
			Debug.Log("timesincemutation: " + timeSinceMutation);

			GetComponent<SpriteRenderer>().sortingLayerName = "cancer";
			GetComponent<SpriteRenderer>().sortingOrder = 1;

			Debug.Log ("Mutated!");
		}
	}

	public bool IsMutated () {
		return _mutated;
	}


	bool IsEmpty (FlatHexPoint point) {
		return grid[point] == null;
	}

	IEnumerator StartCoroutineDie () {
		yield return new WaitForSeconds (Random.Range (0, _deathDelayInSeconds * 2f));
		Die ();

	}

}
