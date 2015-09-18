using UnityEngine;
using System.Collections;
using System;

public class Water : MonoBehaviour {
	
	Controls controls;
	float WATERRUNOFFRATE = 0.8f;
	float waterScale = 1;
	float DISAPPEARTHRESHOLD = 0.05f;
	bool resizing = false;
	DeviceOrientation deviceOrientation;
	string lengthwise = "x";
	string heightwise = "y";

	// Use this for initialization
	void Start () {
		deviceOrientation = Input.deviceOrientation;
		controls = GameObject.FindGameObjectWithTag("Player").GetComponent<Controls>();
		AddListener (controls);
	}

	private void AddListener (Controls controls)
	{
		controls.OrientationChange += HandleOrientationChange;
	}

	public void HandleOrientationChange() {
		deviceOrientation = Input.deviceOrientation;
		if (deviceOrientation == DeviceOrientation.Portrait || deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
			lengthwise = "x";
			heightwise = "y";
		}
		else if (deviceOrientation == DeviceOrientation.LandscapeLeft || deviceOrientation == DeviceOrientation.LandscapeRight) {
			lengthwise = "y";
			heightwise = "x";
		}
		resizing = true;
	}

	public void resizeWater() {
		RaycastHit2D leftedgehit;
		RaycastHit2D rightedgehit;
		if (lengthwise == "x") {
			Vector2 leftRayOrigin = new Vector2 (this.transform.position.x - Math.Abs(this.GetComponent<SpriteRenderer>().bounds.min.x), this.transform.position.y);
			leftedgehit = Physics2D.Raycast (leftRayOrigin, Vector2.down);
			Vector2 rightRayOrigin = new Vector2 (this.transform.position.x + Math.Abs(this.GetComponent<SpriteRenderer>().bounds.min.x), this.transform.position.y);
			rightedgehit = Physics2D.Raycast (rightRayOrigin, Vector2.down);
		}
		if (lengthwise == "y") {
			Vector2 leftRayOrigin = new Vector2 (this.transform.position.y - Math.Abs (this.GetComponent<SpriteRenderer> ().bounds.min.y), this.transform.position.x);
			leftedgehit = Physics2D.Raycast (leftRayOrigin, Vector2.down);
			Vector2 rightRayOrigin = new Vector2 (this.transform.position.y + Math.Abs (this.GetComponent<SpriteRenderer> ().bounds.min.y), this.transform.position.x);
			rightedgehit = Physics2D.Raycast (rightRayOrigin, Vector2.down);
		}
		Vector2 scale = this.transform.localScale;
		scale.y = WATERRUNOFFRATE;
		scale.x = 1/WATERRUNOFFRATE;
		this.transform.localScale = scale;
		waterScale = waterScale * WATERRUNOFFRATE;
		if (waterScale <= DISAPPEARTHRESHOLD) {
			Destroy(this);
		}
	}

	public void OnCollision2D(Collider2D colliderEvent) {
			resizing = false;
	}

	// Update is called once per frame
	void Update () {
		if (resizing == true) {
			resizeWater();
		}

	}
}
