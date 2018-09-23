using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance;
	public AudioSource musicSource;
	public AudioClip standardMusic;
	public AudioClip gameOverMusic;
	public AudioSource sfxSource;

	void Awake() {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}
	}

	void Start() {
		Debug.Log("Sound Manager Start");
	}

	public void PlayClip(AudioClip clip) {
		sfxSource.PlayOneShot(clip);
	}

	public void PlayGameOverMusic() {
		musicSource.Stop();
		musicSource.clip = gameOverMusic;
		musicSource.loop = false;
		musicSource.Play();
	}

	public void PlayRegularMusic() {
		musicSource.Stop();
		musicSource.clip = standardMusic;
		musicSource.loop = true;
		musicSource.Play();
	}
}
