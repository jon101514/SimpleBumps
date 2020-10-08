/** Jonathan So, jonso.gamedev@gmail.com
 * Every tank automatically moves forward and can turn left and right.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Control : MonoBehaviour {

	// PUBLIC
	public KeyCode left, right;
	public PlayerController playerController;

	// PRIVATE
	private float rotAngle = 0;
	private bool stunned = false;
	[SerializeField]
	private float speed;

	// CONSTANTS
	private const float INIT_SPEED = 1/24f;
	private const float MAX_SPEED = 1/8f;
	private const float TURN_SPEED = 1/32f;
	private const float THETA = 3;
	private const float STUN_TIME = 3/8f;
	private const float BUMP_FORCE = 512f;
	private const float BUMP_VEL = 12f;
	private const float BUMPER_BUMP_FORCE = 640f;
	private const float BUMPER_BUMP_VEL = 16f;
	private const float KOD_HEIGHT = -8f;

	// COMPONENTS
	private Rigidbody rb;
	private Transform tm;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
		tm = GetComponent<Transform>();
	}

	private void OnEnable() {
		speed = INIT_SPEED; // Reset to initial speed.
	}

	void Update () {
		if (!stunned && StateManager.instance.GetState() == 2) {
			if (Input.GetKey(left)) {
			rotAngle -= THETA;
			tm.rotation = (Quaternion.AngleAxis(rotAngle, Vector3.up));
			tm.Translate(Vector3.forward * TURN_SPEED);
			} else if (Input.GetKey(right)) {
				rotAngle += THETA;
				tm.rotation = (Quaternion.AngleAxis(rotAngle, Vector3.up));
				tm.Translate(Vector3.forward * TURN_SPEED);
			} else {
				tm.Translate(Vector3.forward * speed);
			}
		}
		CheckKOd();
	}

	/** Handles knockback and plays the particle effect since we've been bumped.
	 * If it's a Bumper tank that bumped us and they were charging forward, we get
	 * knocked back a little more.
	 */
	public void TankStun(Vector3 otherForward, bool bumperBump = false) {
		stunned = true;
		// if (bumperBump) { rb.AddForce(otherForward * BUMPER_BUMP_FORCE); }
		// else { rb.AddForce(otherForward * BUMP_FORCE); }
		if (bumperBump) { rb.velocity = (otherForward * BUMPER_BUMP_VEL); }
		else { rb.velocity = (otherForward * BUMP_VEL); }
		if (!otherForward.Equals(Vector3.zero)) { ParticleSystemManager.instance.DisplayParticlesAt(tm.position); }
		StartCoroutine(StunCooldown());
	}

	private IEnumerator StunCooldown() {
		yield return new WaitForSeconds(STUN_TIME);
		stunned = false;
	}

	public void SetRotAngle(float newRotAngle) {
		rotAngle = newRotAngle;
	}

	public void IncreaseSpeed(float added = 1/32f) {
		speed += added;
		speed = Mathf.Clamp(speed, INIT_SPEED, MAX_SPEED);
	}

	private void CheckKOd() {
		if (tm.position.y <= KOD_HEIGHT) {
			if (PlayerTracker.instance != null) {
				PlayerTracker.instance.RemovePlayer(this);
			} else {
				Destroy(this.gameObject);
			}
		}
	}
}
