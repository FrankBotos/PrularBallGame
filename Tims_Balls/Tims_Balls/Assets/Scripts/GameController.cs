using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;//need to add this line in order to use generic C# lists

public class GameController : MonoBehaviour {

	public GameObject ballPrefab;
	public GameObject dividerPrefab;
	public GameObject gameOverText;
	public GameObject scoreController;

	private AudioSource audioSource;
	public AudioClip scoreSound;

	public bool isGameOver = false;

	public enum DifficultyStage {
		Stage1_Initial, // Balls only moving downwards
		Stage2_Divided, // Screen split and balls moving from both directions (20 balls in)
		Stage3_Warped, // Balls can now move in warped paths (40 balls in)
		Stage4_Reverse, // Balls can now reverse (50 balls in)
		Stage5_ReverseAndCross, // Balls can now reverse and cross over (60 balls in)
		Stage6_RedBalls, // Balls can now be red (70 balls in)
		// More to be added...
	}

	public DifficultyStage difficultyStage = DifficultyStage.Stage1_Initial;

	private float spawnFrequency = 1.0f; // # of balls per second
	private int numBallsSpawned = 0; // # of balls spawned so far
	private float timeOfLastSpawn;
	private float spawnFreqIncrement = 0.02f;

	void Awake () {
		audioSource = GetComponent<AudioSource>();
	}

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

		// Adjust the movement pattern based on the difficulty level
		float randomNum = Random.value;
		BallBehaviour.MovementPattern movementPattern = BallBehaviour.MovementPattern.Straight;

		if (difficultyStage == DifficultyStage.Stage3_Warped) {
			if (randomNum < 0.7) {
				movementPattern = BallBehaviour.MovementPattern.Straight;
			}
			else {
				movementPattern = BallBehaviour.MovementPattern.Wave;
			}
		}
		else if (difficultyStage == DifficultyStage.Stage4_Reverse) {
			if (randomNum < 0.5) {
				movementPattern = BallBehaviour.MovementPattern.Straight;
			}
			else if (randomNum < 0.75) {
				movementPattern = BallBehaviour.MovementPattern.Wave;
			}
			else {
				movementPattern = BallBehaviour.MovementPattern.Reverse;
			}
		}
		else if (difficultyStage == DifficultyStage.Stage5_ReverseAndCross || difficultyStage == DifficultyStage.Stage6_RedBalls) {
			if (randomNum < 0.4) {
				movementPattern = BallBehaviour.MovementPattern.Straight;
			}
			else if (randomNum < 0.6) {
				movementPattern = BallBehaviour.MovementPattern.Wave;
			}
			else if (randomNum < 0.8) {
				movementPattern = BallBehaviour.MovementPattern.Reverse;
			}
			else {
				movementPattern = BallBehaviour.MovementPattern.ReverseAndCross;
			}
		}

		// Adjust the color type
		BallBehaviour.BallType ballType = BallBehaviour.BallType.WhiteBall;

		if (difficultyStage == DifficultyStage.Stage6_RedBalls || true) {
			ballType = Random.value > 0.3 ? BallBehaviour.BallType.WhiteBall : BallBehaviour.BallType.RedBall;
		}

		BallBehaviour spawnedBallScript = spawnedBall.GetComponent <BallBehaviour>();
		spawnedBallScript.movementPattern = movementPattern;
		spawnedBallScript.ballType = ballType;
	}

	private void UpdateDifficultLevel() {
		bool isDebugging = false;
		float ballSpawnMultiplier = isDebugging ? 2.0f : 1.0f;
		float numBallsSpawnedAdjusted = ballSpawnMultiplier * numBallsSpawned;

		if (difficultyStage == DifficultyStage.Stage1_Initial && numBallsSpawnedAdjusted > 20) {
			difficultyStage = DifficultyStage.Stage2_Divided;

			Instantiate (dividerPrefab, new Vector3(), Quaternion.identity);
		}
		else if (difficultyStage == DifficultyStage.Stage2_Divided && numBallsSpawnedAdjusted > 40) {
			difficultyStage = DifficultyStage.Stage3_Warped;
		}
		else if (difficultyStage == DifficultyStage.Stage3_Warped && numBallsSpawnedAdjusted > 50) {
			difficultyStage = DifficultyStage.Stage4_Reverse;
		}
		else if (difficultyStage == DifficultyStage.Stage4_Reverse && numBallsSpawnedAdjusted > 60) {
			difficultyStage = DifficultyStage.Stage5_ReverseAndCross;
		}
		else if (difficultyStage == DifficultyStage.Stage5_ReverseAndCross && numBallsSpawnedAdjusted > 70) {
			difficultyStage = DifficultyStage.Stage6_RedBalls;
		}
	}

	public void TriggerScorePoint() {
		if (isGameOver) {
			return;
		}

		ScoreController controller = scoreController.GetComponent<ScoreController>();
		controller.score++;

		audioSource.PlayOneShot(scoreSound);
	}

	public void TriggerGameOver() {
		if (isGameOver) {
			return;
		}

		isGameOver = true;
		gameOverText.SetActive (true);
	}
}