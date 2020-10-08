/** Jonathan So, jonso.gamedev@gmail.com
 * Creates and sets up the player, setting their controls, color, and tank type.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	// PUBLIC
	public int id;
	public int score = 0;
	public KeyCode left, center, right;
	public GameObject leftChoice, centerChoice, rightChoice;
	public Vector3 forward; 
	public Color idColor;
	public GameObject hud;

	// PRIVATE
	private GameObject player;

	private void Awake() {
		transform.forward = forward;
		UpdateHUD();
	}

	public void UpdateScore() {
		score++;
		UpdateHUD();
	}

	private void UpdateHUD() {
		// hud.GetComponent<Text>().text = "P" + id + " - " + score + "\n" + left.ToString() + "    " + center.ToString() + "    " + right.ToString();
	}

	private void Update() {
		if (player != null) { return; }
		if (Input.GetKeyDown(left) && StateManager.instance.GetState() == 0) {
			player = Instantiate(leftChoice, transform.position, transform.rotation);
			SetPlayerTransform();
			SetPlayerControls();
			SetPlayerColor();
			PlayerControllerManager.instance.ControllerReady(this);
		} else if (Input.GetKeyDown(center) && StateManager.instance.GetState() == 0) {
			player = Instantiate(centerChoice, transform.position, transform.rotation);
			SetPlayerTransform();
			SetPlayerControls();
			SetPlayerColor();
			PlayerControllerManager.instance.ControllerReady(this);
		} else if (Input.GetKeyDown(right) && StateManager.instance.GetState() == 0) {
			player = Instantiate(rightChoice, transform.position, transform.rotation);
			SetPlayerTransform();
			SetPlayerControls();
			SetPlayerColor();
			PlayerControllerManager.instance.ControllerReady(this);
		}
	}

	private void SetPlayerTransform() {
		player.transform.forward = this.transform.forward;
		Tank_Control playerControls = player.GetComponent<Tank_Control>();
		playerControls.playerController = this;
		if (forward.z == -1) {
			playerControls.SetRotAngle(180f);
		}
	}

	private void SetPlayerControls() {
		// Add more types as time goes on
		Tank_Control playerControls = player.GetComponent<Tank_Control>();
		playerControls.left = this.left;
		playerControls.right = this.right;
		if (player.GetComponent<Tank_Bumper>() != null) {
			player.GetComponent<Tank_Bumper>().bump = this.center;
		} else if (player.GetComponent<Tank_Jumper>() != null) {
			player.GetComponent<Tank_Jumper>().jump = this.center;
		} else if (player.GetComponent<Tank_Shooter>() != null) {
			player.GetComponent<Tank_Shooter>().shoot = this.center;
		}
	}

	private void SetPlayerColor() {
		player.GetComponentInChildren<MeshRenderer>().material.color = idColor;
		player.GetComponent<Tank_Collision>().SetIDColor(idColor);
	}

	public bool GetPlayerChosen() { return player != null; }

	public void DestroyPlayer() { 
		Destroy(player); 
		player = null;
	}
}
