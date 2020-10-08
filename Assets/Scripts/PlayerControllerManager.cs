/** Jonathan So, jonso.gamedev@gmail.com
 * Handles letting up to 8 players join, and begins the match when 2 players have joined.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerManager : MonoBehaviour {

	// SINGLETON
	public static PlayerControllerManager instance;

	// PUBLIC
	public Text countdown; // Message that displays prompts to join and how much time there is left to join.

	// PRIVATE
	private List<PlayerController> controllers; // List of Controllers
	[SerializeField]
	private int psReady = 0; // Number of players ready.
	private int countdownTimer = 9;
	private IEnumerator currCountdown;

	// CONSTANT
	private const int SECONDS_LEFT = 9;
	private const int MAX_PLAYERS = 8;

	private void Awake() {
		if (instance == null) { instance = this; }
		controllers = new List<PlayerController>(GameObject.FindObjectsOfType<PlayerController>());
	}

	private void OnEnable() {
		psReady = 0;
		countdown.gameObject.SetActive(true);
		countdown.text = "PRESS ANY BUTTON TO JOIN!";
	}
	/*
	private void Start() {
		// Set each controller object to its "PRESS ANY BUTTON TO JOIN" state
		foreach (PlayerController pc in controllers) {
			// #TODO
		}
	}
	*/

	public void ControllerReady(PlayerController pc) {
		psReady++;
		// If there are at least two players in the game, then begin the countdown.
		if (psReady >= 2) {
			if (currCountdown != null) { StopCoroutine(currCountdown); }
			if (psReady >= MAX_PLAYERS) { 
				StateManager.instance.IncreaseState();
				return;
			}
			currCountdown = Countdown();
			StartCoroutine(currCountdown);
		}
	}

	private IEnumerator Countdown() {
		countdownTimer = SECONDS_LEFT;
		while (countdownTimer >= 0) {
			countdown.text = countdownTimer.ToString();
			yield return new WaitForSeconds(1f);
			countdownTimer--;
		}
		StateManager.instance.IncreaseState();
	}

	private void OnDisable() {
		countdownTimer = 3;
		countdown.text = "";
		if (currCountdown != null) { StopCoroutine(currCountdown); }
	}
}
