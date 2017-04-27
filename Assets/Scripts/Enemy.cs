using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float health = 500;
	public int pointsWorth = 50;
	public float projectileSpawnYOffset = -0.5f;
	public float projectileSpeed = 10;
	public float shotsPerSecond = 0.5f;
	public GameObject laser;
	public AudioClip laserSound;
	public AudioClip dieSound;

	private ScoreKeeper scoreKeeper;

	void Start(){
		scoreKeeper = FindObjectOfType<ScoreKeeper> ();
	}

	void Update(){
		float probability = Time.deltaTime * shotsPerSecond;
		if (Random.value < probability) {
			Fire ();
		}
	}

	void OnTriggerEnter2D(Collider2D collider){

		Projectile projectileScript = collider.GetComponent<Projectile> ();

		if (projectileScript) {
			health -= projectileScript.GetDamage ();
			if (health <= 0) {
				Destroyed();
			}
			projectileScript.Hit ();
			Debug.Log ("Ship hit by laser");
		}
	}

	void Destroyed(){
		AudioSource.PlayClipAtPoint (dieSound, transform.position);
		scoreKeeper.Score (pointsWorth);
		Destroy (gameObject);
	}
	void Fire(){
		Vector3 startPosition = transform.position + new Vector3 (0, projectileSpawnYOffset, 0);
		GameObject laserInstance = Instantiate (laser, startPosition, Quaternion.Euler(0,0,180)) as GameObject;
		laserInstance.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, -projectileSpeed, 0);
		AudioSource.PlayClipAtPoint (laserSound, transform.position);
	}
}
