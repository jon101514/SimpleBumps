/** Jonathan So, jonso.gamedev@gmail.com
 * Handles stun collisions for tanks, including flashing when hit.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Collision : MonoBehaviour {

	private Tank_Control controller;
	private Material mat;
	private Color idColor;
	private Color flashColor = new Color(1/2f, 0, 0);

	private const int FLASH_FRAMES = 24;

	private void Awake() {
		controller = GetComponent<Tank_Control>();
		mat = GetComponentInChildren<MeshRenderer>().material;
	}

	public void SetIDColor(Color newIDColor) { idColor = newIDColor; }

	private void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag.Equals("Jumper") || coll.gameObject.tag.Equals("Shooter") || coll.gameObject.tag.Equals("Bullet")) {
			controller.TankStun(coll.transform.forward);
			StartCoroutine(ColorFlash());
			if (coll.gameObject.tag.Equals("Bullet")) { coll.gameObject.SetActive(false); }
		} else if (coll.gameObject.tag.Equals("Bumper")) {
			if (coll.gameObject.GetComponent<Tank_Bumper>().GetIsBumping()) {
				controller.TankStun(coll.transform.forward, true);
				StartCoroutine(ColorFlash());
			} else {
				controller.TankStun(coll.transform.forward);
				StartCoroutine(ColorFlash());
			}
		}
	}

	private IEnumerator ColorFlash() {
		for (int i = 0; i < FLASH_FRAMES; i++) {
			mat.color = Color.Lerp(flashColor, idColor, i / (float) FLASH_FRAMES);
			yield return new WaitForEndOfFrame();
		}	
		mat.color = idColor;
	}
}
