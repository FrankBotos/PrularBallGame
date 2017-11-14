using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

		//each time the game updates we check for a "mouse left click" or "tap" and convert it from world space to screen space
		if (Input.GetMouseButtonDown (0)) {

			Vector3 getMousePositionFar = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
			Vector3 getMousePositionNear = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

			Vector3 mousePosFar = Camera.main.ScreenToWorldPoint (getMousePositionFar);
			Vector3 mousePosNear = Camera.main.ScreenToWorldPoint (getMousePositionNear);
	
			//This is a debug raycast, it lets you see the raycast in the scene view if needed
			Debug.DrawRay(mousePosNear, mousePosFar - mousePosNear, Color.red);

			//we send out a ray cast during each "click" or "tap" and destroy the object that has been clicked
			RaycastHit2D hit = Physics2D.Raycast (mousePosNear, mousePosFar - mousePosNear);
			if (hit.collider != null) {
				Destroy (hit.transform.gameObject);
			}

		}
	}
}
