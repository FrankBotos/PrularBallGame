using UnityEngine;
using System.Collections;
using System.Collections.Generic;//need to add this line in order to use generic C# lists

public class GameController : MonoBehaviour {

	public GameObject ball;

	public float spawnTimeSeconds;
	public int difficultyLevel;

	// Use this for initialization
	void Start () {

		difficultyLevel = 0;
		spawnTimeSeconds = 1.5f;

		InvokeRepeating ("SpawnBall", spawnTimeSeconds, spawnTimeSeconds);


	}
	
	// Update is called once per frame
	void Update () {
	}

	void SpawnBall(){
		//make a list to contain all possible spawn spawn points for the ball
		List<Vector3> spawnPositions = new List<Vector3> ();

		//dynamically convert spawn points to Vector3s and add them to our list
		spawnPositions.Add (Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/4, Screen.height, 10))); // top left
		spawnPositions.Add (Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/4, 0, 10))); // bottom left
		spawnPositions.Add (Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/1.3333f, Screen.height, 10))); // top right
		spawnPositions.Add (Camera.main.ScreenToWorldPoint (new Vector3(Screen.width/1.3333f, 0, 10))); // bottom right

		//randomize spawn point (within bounds of spawn position list (NOTE: This algorithm can be changed later on)
		int whereToSpawn = Random.Range(0,spawnPositions.Count);

		//now we get a reference to the script that controls the ball's behaviour, and alter the direction based on where it spawns
		BallBehaviour script = ball.GetComponent <BallBehaviour>();

		//now that we have a reference to the script, we alter the direction of the ball, based on the spawn position, before it spawns
		if(whereToSpawn == 0 || whereToSpawn == 2){//if it spawns on the top, ball moves down
			script.ballGoesUp = false; //1 means the ball goes up
		} else if (whereToSpawn == 1 || whereToSpawn == 3) {//if it spawns on the bottom, ball moves up
			script.ballGoesUp = true;
		}


		Instantiate (ball, spawnPositions[whereToSpawn] , transform.rotation);

	}

}