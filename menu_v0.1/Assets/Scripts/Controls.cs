using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controls : MonoBehaviour{

	public float THRUST = 0.5f;
	public float FRICTIONMODIFIER = -5f;
	public Rigidbody rb;
	public float GRAVITYCONST = 9.81f;
	public float perspectiveSpeed = 0.5f;
	public float pinchSpeed = 0.5f;

	//public float AccelerometerUpdateInterval = 1.0f / 60.0f;
	//public float LowPassKernalWidthInSeconds = 0.1f;		//greater the value, the slower the acceleration will converge to the current input sampled *taken from unity docs*
	private Vector3 lowPassValue = Vector3.zero;
	private GameObject Controller;
	private GameObject PauseScreen;
	private GameObject DeathScreen;
	private Vector3 rightForce = new Vector3(1, 0, 0);//change these perpendicular relative to gravity
	private Vector3 leftForce = new Vector3(-1, 0, 0);// ** same as above
	private Vector3 downGravity = new Vector3(0, -1, 0);
	private Vector3 upGravity = new Vector3 (0, 1, 0);
	private Vector3 leftGravity = new Vector3 (-1, 0, 0);
	private Vector3 rightGravity = new Vector3 (1, 0, 0);
	private bool topRight = false;
	private bool topLeft = false;
	private bool bottomRight = false;
	private bool bottomLeft = false;



	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		lowPassValue = Input.acceleration;
		Controller = GameObject.Find("ControlCanvas");
		PauseScreen = GameObject.Find("PauseCanvas");
		DeathScreen = GameObject.Find ("DeathCanvas");
		PauseScreen.GetComponent<Canvas>().enabled = false;
		DeathScreen.GetComponent<Canvas>().enabled = false;
		//lowPassValue = Input.acceleration;
	}
	
	// Update is called once per frame
	void Update () {
		//float LowPassFilterFactor = AccelerometerUpdateInterval / LowPassKernalWidthInSeconds; //modifiable;

		// If right side of screen is touched
		if (bottomRight || Input.GetKey("d")) {
			addForce(rightForce, ForceMode.Impulse);
		}
		// If left side of screen is touched
		if (bottomLeft || Input.GetKey("a")) {
			addForce(leftForce, ForceMode.Impulse);
		}

		if (Input.GetKeyDown(KeyCode.Escape) && !DeathScreen.GetComponent<Canvas>().enabled) {
			Time.timeScale = 0;
			PauseScreen.GetComponent<Canvas>().enabled = true;
			Controller.GetComponent<Canvas>().enabled = false;
		}

		/*Vector3 forward = new Vector3(0,0,1); //always face along the z-plane // this can be used to rotate the player entity model KEEP THIS.
		Vector3 up = LowPassFilterAccelerometer(LowPassFilterFactor) * -1.0f; //get the upwards facing vector opposite of gravity
		Quaternion rotation = Quaternion.LookRotation (forward, up);
		transform.rotation = rotation;*/
		if (Screen.orientation == ScreenOrientation.Portrait) {
			Physics.gravity = downGravity * GRAVITYCONST;
		}
		if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
			Physics.gravity = upGravity * GRAVITYCONST;
		}
		if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
			Physics.gravity = leftGravity * GRAVITYCONST;
		}
		if (Screen.orientation == ScreenOrientation.LandscapeRight) {
			Physics.gravity = rightGravity * GRAVITYCONST;
		}

		//Physics.gravity = LowPassFilterAccelerometer (LowPassFilterFactor);
	}

	//Flag Handling for buttons
	public void TopRightDown(){
		topRight =! topRight;
	}

	public void TopRightUp(){
		topRight =! topRight;
	}

	public void TopLeftDown(){
		topLeft =! topLeft;
	}
	
	public void TopLeftUp(){
		topLeft =! topLeft;
	}

	public void BottomRightDown(){
		bottomRight =! bottomRight;
	}
	
	public void BottomRightUp(){
		bottomRight =! bottomRight;
	}

	public void BottomLeftDown(){
		bottomLeft =! bottomLeft;
	}
	
	public void BottomLeftUp(){
		bottomLeft =! bottomLeft;
	}

	/*Vector3 LowPassFilterAccelerometer(float filter){ 
		float xfilter = Mathf.Lerp (lowPassValue.x, Input.acceleration.x, filter);
		float yfilter = Mathf.Lerp (lowPassValue.y, Input.acceleration.y, filter);
		lowPassValue = new Vector3(xfilter, yfilter, 0f);
		return lowPassValue;
	}*/

	public void addForce(Vector3 vect, ForceMode force){
		rb.AddForce (vect, force);
	}

}
