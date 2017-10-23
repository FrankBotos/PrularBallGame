using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {

	private int dir = 0;
	public float movementSpeed = 500f;

	// Use this for initialization
	void Start () {
		dir = Random.Range (0,2);
	}
	
	// Update is called once per frame
	void Update () {
		if (dir == 1) {
			//Debug.Log ("We're going up!");
			transform.Translate(Vector3.up * movementSpeed  * Time.deltaTime);
		} else {
			//Debug.Log ("We're going down!");
			transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);
		}
	}
}