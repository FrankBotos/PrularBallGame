using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
	
	public float moveSpeed = 5.0f;
	public bool ballGoesUp = false;
	public bool isVisibleByCamera;
	public float screenOffset = 0.3f;

	public float spawnTime;
	public Vector3 spawnPosition;

	public enum MovementPattern {
		Straight,
		Wave,
	}

	public MovementPattern movementPattern = MovementPattern.Straight;

	void Start() {
		spawnTime = Time.time;
		spawnPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {
		float timeSinceSpawn = Time.time - spawnTime;

		// Compute new position
		if (movementPattern == MovementPattern.Straight) {
			transform.position = spawnPosition + moveSpeed * timeSinceSpawn * (ballGoesUp ? Vector3.up : Vector3.down);

		} else if (movementPattern == MovementPattern.Wave) {
			float vertical = moveSpeed * timeSinceSpawn * (ballGoesUp ? 1.0f : -1.0f);
			float horizontal = 1.0f * Mathf.Sin (vertical / 1.5f);
			transform.position = spawnPosition + new Vector3(horizontal, vertical, 0.0f);
		}

		//check if visible by camera
		//we check if ball is within y fulstrum of camera
		Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
		isVisibleByCamera = screenPoint.y > -screenOffset && screenPoint.y < 1 + screenOffset;
		if (!isVisibleByCamera){
			Destroy (gameObject);
		}
	}
		
	void OnDestroy(){
		GameObject gameControllerRef = GameObject.Find ("GameController");
		GameController gameController = gameControllerRef.GetComponent<GameController> ();

		//we increase the score if the ball is visible by the camera only, since that means the player clicked
		if (isVisibleByCamera) {
			gameController.TriggerScorePoint ();
		} else {
			gameController.TriggerGameOver ();
		}



	}

}