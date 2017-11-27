using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour {

	void Start() {
		
	}

	public void onPlayButtonPressed() {
		SceneManager.LoadScene("GameScreen", LoadSceneMode.Single);
	}
}
