using UnityEngine;
using System.Collections;
using UnityEngine.UI;//this gives us access to Text Component

public class ScoreController : MonoBehaviour {

	public int score = 0;

	Text scoreText;//will hold text component

	// Use this for initialization
	void Start () {
		score = 0;//set score to zero when game starts

		//initialize text component
		scoreText = GetComponent<Text> ();

	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = "Score: " + score;//output score through text component on screen
	}
}
