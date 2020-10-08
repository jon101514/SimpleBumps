/** Jonathan So, jonso.gamedev@gmail.com
 * This tank can boost forward to knock opponents back.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Bumper : MonoBehaviour {

	// PUBLIC
	public KeyCode bump;

	// PRIVATE
	private bool isBumping = false;
	private float timer = 0;

	// CONSTANTS
	private const float BUMP_FORCE = 512f;
	private const float BUMP_VEL = 16f;
	private const float BUMP_TIME = 1/8f;
	private const float COOL_DOWN = 1/2f;

	// COMPONENTS
	private Rigidbody rb;
	private Tank_Control tank;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		tank = GetComponent<Tank_Control>();
	}

	private void Update() {
		UpdateTimer();
		if (Input.GetKeyDown(bump) && timer <= 0) {
			StartCoroutine(Bump());
		}
	}

	private void UpdateTimer() {
		timer -= Time.deltaTime;
	}	

	public void PublicBump() {
		StartCoroutine(Bump());
	}

	private IEnumerator Bump() {
		if (StateManager.instance.GetState() == 2) {
			
		ParticleSystemManager.instance.DisplayParticlesAt(transform.position, true);
		timer = COOL_DOWN;
		// rb.velocity = (transform.forward * BUMP_VEL);
		rb.velocity = (transform.forward * BUMP_VEL);
		isBumping = true;
		yield return new WaitForSeconds(BUMP_TIME);
		isBumping = false;
		tank.TankStun(Vector3.zero);
		}
	}

	public bool GetIsBumping() { return isBumping; }
}
