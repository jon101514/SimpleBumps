/** Jonathan So, jonso.gamedev@gmail.com
 * Manages in-game timer for "overtime" mode where we increase the players' speeds and spawn
 * lightning to destroy the board.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpeedManager : MonoBehaviour {

	// SINGLETON
	public static GameSpeedManager instance;

	// PUBLIC
	public GameObject lightningPrefab;

	// PRIVATE
	[SerializeField] 
	private float timer = 0f;
	private List<Tank_Control> players; // We're controlling the speed of the players.
	[SerializeField]
	private bool overtime; // When on, constantly increase the speed of the players.

	// CONSTANT
	private const float IDEAL_TIME = 60f; // Time where the game remains "normal" and afterwards goes into overtime.
	private const float WAIT_TIME = 6f; // Amount of time to wait in between speed increases

	private void Awake() {
		if (instance == null) { instance = this; }
	}

	private void OnEnable() {
		timer = 0f;
		overtime = false;
		CancelInvoke("SpawnLightning");
	}

	public void UpdatePlayers() {
		players = PlayerTracker.instance.GetPlayers();
	}

	private void Update() {
		if (StateManager.instance.GetState() == 0) {
			return;
		}
		timer += Time.deltaTime;
		if (timer > IDEAL_TIME && !overtime) {
			StartCoroutine(Overtime());
		}
	}

	/** Begins overtime mode, where we increase every remaining player's speed and
	 * drop lightning bolts to destroy the board.
	 */
	private IEnumerator Overtime() {
		Debug.Log("OVERTIME");
		overtime = true;
		InvokeRepeating("SpawnLightning", 1/2f, 1/2f);
		while (true) {
			foreach (Tank_Control px in players) {
				if (px != null) {
					px.IncreaseSpeed();
				}
			}
			yield return new WaitForSeconds(WAIT_TIME);
		}
	}

	/** Spawns lightning at a random position of the board.
	 * Picks a tile off the grid to destroy by dropping a lightning bolt onto it.
	 */
	private void SpawnLightning() {
		int x, z;
		// Corners
		x = Random.Range(4, 13);
		z = Random.Range(2, 8);
		// Z Range
		if (Random.Range(0, 2) == 0) {
			z = Random.Range(-7, 8);
		}
		// X Range 
		if (Random.Range(0, 2) == 0 && Mathf.Abs(z) > 2) {
			x = Random.Range(-14, 13);
		}
		if (Random.Range(0, 2) == 0) { x *= -1; }
		if (Random.Range(0, 2) == 0) { z *= -1; }
		Vector3 lightningPos = new Vector3(x, 128, z);
		Instantiate(lightningPrefab, lightningPos, Quaternion.identity);
	}

}
