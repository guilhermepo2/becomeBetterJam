using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public Animator mainMenuAnimator;
	public AudioSource soundReference;

	float timeToWait = 2.0f;

	private IEnumerator LoadSceneRoutine() {
		int pass = 10;
		float f_pass = timeToWait / pass;

		for(int i = 0; i < pass; i++) {
			soundReference.volume -= f_pass;
			yield return new WaitForSeconds(f_pass);
		}
		
		SceneManager.LoadScene("SampleScene");
	}
	public void StartGame() {
		mainMenuAnimator.Play("FadeOut");
		StartCoroutine(LoadSceneRoutine());
	}
	
	public void QuitGame() {
		Application.Quit();
	}
}
