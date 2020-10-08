/** Jonathan So, jonso.gamedev@gmail.com
 * Handles controls and the basic UI animations of the title screen, including scene transitions.
 * You can quit or choose to play on the title screen.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	// PUBLIC
	public Animator simple, bumps;
	public Text simpleText, bumpsText, pressAnyKeyText;

	// PRIVATE
	private bool canStart = false;

	// CONSTANT
	private const float SIMPLE_TIME = 0.833f;
	private const float SLOW_BLINK = 1/2f;
	private const float FAST_BLINK = 1/4f;

	// COMPONENT

	// Use this for initialization
	void Start () {
		simple.enabled = false;
		simpleText.enabled = false;
		bumps.enabled = false;
		bumpsText.enabled = false;
		StartCoroutine("TitleAnimation");
		StartCoroutine(AnyKeyBlink(false));
	}
	
	private IEnumerator TitleAnimation() {
		yield return new WaitForSeconds(SIMPLE_TIME);
		simple.enabled = true;
		simpleText.enabled = true;
		yield return new WaitForSeconds(SIMPLE_TIME);
		bumps.enabled = true;
		bumpsText.enabled = true;
		// yield return new WaitForSeconds(SIMPLE_TIME);
		simple.SetTrigger("slideOut");
		yield return new WaitForSeconds(SIMPLE_TIME);
		canStart = true;
	}

	private IEnumerator AnyKeyBlink(bool isFast) {
		float blinkTime = SLOW_BLINK;
		if (isFast) { blinkTime = FAST_BLINK; }
		while (true) {
			pressAnyKeyText.enabled = !pressAnyKeyText.enabled;
			yield return new WaitForSeconds(blinkTime);
		}
	}

	private IEnumerator StartGame() {
		StopCoroutine("AnyKeyBlink");
		StartCoroutine(AnyKeyBlink(true));
		yield return new WaitForSeconds(1.0f);
		SceneManager.LoadScene("Game");
	}

	private void Update() {
		if (Input.anyKey && canStart) { // Go to the game
			if (Input.GetKey(KeyCode.Escape)) {
				SceneFadeOut.instance.FadeToBlack("quit");
			} else {
				StartCoroutine(StartGame());
			}
		}
	}
}
