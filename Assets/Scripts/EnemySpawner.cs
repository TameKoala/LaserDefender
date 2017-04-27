using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemy;
	public float width = 10;
	public float height = 5;
	public float speed;
	[Tooltip("use this to set the delay between appearance of each enemy during spawn event")]
	public float spawnDelay = 0.5f;
	[Tooltip("use this to set initial direction of movement")]
	public bool movingRight = true;

	private float xmin;
	private float xmax;
	private bool movementActive;
	private Vector3 formationStartPosition;

	void Start () {
		//set boundaries for movement of formation
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distance));
		xmin = leftmost.x + (width / 2);
		xmax = rightmost.x - (width / 2);

		formationStartPosition = transform.position;
		movementActive = false;

		SpawnUntilFull();
	}

	void Update(){
		if (movementActive) {
			MoveShipFormation ();
		}

		if(AllMembersDead()){
			Debug.Log("All enemies destroyed. New wave created");
			movementActive = false;
			transform.position = formationStartPosition;
			SpawnUntilFull();
		}
	}

	public void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 1));
	}

	void SpawnShips(){
		foreach (Transform child in transform) {
			GameObject enemyInstance = Instantiate (enemy, child.position, Quaternion.identity)as GameObject;
			enemyInstance.transform.parent = child;
		}
	}

	void SpawnUntilFull(){
		Transform freePosition = NextFreePosition ();
		if (freePosition) {
			GameObject enemyInstance = Instantiate (enemy, freePosition.position, Quaternion.identity)as GameObject;
			enemyInstance.transform.parent = freePosition;

			Invoke ("SpawnUntilFull", spawnDelay);
		}else {
			movementActive = true;
		}
	}

	void MoveShipFormation (){
		if (movingRight) {
			transform.position += Vector3.right * speed * Time.deltaTime;
			if (transform.position.x >= xmax){
				movingRight = false;
			}
		} else if (!movingRight) {
			transform.position += Vector3.left * speed * Time.deltaTime;
			if (transform.position.x <= xmin){
				movingRight = true;
			}
		}
	}

	Transform NextFreePosition(){
		foreach (Transform childPositionGameObject in transform) {
			if(childPositionGameObject.childCount == 0){
				return childPositionGameObject;
			}
		}
		return null;
	}

	bool AllMembersDead(){
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0){
				return false;
			}
		}
		return true;
	}
}
