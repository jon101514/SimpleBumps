/** Jonathan So, jonso.gamedev@gmail.com
 * These tiles can crack (fall) when jumped on.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {

	// PRIVATE
	private float timer = 0;

	// CONSTANTS
	private const float FALL_TIME = 7/8f;

	// COMPONENTS
	private Collider coll;
	private MeshRenderer mr;

	private void Awake() {
		coll = GetComponent<Collider>();
		mr = GetComponent<MeshRenderer>();
	}

	public void PanelFall() {
		StartCoroutine(Fall());
	}

	/** Makes a panel disappear via transparency before destroying the collider.
 	* 
 	*/
	private IEnumerator Fall(){
		Color originalColor = mr.material.color;
		while (timer < FALL_TIME) {
			mr.material.color = Color.Lerp(originalColor, Color.clear, timer / FALL_TIME);
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		coll.enabled = false;
		mr.material.color = Color.clear;
		this.gameObject.SetActive(false);
	}
}
