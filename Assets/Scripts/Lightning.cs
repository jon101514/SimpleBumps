/** Jonathan So, jonso.gamedev@gmail.com
 * Lightning can destroy panels on contact.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

	// CONSTANT
	private const float LIFE_TIME = 1/8f;

	private void OnCollisionEnter(Collision coll) {
		if (coll.gameObject.tag.Equals("Panel")) {
			coll.gameObject.GetComponent<Panel>().PanelFall();
			Invoke("Destroy", LIFE_TIME);
		}
	}

	private void Destroy() {
		Destroy(this.gameObject);
	}	
}
