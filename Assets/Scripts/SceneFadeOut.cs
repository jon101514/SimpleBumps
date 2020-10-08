using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFadeOut : MonoBehaviour {

	// SINGLETON
	public static SceneFadeOut instance;

	// CONSTANT
	private const float FADE_INTERVAL = 1/64f;

	// COMPONENT
	private Image img; // The image attached, which fades the game to black.

	private void Awake() {
		if (instance == null) {
			instance = this;
		}
		img = GetComponent<Image>();
	}

	public void FadeToBlack(string scene) {
		StartCoroutine(FadeCoroutine(scene));
	}

	private IEnumerator FadeCoroutine(string scene) {
		float t = 0;
		while(t < 1) {
			img.color = Color.Lerp(Color.clear, Color.black, t);
			t += FADE_INTERVAL;
			yield return new WaitForEndOfFrame();
		}
		yield return new WaitForSeconds(1/4f);
		if (scene.Equals("quit")) {
			Application.Quit();
		} else {
			SceneManager.LoadScene(scene);
		}
	}
}
