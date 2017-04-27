using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 15;
	public float shipBoundaryOffset = 0;
	public float projectileSpawnYOffset = 0.5f;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public float health = 500;
	public GameObject laser;
	public AudioClip laserSound;

	private LevelManager levelManager;
	float xmin;
	float xmax;

	void Start(){
		levelManager = FindObjectOfType<LevelManager> ();
		SetBoundaries ();
	}

	void Update () {
		if(Input.GetKey("right")){
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		if (Input.GetKey ("left")) {
			transform.position += Vector3.left * speed * Time.deltaTime;
		}
		if (Input.GetKeyDown ("up")) {
			InvokeRepeating("Fire", 0.00001f, firingRate);
		}
		if (Input.GetKeyUp ("up")) {
			CancelInvoke("Fire");
		}
		ClampXMovement ();
	}

	void SetBoundaries(){
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distance));
		xmin = leftmost.x + shipBoundaryOffset;
		xmax = rightmost.x - shipBoundaryOffset;
	}

	void ClampXMovement (){
		float newX = Mathf.Clamp (transform.position.x, xmin, xmax);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
	}

	void OnTriggerEnter2D(Collider2D collider){
		
		Projectile projectileScript = collider.GetComponent<Projectile> ();
		
		if (projectileScript) {
			health -= projectileScript.GetDamage ();
			if (health <= 0) {
				Destroyed();
			}
			projectileScript.Hit ();
			Debug.Log ("Player hit by laser");
		}
	}

	void Destroyed(){
		Destroy (gameObject); //TODO make a more sensible outcome
		levelManager.LoadLevel ("Game Over");
	}

	void Fire(){
		Vector3 startPosition = transform.position + new Vector3 (0, projectileSpawnYOffset, 0);
		GameObject laserInstance = Instantiate (laser, startPosition, Quaternion.identity) as GameObject;
		laserInstance.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint (laserSound, transform.position);
	}
}
