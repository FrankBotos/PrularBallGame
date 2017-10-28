using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
	
	public float moveSpeed = 5.0f;
	public bool ballGoesUp = false;
	public bool isVisibleByCamera;

	void Start(){
	}

	// Update is called once per frame
	void Update () {
		//move
		if (ballGoesUp == true) {
			transform.Translate (moveSpeed * Vector3.up * Time.deltaTime);
		} else {
			transform.Translate (moveSpeed * Vector3.down * Time.deltaTime);
		}

		//check if visible by camera
		//we check if ball is within y fulstrum of camera
		Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
		isVisibleByCamera = screenPoint.y > 0 && screenPoint.y < 1;
		if (!isVisibleByCamera){
			Destroy (gameObject);
		}


	}
		
	void OnDestroy(){


		//we increase the score if the ball is visible by the camera only, since that means the player clicked
		if (isVisibleByCamera) {
			//when ball is destroyed we find the "Text" object, then get a reference to the score script attached to that object, then update the score
			GameObject tempRef = GameObject.Find ("Text");
			ScoreController scoreRef = tempRef.GetComponent<ScoreController> ();
			scoreRef.score++;
		} else {//otherwise, game over and we reset the game
			Debug.Log ("Game Over!");

			//temporary game over solution
			GameObject tempRef = GameObject.Find ("Text");
			ScoreController scoreRef = tempRef.GetComponent<ScoreController> ();
			scoreRef.score = 0;

		}



	}

}