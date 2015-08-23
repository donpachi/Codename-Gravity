using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	public float THRUST = 0.5f;
	public float FRICTIONMODIFIER = -5f;
	public Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		// If right side of screen is touched
		if ((Input.touchCount == 1 && Input.GetTouch (0).position.x >= Screen.currentResolution.width / 2) || Input.GetKey("d")) {
			Vector3 right = new Vector3(1, 0, 0);
			addForce(right, ForceMode.Impulse);
		}
		// If left side of screen is touched
		if ((Input.touchCount == 1 && Input.GetTouch (0).position.x < Screen.currentResolution.width / 2) || Input.GetKey("a")) {
			Vector3 left = new Vector3(-1, 0, 0);
			addForce(left, ForceMode.Impulse);
		}
		
		//rb.AddForce(Input.gyro.gravity, ForceMode.Acceleration); not working as of yet
	}

	// Add force to the player
	void addForce(Vector3 vector,ForceMode forceMode) {
		if (Mathf.Abs(rb.velocity.x) < 13)
			rb.AddForce (vector * THRUST, forceMode);
	}

}
