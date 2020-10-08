/** Jonathan So, jonso.gamedev@gmail.com
 * This tank can shoot bullets which knock opponents backwards.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank_Shooter : MonoBehaviour {

	// PUBLIC
	public KeyCode shoot;
	public GameObject bulletPrefab;

	// PRIVATE
	private GameObject bullet;
	private bool canShoot;

	// CONSTANTS
	private const float COOL_DOWN = 1f;

	// COMPONENTS

	private void Awake() {
		bullet = (GameObject) Instantiate(bulletPrefab);
		bullet.SetActive(false);
		canShoot = true;
	}

	private void Update() {
		if (Input.GetKeyDown(shoot)) {
			Shoot();
		}
	}

	public void PublicShoot() {
		Shoot();
	}

	/** Shooting has a one-second cooldown; if that cooldown's expired, then 
	 * "shoot" our bullet by setting it to the position and rotation of our cannon.
	 * No instantiation here, folks!
	 */
	private void Shoot() {
		if (!bullet.activeInHierarchy && StateManager.instance.GetState() == 2 && canShoot) {
			canShoot = false;
			bullet.transform.position = transform.position + (transform.forward);
			bullet.transform.rotation = transform.rotation;
			bullet.SetActive(true);
			Invoke("ShootCoolDown", COOL_DOWN);
		}
	}

	private void ShootCoolDown() { canShoot = true; }

	private void OnDestroy() {
		Destroy(bullet);
	}
}
