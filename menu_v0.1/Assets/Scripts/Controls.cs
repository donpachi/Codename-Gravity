using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	public float thrust = 365;
	public Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		// If right side of screen is touched
		if (Input.touchCount == 1 && Input.GetTouch (0).position.x >= Screen.currentResolution.width / 2) {
			Vector3 right = new Vector3(1, 0, 0);
			rb.AddForce(right * thrust, ForceMode.Acceleration);
		}
		// If left side of screen is touched
		if (Input.touchCount == 1 && Input.GetTouch (0).position.x < Screen.currentResolution.width / 2) {
			Vector3 left = new Vector3(-1, 0, 0);
			rb.AddForce(left * thrust, ForceMode.Acceleration);
		}
	}
}
