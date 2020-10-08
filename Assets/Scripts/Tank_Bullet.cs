/** Jonathan So, jonso.gamedev@gmail.com
 * Bullet for the shooting tank. Handles movement and "destruction" (since we recycle this bullet).
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Bullet : MonoBehaviour {

	// CONSTANTS
	private const float SPEED = 1/3f;
	private const float X_BOUNDS = 17f;
	private const float Z_BOUNDS = 10f;
	private const float COLL_WAIT = 1/16f;	

	// COMPONENTS
	private Collider coll;
	private Transform tm;

	private void Awake() {
		coll = GetComponent<Collider>();
		tm = GetComponent<Transform>();
	}

	private void OnEnable() {
		StartCoroutine(ToggleCollider());
	}

	private void Update() {
		tm.Translate(Vector3.forward * SPEED);
		BoundsCheck();
	}

	private void BoundsCheck() {
		if (Mathf.Abs(tm.position.x) > X_BOUNDS || Mathf.Abs(tm.position.z) > Z_BOUNDS) {
			this.gameObject.SetActive(false);
		}
	}

	private IEnumerator ToggleCollider() {
		coll.enabled = false;
		yield return new WaitForSeconds(COLL_WAIT);
		coll.enabled = true;
	}
}
