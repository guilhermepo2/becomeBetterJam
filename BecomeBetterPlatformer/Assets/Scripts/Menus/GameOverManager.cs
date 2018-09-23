using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {


	private IEnumerator ReloadGameRoutine() {
		yield return new WaitForSeconds(4.0f);
		SceneManager.LoadScene("MainMenu");
	}
	void Start() {
		StartCoroutine(ReloadGameRoutine());
	}
}
