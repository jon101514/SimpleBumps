/** Jonathan So, jonso.gamedev@gmail.com
 * Acts as an object pool for particle systems.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour {

	// SINGLETON
	public static ParticleSystemManager instance;

	// PUBLIC
	public GameObject systemPrefab, boostPrefab;

	// PRIVATE
	private List<GameObject> systems, boosts;

	// CONSTANT
	private const int MAX_SYSTEMS = 8;

	/** Populate the two object pools with objects,
	 * one for each player.
	 */
	private void Awake() {
		if (instance == null) { instance = this; }
		systems = new List<GameObject>();
		boosts = new List<GameObject>();
		for (int i = 0; i < MAX_SYSTEMS; i++) {
			GameObject sys = (GameObject) Instantiate (systemPrefab);
			sys.SetActive(false);
			systems.Add(sys);
			GameObject boostSys = (GameObject) Instantiate (boostPrefab);
			boostSys.SetActive(false);
			boosts.Add(boostSys);
		}
	}

	/** Displays a particle effect at a specified location.
	 * Depending on if it's a boost or a normal collision, spawn
	 * the appropriate effect.
	 */
	public void DisplayParticlesAt(Vector3 location, bool isBoost = false) {
		if (isBoost) {
			for (int i = 0; i < MAX_SYSTEMS; i++) {
				GameObject curr = boosts[i];
				if (!curr.activeInHierarchy) {
					curr.transform.position = location;
					curr.SetActive(true);
					break;
				}
			}
		} else {
			for (int i = 0; i < MAX_SYSTEMS; i++) {
				GameObject curr = systems[i];
				if (!curr.activeInHierarchy) {
					curr.transform.position = location;
					curr.SetActive(true);
					break;
				}
			}
		}
	}

}
