/** Jonathan So, jonso.gamedev@gmail.com
 * Tracks the number of active players in the game, adding/removing them as necessary.
 * When there's only one player, then they've won.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour {

	// SINGLETON
	public static PlayerTracker instance;

	// PRIVATE
	private List<Tank_Control> players; // We're controlling the speed of the players.
	private bool gameWon = false;
	private Vector3 camInitLook;
	private Vector3 camInitPos;

	// CONSTANT
	private const float LERP_TIME = 3/2f;
	private const float LERP_SPEED = 1/10f;

	private void Awake() {
		if (instance == null) { instance = this; }
		camInitLook = Camera.main.transform.forward;
		camInitPos = Camera.main.transform.position;
	}

	private void OnEnable() {
		gameWon = false;
	}

	public void FindPlayers() {
		players = new List<Tank_Control>(FindObjectsOfType<Tank_Control>());
		GameSpeedManager.instance.UpdatePlayers();
	}

	public void RemovePlayer(Tank_Control px) {
		players.Remove(px);
		px.gameObject.SetActive(false);
		// There is a definitive winner
		if (players.Count == 1 && !gameWon) {
			StartCoroutine(Win(players[0]));
		}
		GameSpeedManager.instance.UpdatePlayers();
	}

	private IEnumerator Win(Tank_Control winner) {
		Vector3 final = winner.transform.position;
		float timer = 0f;
		gameWon = true;

		if (players.Count > 1) {
			players.Clear();
			players = new List<Tank_Control>(Transform.FindObjectsOfType<Tank_Control>());
		} else if (players.Count <= 0) {
			Debug.Log("NO ONE WINS"); // This doesn't occur.
		} else {
			while (players[0] == null) {
				players.RemoveAt(0);
			}
			Debug.Log(players[0].name + " WINS");
			players[0].playerController.UpdateScore();
			while (timer < LERP_TIME) {
				Camera.main.transform.LookAt(Vector3.Lerp(camInitLook, final, timer / LERP_TIME));
				Camera.main.transform.Translate(Vector3.forward * LERP_SPEED);
				timer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			// Wait an amount of time to showcase the winner.
			yield return new WaitForSeconds(LERP_TIME);
			// Now, revert the camera to its original position.
			timer = 0f;	
			while (timer < LERP_TIME) {
				Camera.main.transform.LookAt(Vector3.Lerp(final, camInitLook, timer / LERP_TIME));
				Camera.main.transform.Translate(-Vector3.forward * LERP_SPEED);
				timer += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			Camera.main.transform.forward = camInitLook;
			Camera.main.transform.position = camInitPos;
			StateManager.instance.IncreaseState();
		}
		yield return new WaitForEndOfFrame();
	}

	public List<Tank_Control> GetPlayers() { return players; }

}
