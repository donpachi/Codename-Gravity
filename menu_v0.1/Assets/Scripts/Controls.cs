using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Controls : MonoBehaviour{
	
	public float THRUST = 0.5f;
	public float FRICTIONMODIFIER = -5f;
	public Rigidbody2D rb;
	public float GRAVITYCONST = 9.81f;
	public float perspectiveSpeed = 0.5f;
	public float pinchSpeed = 0.5f;
	
	//public float AccelerometerUpdateInterval = 1.0f / 60.0f;
	//public float LowPassKernalWidthInSeconds = 0.1f;		//greater the value, the slower the acceleration will converge to the current input sampled *taken from unity docs*
	private Vector3 lowPassValue = Vector3.zero;
	private GameObject Controller;
	private GameObject PauseScreen;
	private GameObject DeathScreen;
	private Vector2 rightForce = new Vector2(1, 0);
	private Vector2 leftForce = new Vector2(-1, 0);
	private Vector2 downForce = new Vector2(0, -1);
	private Vector2 upForce = new Vector2 (0, 1);
	private bool topRight = false;
	private bool topLeft = false;
	private bool bottomRight = false;
	private bool bottomLeft = false;
	
	
	
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
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
		if (Input.touchCount == 1) {
			// If right side of screen is touched
			if (bottomRight || Input.GetKey ("d")) {
				addForce (rightForce, ForceMode2D.Impulse);
			}
			// If left side of screen is touched
			if (bottomLeft || Input.GetKey ("a")) {
				addForce (leftForce, ForceMode2D.Impulse);
			}
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
		if (Input.deviceOrientation == DeviceOrientation.Portrait) {
			Physics2D.gravity = downForce * GRAVITYCONST;
		}
		if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
			Physics2D.gravity = upForce * GRAVITYCONST;
		}
		if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft) {
			Physics2D.gravity = leftForce * GRAVITYCONST;
		}
		if (Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
			Physics2D.gravity = rightForce * GRAVITYCONST;
		}
		
		//Physics.gravity = LowPassFilterAccelerometer (LowPassFilterFactor);
		if (Input.touchCount == 0) {
			resetMovementFlags ();
		}
	}
	
	//Flag Handling for buttons
	public void TopRightDown(){
		topRight = true;
	}
	
	public void TopRightUp(){
		topRight = false;
	}
	
	public void TopLeftDown(){
		topLeft = true;
	}
	
	public void TopLeftUp(){
		topLeft = false;
	}
	
	public void BottomRightDown(){
		bottomRight = true;
	}
	
	public void BottomRightUp(){
		bottomRight = false;
	}
	
	public void BottomLeftDown(){
		bottomLeft = true;
	}
	
	public void BottomLeftUp(){
		bottomLeft = false;
	}

	public void resetMovementFlags(){
		topRight = false;
		topLeft = false;
		bottomRight = false;
		bottomLeft = false;
	}
	/*Vector3 LowPassFilterAccelerometer(float filter){ 
		float xfilter = Mathf.Lerp (lowPassValue.x, Input.acceleration.x, filter);
		float yfilter = Mathf.Lerp (lowPassValue.y, Input.acceleration.y, filter);
		lowPassValue = new Vector3(xfilter, yfilter, 0f);
		return lowPassValue;
	}*/
	
	public void addForce(Vector2 vect, ForceMode2D force){
		rb.AddForce (vect, force);
	}
	
}
