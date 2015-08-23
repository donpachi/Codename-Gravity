using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	public float THRUST = 0.5f;
	public float FRICTIONMODIFIER = -5f;
	public Rigidbody rb;

	public float AccelerometerUpdateInterval = 1.0f / 60.0f;
	public float LowPassKernalWidthInSeconds = 0.1f;		//greater the value, the slower the acceleration will converge to the current input sampled *taken from unity docs*
	private Vector3 lowPassValue = Vector3.zero;
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		lowPassValue = Input.acceleration;
	}
	
	// Update is called once per frame
	void Update () {
		float LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernalWidthInSeconds; //modifiable;

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

		/*Vector3 forward = new Vector3(0,0,1); //always face along the z-plane // this can be used to rotate the player entity model KEEP THIS.
		Vector3 up = LowPassFilterAccelerometer(LowPassFilterFactor) * -1.0f; //get the upwards facing vector opposite of gravity
		Quaternion rotation = Quaternion.LookRotation (forward, up);
		transform.rotation = rotation;*/

		Physics.gravity = LowPassFilterAccelerometer (LowPassFilterFactor);

	}

	Vector3 LowPassFilterAccelerometer(float filter){ 
		float xfilter = Mathf.Lerp (lowPassValue.x, Input.acceleration.x, filter);
		float yfilter = Mathf.Lerp (lowPassValue.y, Input.acceleration.y, filter);
		lowPassValue = new Vector3(xfilter, yfilter, 0f);
		return lowPassValue;
	}

	// Add force to the player
	void addForce(Vector3 vector,ForceMode forceMode) {
		if (Mathf.Abs(rb.velocity.x) < 13)
			rb.AddForce (vector * THRUST, forceMode);
	}

}
