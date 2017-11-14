using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;//need to add this line in order to use generic C# lists

public class GameController : MonoBehaviour {

	public GameObject ballPrefab;
	public GameObject dividerPrefab;
	public GameObject gameOverText;
	public GameObject scoreController;

	public bool isGameOver = false;

	public enum DifficultyStage {
		Stage1_Initial, // Balls only moving downwards
		Stage2_Divided, // Screen split and balls moving from both directions (30 balls in)
		Stage3_Warped, // Balls can now move in warped paths (60 balls in)
		// More to be added...
	}

	public DifficultyStage difficultyStage = DifficultyStage.Stage1_Initial;

	private float spawnFrequency = 1.0f; // # of balls per second
	private int numBallsSpawned = 0; // # of balls spawned so far
	private float timeOfLastSpawn;
	private float spawnFreqIncrement = 0.02f;

	// Use this for initialization
	void Start () {
		timeOfLastSpawn = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (isGameOver) {
			if (Input.GetMouseButtonDown(0)) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}

			return;
		}

		float timeSinceLastSpawn = Time.time - timeOfLastSpawn;
		float timeBetweenSpawns = 1.0f / spawnFrequency;
		bool shouldSpawnBall = timeSinceLastSpawn > timeBetweenSpawns;

		if (shouldSpawnBall) {
			SpawnBall ();

			timeOfLastSpawn = Time.time;
			numBallsSpawned++;

			// Slowly increase this over time
			spawnFrequency += spawnFreqIncrement;

			UpdateDifficultLevel ();
		}
	}

	void SpawnBall(){
		//make a list to contain all possible spawn spawn points for the ball
		List<Vector2> spawnPositions = new List<Vector2> ();

		//dynamically convert spawn points to Vector3s and add them to our list
		if (difficultyStage == DifficultyStage.Stage1_Initial) {
			spawnPositions.Add (new Vector2(Screen.width * 1.0f/5.0f, Screen.height));
			spawnPositions.Add (new Vector2(Screen.width * 2.0f/5.0f, Screen.height));
			spawnPositions.Add (new Vector2(Screen.width * 3.0f/5.0f, Screen.height));
			spawnPositions.Add (new Vector2(Screen.width * 4.0f/5.0f, Screen.height));
		} else {
			spawnPositions.Add (new Vector2(Screen.width * 1.0f/5.0f, Screen.height));
			spawnPositions.Add (new Vector2(Screen.width * 2.0f/5.0f, Screen.height));
			spawnPositions.Add (new Vector2(Screen.width * 3.0f/5.0f, 0));
			spawnPositions.Add (new Vector2(Screen.width * 4.0f/5.0f, 0));
		}

		//randomize spawn point (within bounds of spawn position list (NOTE: This algorithm can be changed later on)
		int whereToSpawn = Random.Range(0,spawnPositions.Count);
		Vector3 spawnPositionScreen = spawnPositions [whereToSpawn];
		spawnPositionScreen.z = 10;

		Vector3 spawnPositionWorld = Camera.main.ScreenToWorldPoint (spawnPositionScreen);

		//now we get a reference to the script that controls the ball's behaviour, and alter the direction based on where it spawns
		BallBehaviour script = ballPrefab.GetComponent <BallBehaviour>();

		if (spawnPositionScreen.y > Screen.height / 2) {
			script.ballGoesUp = false;
		} else {
			script.ballGoesUp = true;
		}

		GameObject spawnedBall = Instantiate (ballPrefab, spawnPositionWorld, Quaternion.identity);
		spawnedBall.transform.localScale = new Vector3 (0.7f, 0.7f, 1.0f);

		if (difficultyStage == DifficultyStage.Stage3_Warped) {
			BallBehaviour spawnedBallScript = spawnedBall.GetComponent <BallBehaviour>();
			spawnedBallScript.movementPattern = Random.value > 0.5 ? BallBehaviour.MovementPattern.Wave :BallBehaviour.MovementPattern.Straight;
		}
	}

	private void UpdateDifficultLevel() {
		if (difficultyStage == DifficultyStage.Stage1_Initial && numBallsSpawned > 30) {
			difficultyStage = DifficultyStage.Stage2_Divided;

			Instantiate (dividerPrefab, new Vector3(), Quaternion.identity);
		}
		else if (difficultyStage == DifficultyStage.Stage2_Divided && numBallsSpawned > 60) {
			difficultyStage = DifficultyStage.Stage3_Warped;
		}
	}

	public void TriggerScorePoint() {
		if (isGameOver) {
			return;
		}

		ScoreController controller = scoreController.GetComponent<ScoreController>();
		controller.score++;
	}

	public void TriggerGameOver() {
		if (isGameOver) {
			return;
		}

		isGameOver = true;
		gameOverText.SetActive (true);
	}
}