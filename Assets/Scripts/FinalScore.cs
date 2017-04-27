using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalScore : MonoBehaviour {
	
	void Start () {
		Text scoreText = GetComponent<Text>();
		scoreText.text = "Score: " + ScoreKeeper.score.ToString();
		ScoreKeeper.Reset ();
	}
}
