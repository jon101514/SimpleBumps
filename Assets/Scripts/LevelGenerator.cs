/** Jonathan So, jonso.gamedev@gmail.com
 * Generates the board for the game; also destroys it (and lightning) when the game needs to be reset.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	// SINGLETON
	public static LevelGenerator instance;

	// PUBLIC
	public GameObject panel;
	public Material[] panelColors;

	// PRIVATE
	private int panelIndex = 0;

	// CONSTANTS
	private const int WIDTH = 12;
	private const int HEIGHT = 7;

	private void Awake() {
		if (instance == null) { instance = this; } 
	}

	private void OnEnable() {
		GenTiles();
	}

	/** Generates a checkerboard pattern of tiles making up the level.
	 */
	private void GenTiles() {
		for (int i = -WIDTH; i <= WIDTH; i++) {
			for (int j = -HEIGHT; j <= HEIGHT; j++) {	
				GameObject pan = (GameObject) Instantiate(panel, new Vector3(i, 0, j), Quaternion.identity);
				pan.GetComponent<MeshRenderer>().material = panelColors[panelIndex];
				pan.transform.parent = this.transform;
				if (panelIndex == 1) {
					panelIndex = 0;
				} else {
					panelIndex = 1;
				}
			}
		}
	}

	private void OnDisable() {
		DestroyTiles();
	}

	/** Destroys all the tiles and lightning bolts when the game is reset.
	 *
	 */
	private void DestroyTiles() {
		Panel[] panels = GameObject.FindObjectsOfType<Panel>();
		foreach (Panel p in panels) {
			Destroy(p.gameObject);
		}
		Lightning[] sparks = GameObject.FindObjectsOfType<Lightning>();
		foreach (Lightning spark in sparks) {
			Destroy(spark.gameObject);
		}
	}
}
