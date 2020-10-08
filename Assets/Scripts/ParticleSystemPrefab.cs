/** Jonathan So, jonso.gamedev@gmail.com
 * Script for the particle systems, since they're pooled objects.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPrefab : MonoBehaviour {

	// PRIVATE
	private const float LIFE_TIME = 1f;

	// COMPONENT
	private ParticleSystem particles;

	private void Awake() {
		particles = GetComponent<ParticleSystem>();
	}

	private void OnEnable() {
		particles.Play();
		Invoke("Disable", LIFE_TIME);
	}

	private void Disable() {
		particles.Stop();
		this.gameObject.SetActive(false);
	}
}
