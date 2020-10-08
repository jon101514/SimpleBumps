/** Jonathan So, jonso.gamedev@gmail.com
 * At the title screen, spawns in different tanks and have them demonstrate their actions
 * so that the players know what they do.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSpawner : MonoBehaviour {

	// PUBLIC
	public GameObject bumper, jumper, shooter; // Prefabs for tanks.
	public Color[] colors; // Colors for the tanks.

	// PRIVATE
	private List<GameObject> tanks;
	private List<bool> tankActivations;

	// CONSTANT
	private const int MAX_TANKS = 3;
	private const float SPAWN_TIME = 4f;

	private void Awake() {
		StartCoroutine(TankSpawn());
		tanks = new List<GameObject>();
		tankActivations = new List<bool>();
	}

	private IEnumerator TankSpawn() {
		while (true) {
			yield return new WaitForSeconds(SPAWN_TIME);
			if (tanks.Count <= MAX_TANKS) {
				tanks.Add(Spawn());
				tankActivations.Add(false);
			}
		}
	}

	/** Handles making the tanks perform their actions and destroying them.
	 * 
	 */
	private void Update() {
		for (int i = 0; i < tanks.Count; i++) {
			if (tanks[i] != null) {
				if (tanks[i].transform.position.x < 1 && tanks[i].transform.position.x > -1 && tankActivations[i] == false && Random.Range(0, 10) == 0) {
					PerformAction(tanks[i]);
					tankActivations[i] = true;
				} else if (tanks[i].transform.position.y < -4f) {
					tanks.RemoveAt(i);
					tankActivations.RemoveAt(i);
					// Destroy(tanks[i]);
				}	
			} 
		}
	}

	/** Spawns a random type of tank with a randomized color.
 	* 
 	*/
	private GameObject Spawn() {
		GameObject tank;
		int tankType = Random.Range(0, 3);
		if (tankType == 0) { // Bumper
			tank = Instantiate(bumper, transform.position, transform.rotation);
		} else if (tankType == 1) { // Jumper
			tank = Instantiate(jumper, transform.position, transform.rotation);
		} else { // Shooter
			tank = Instantiate(shooter, transform.position, transform.rotation);
		}
		tank.GetComponentInChildren<MeshRenderer>().material.color = colors[Random.Range(0, colors.Length)];
		tank.GetComponent<Tank_Collision>().SetIDColor(colors[Random.Range(0, colors.Length)]);
		return tank;
	}

	/** Have the tanks demonstrate their abilities.
	* 
	*/
	private void PerformAction(GameObject tank) {
		// Determine the tank's type and perform its action
		if (tank.GetComponent<Tank_Bumper>() != null) {
			tank.GetComponent<Tank_Bumper>().PublicBump();
			return;
		} else if (tank.GetComponent<Tank_Jumper>() != null) {
			tank.GetComponent<Tank_Jumper>().PublicJump();
			return;
		} else if (tank.GetComponent<Tank_Shooter>() != null) {
			tank.GetComponent<Tank_Shooter>().PublicShoot();
			return;
		} else {
			Debug.Log("PERFORM ACTION FAILED");
		}
	}
}
