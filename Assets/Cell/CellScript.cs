using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Gamelogic.Grids;
using System;


public class CellScript : MonoBehaviour {

	public enum CellProperty
		{
			Divide = 0,
			Death,
			Mutate
		}

	public float _divideDelayInSeconds = 200;
	float _divideChance;
	public float _calcDivide;

	public float _deathDelayInSeconds = 200;
	float _deathChance;
	public float _calcDeath;

	public float _mutationFactor = 0;
	public float _mutateChance;
	public float calcMutate;

	public bool onlyDivideIntoEmptyNeighbour = true;

	float mutateChance;
	public bool _mutated;

	public FlatHexGrid<CellScript> grid;
	public FlatHexPoint hexPoint;
	public PlayArea area;
	private List<FlatHexPoint> neighbors;

	public bool animating = false;
	public Vector3 animationTarget;

	public Animator anim;

	public float timeSinceMutation;
	public float animTime = 1.0f;

	public static int healthyCount = 0;
	public static int cancerCount = 0;

	public static float chemoCancerDivide = 1;
	public static float chemoCancerDeath = 1;
	public static float chemoCancerMutate = 1;
	public static float chemoHealthyDivide = 1;
	public static float chemoHealthyDeath = 1;
	public static float chemoHealthyMutate = 1;

	public float radiationDivideBuff = 1;
	public float radiationDeathBuff = 1;
	public float radiationMutateBuff = 1;

	public float dividerng;

	Action CellUpdate;

	void Start(){
		
		if (_mutated) {
			CellUpdate = Cancer;
		} else
			CellUpdate = Healthy;

		anim = this.GetComponent<Animator>();
	}

	void OnEnable () {
		if (_mutated) {
			cancerCount++;
		} else {
			healthyCount++;
		}
		
		StartCoroutine (StartCoroutineDie());
	}

	void OnDisable () {
		if (_mutated) {
			cancerCount--;
		} else {
			healthyCount--;
		}
	}

	void Update () {
		
		if (_mutated) {
			CellUpdate = Cancer;
		} else
			CellUpdate = Healthy;


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
		_deathChance = 1 / (_deathDelayInSeconds * (1 / Time.fixedDeltaTime));
		_mutateChance = (_mutationFactor * Time.fixedDeltaTime) / 100;

		CellUpdate();

		DieByAge ();
		Divide ();
		Mutate ();
	}

	void Healthy () {

		_calcDivide = _divideChance * chemoHealthyDivide * radiationDivideBuff;
		_calcDeath = _deathChance * chemoHealthyDeath * radiationDeathBuff;
		
		if (!_mutated) {
			calcMutate = _mutateChance * chemoHealthyMutate * radiationMutateBuff;
		}
	}

	void Cancer () {
		_calcDivide = _divideChance * chemoCancerDivide * radiationDivideBuff;
		_calcDeath = _deathChance * chemoCancerDeath * radiationDeathBuff;
		
		if (!_mutated) {
			calcMutate = _mutateChance * chemoCancerMutate * radiationMutateBuff;
		}
	}


	bool Die () {

		Destroy (this.gameObject);
		return false;
	}


	void DieByAge () {
		float rng = UnityEngine.Random.Range(0.0f,1.0f);

		dividerng = rng;

		if (_calcDeath > rng) {
			Die ();
		}
	}

	void Divide () {
		float rng = UnityEngine.Random.Range (0.0f,1.0f);



		if (_calcDivide > rng) {


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
				FlatHexPoint dir = directions[UnityEngine.Random.Range(0, directions.Length)];
				area.SpawnCell(this, hexPoint + dir, dir);
			}
		}
	}

	void Mutate () {
		float rng = UnityEngine.Random.Range (0.0f,1.0f);

		if (!_mutated && calcMutate > rng) {
			// become cancer cell

			_divideDelayInSeconds /= 2;
			_deathDelayInSeconds *= 2;

			_mutateChance = 0;
			_mutated = true;

			healthyCount--;
			cancerCount++;

			onlyDivideIntoEmptyNeighbour = false;

			//Material newMat = Resources.Load ("CancerCellMaterial", typeof(Material)) as Material;
			//gameObject.renderer.material = newMat;

			anim.SetBool("mutate", true);
			timeSinceMutation = Time.time;

			GetComponent<SpriteRenderer>().sortingLayerName = "cancer";
			GetComponent<SpriteRenderer>().sortingOrder = 1;

		}
	}

	public bool IsMutated () {
		return _mutated;
	}


	bool IsEmpty (FlatHexPoint point) {
		return grid[point] == null;
	}

	IEnumerator StartCoroutineDie () {
		while (true) {
				// check if surrounded by cancer
			yield return new WaitForSeconds (0.5f);

			if (!_mutated && grid != null && 
					grid.GetNeighbors (hexPoint, (CellScript n) => n != null && n._mutated).Count () == 
					grid.GetNeighbors (hexPoint, (CellScript n) => n != null).Count ()) {
					Die ();
			}
		}
	}

}
