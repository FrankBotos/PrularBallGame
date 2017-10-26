using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
	
	public float moveSpeed = 5.0f;
	public bool ballGoesUp = false;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

		//move
		if (ballGoesUp == true) {
			transform.Translate (moveSpeed * Vector3.up * Time.deltaTime);
		} else {
			transform.Translate (moveSpeed * Vector3.down * Time.deltaTime);
		}

	}

	void OnDestroy(){

		//when ball is destroyed we find the "Text" object, then get a reference to the score script attached to that object, then update the score
		GameObject tempRef = GameObject.Find ("Text");
		ScoreController scoreRef = tempRef.GetComponent<ScoreController> ();
		scoreRef.score++;

	}

}