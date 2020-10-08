/* Jonathan So
 * Manages the game states of 0 - Setup | 1 - Cooldown | 2 - Game | 3 - Reset to Setup
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour {

	// SINGLETON
	public static StateManager instance;

	// PUBLIC
	public GameObject setupGroup; // The instructions that display during the 0 - Setup state.
	public Text countdown; // The countdown number that displays during the 1 - Countdown state.
	public int fakeState = -1; // Used for faking states on the title screen.

	// PRIVATE
	private PlayerController[] players;
	[SerializeField]
	private int state = 0;

	private void Awake() {
		if (instance == null) { instance = this; }
		players = Transform.FindObjectsOfType<PlayerController>();
		EvaluateState();
	}

	private void Update() {
		if (state == 0) {
			bool ready = true;
			foreach (PlayerController px in players) {
				if (px.GetPlayerChosen() == false) {
					ready = false;
					break;
				}
			}
			if (ready) {
				IncreaseState();
			}
			// Going back to start
			if (Input.GetKeyDown(KeyCode.Escape)) {
				SceneFadeOut.instance.FadeToBlack("Title Screen");
			}
		}
	}

	public int GetState() { return state; }

	public void IncreaseState() {  
		state++;
		if (state > 3) { state = 0; }
		EvaluateState();
	}

	private void EvaluateState() {
		if (fakeState != -1) { state = fakeState; }
		setupGroup.SetActive(false);
		// countdown.gameObject.SetActive(false);
		if (state == 0) {
			setupGroup.SetActive(true);
		}
		if (state == 1) {
			countdown.gameObject.SetActive(true);
			PlayerTracker.instance.FindPlayers();
			StartCoroutine(Countdown());
		}
		if (state == 2) {
			PlayerControllerManager.instance.enabled = false;
		}
		if (state == 3) { // Reset to Setup
			PlayerTracker.instance.enabled = false;
			GameSpeedManager.instance.enabled = false;
			LevelGenerator.instance.enabled = false;
//			PlayerControllerManager.instance.enabled = false;
			PlayerTracker.instance.enabled = true;
			GameSpeedManager.instance.enabled = true;
			LevelGenerator.instance.enabled = true;
			PlayerControllerManager.instance.enabled = true;
			foreach (PlayerController px in players) {
				px.DestroyPlayer();
			}
			IncreaseState();
		}
	}

	private IEnumerator Countdown() {
		countdown.text = "3";
		yield return new WaitForSeconds(1f);
		countdown.text = "2";
		yield return new WaitForSeconds(1f);
		countdown.text = "1";
		yield return new WaitForSeconds(1f);
		countdown.text = "BUMP!";
		yield return new WaitForSeconds(1f);
		IncreaseState();
	}
}
