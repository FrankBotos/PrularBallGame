using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
	
	public float moveSpeed = 5.0f;
	public bool ballGoesUp = false;
	public bool isVisibleByCamera;
	public float screenOffset = 0.3f;

	public float spawnTime;
	public Vector3 spawnPosition;

	private SpriteRenderer spriteRenderer;

	public enum MovementPattern {
		Straight,
		Wave,
		Reverse,
		ReverseAndCross
	}

	public enum BallType {
		WhiteBall,
		RedBall
	}

	public BallType ballType = BallType.WhiteBall;

	public MovementPattern movementPattern = MovementPattern.Straight;

	void Start() {
		spawnTime = Time.time;
		spawnPosition = transform.position;
		spriteRenderer = GetComponent<SpriteRenderer> ();
		UpdateForBallType ();
	}

	void UpdateForBallType() {
		if (ballType == BallType.RedBall) {
			spriteRenderer.color = new Color (1.0f, 0.0f, 0.0f, 1.0f);
		}
		else if (ballType == BallType.WhiteBall) {
			spriteRenderer.color = new Color (1.0f, 1.0f, 1.0f, 1.0f);
		}
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
			transform.position = spawnPosition + new Vector3 (horizontal, vertical, 0.0f);

		} else if (movementPattern == MovementPattern.Reverse) {
			float vertical = 5.0f * Mathf.Sin (0.15f * moveSpeed * timeSinceSpawn) * (ballGoesUp ? 1.0f : -1.0f);
			float horizontal = 0.0f;
			transform.position = spawnPosition + new Vector3 (horizontal, vertical, 0.0f);

		} else if (movementPattern == MovementPattern.ReverseAndCross) {
			float t = 0.15f * moveSpeed * timeSinceSpawn;
			float offset = 0.2f;
			float tStartHoriz = Mathf.PI / 2.0f - offset;
			float vertical = 5.0f * Mathf.Sin (t) * (ballGoesUp ? 1.0f : -1.0f);
			float horizontal = 0.0f;
			float targetHoriz = -spawnPosition.x;

			if (t > tStartHoriz) {
				horizontal =  Mathf.Min((t - tStartHoriz) / (offset * 2.0f), 1.0f) * (targetHoriz - spawnPosition.x);
			}

			transform.position = spawnPosition + new Vector3 (horizontal, vertical, 0.0f);
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

		if (ballType == BallType.WhiteBall) {
			if (isVisibleByCamera) {
				gameController.TriggerScorePoint ();
			} else {
				gameController.TriggerGameOver ();
			}
		} else if (ballType == BallType.RedBall) {
			if (isVisibleByCamera) {
				gameController.TriggerGameOver ();
			} else {
				gameController.TriggerScorePoint ();
			}
		}
	}

}