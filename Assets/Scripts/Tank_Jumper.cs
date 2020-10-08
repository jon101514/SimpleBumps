/** Jonathan So, jonso.gamedev@gmail.com
 * This tank can jump, and the force when they land cracks the tiles underneath.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Jumper : MonoBehaviour {

	// PUBLIC
	public KeyCode jump;

	// PRIVATE 
	private bool jumping = false;
	private float timer = 0;

	// CONSTANTS
	private const float JUMP_FORCE = 512f;
	private const float CANCEL_TIME = 4/60f;
	private const float COOL_DOWN = 3/4f;

	// COMPONENTS
	private Rigidbody rb;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	private void Update() {
		UpdateTimer();
		if (Input.GetKeyDown(jump) && timer <= 0) {
			Jump();
		}
	}

	private void UpdateTimer() {
		timer -= Time.deltaTime;
	}	

	public void PublicJump() {
		Jump();
	}

	private void Jump() {
		if (StateManager.instance.GetState() == 2 && Physics.Raycast(transform.position - (transform.forward * 0.5f), Vector3.down, 1f)) {
			timer = COOL_DOWN;
			Invoke("EnableJumping", CANCEL_TIME);
			rb.AddForce(transform.up * JUMP_FORCE);
		}
	}

	private void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag.Equals("Panel") && jumping) {
			coll.gameObject.GetComponent<Panel>().PanelFall();
			Invoke("CancelJumping", CANCEL_TIME);
		}
	}

	private void EnableJumping() {
		jumping = true;
	}

	private void CancelJumping() {
		jumping = false;
	}
}
