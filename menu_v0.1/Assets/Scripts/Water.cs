using UnityEngine;
using System.Collections;
using System;

public class Water : MonoBehaviour {
	
	Controls controls;
	float WATERRUNOFFRATE = 0.8f;
	float waterScale = 1f;
	float DISAPPEARTHRESHOLD = 0.05f;
	bool resizing = false;
	float originalLengthOfWaterBody = 0f;
	DeviceOrientation previousOrientation;
	int frameCounter = 0;

	// Use this for initialization
	void Start () {

		controls = GameObject.FindGameObjectWithTag("Player").GetComponent<Controls>();
		AddListener (controls);
		originalLengthOfWaterBody = this.GetComponent<SpriteRenderer>().bounds.size.x;
		previousOrientation = Input.deviceOrientation;
	}

	// Vector array [left, right, up, down]
	Vector2 [] returnVectors() {
		Vector2 [] vectorArray = new Vector2[4];
		if (Input.deviceOrientation == DeviceOrientation.Portrait) {
			vectorArray[0] = Vector2.left;
			vectorArray[1] = Vector2.right;
			vectorArray[2] = Vector2.up;
			vectorArray[3] = Vector2.down;
			return vectorArray;
		}
		else if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
			vectorArray[0] = Vector2.right;
			vectorArray[1] = Vector2.left;
			vectorArray[2] = Vector2.down;
			vectorArray[3] = Vector2.up;
			return vectorArray;
		}
		else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft) {
			vectorArray[0] = Vector2.up;
			vectorArray[1] = Vector2.down;
			vectorArray[2] = Vector2.right;
			vectorArray[3] = Vector2.left;
			return vectorArray;
		}
		else if (Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
			vectorArray[0] = Vector2.down;
			vectorArray[1] = Vector2.up;
			vectorArray[2] = Vector2.left;
			vectorArray[3] = Vector2.right;
			return vectorArray;
		}
		return null;
	}

	private void AddListener (Controls controls)
	{
		//controls.OrientationChange += HandleOrientationChange;
	}

	public void HandleOrientationChange() {
		resizing = true;
	}

	public void resizeWater() {
		RaycastHit2D leftsidedownhit;
		RaycastHit2D rightsidedownhit;
		RaycastHit2D leftsidehit;
		RaycastHit2D rightsidehit;
		
		Vector2 leftRayOrigin;
		Vector2 rightRayOrigin;

		if (Input.deviceOrientation == DeviceOrientation.Portrait) { // if (controls.getOrientationInfo() == "portrait") {
			leftRayOrigin = new Vector2 (this.transform.position.x - Math.Abs (this.GetComponent<SpriteRenderer> ().bounds.min.x), this.transform.position.y);
			leftsidehit = Physics2D.Raycast (leftRayOrigin, Vector2.left);
			rightRayOrigin = new Vector2 (this.transform.position.x + Math.Abs (this.GetComponent<SpriteRenderer> ().bounds.min.x), this.transform.position.y);
			rightsidehit = Physics2D.Raycast (rightRayOrigin, Vector2.right);
		} else if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
			leftRayOrigin = new Vector2 (this.transform.position.x + Math.Abs (this.GetComponent<SpriteRenderer> ().bounds.min.x), this.transform.position.y);
			leftsidehit = Physics2D.Raycast (leftRayOrigin, Vector2.right);
			rightRayOrigin = new Vector2 (this.transform.position.x - Math.Abs (this.GetComponent<SpriteRenderer> ().bounds.min.x), this.transform.position.y);
			rightsidehit = Physics2D.Raycast (rightRayOrigin, Vector2.left);
		} else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft) {//if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight) { //if (controls.getOrientationInfo() == "landscape") {
			leftRayOrigin = new Vector2 (this.transform.position.y - Math.Abs (this.GetComponent<SpriteRenderer> ().bounds.min.y), this.transform.position.x);
			leftsidehit = Physics2D.Raycast (leftRayOrigin, Vector2.up);
			rightRayOrigin = new Vector2 (this.transform.position.y + Math.Abs (this.GetComponent<SpriteRenderer> ().bounds.min.y), this.transform.position.x);
			rightsidehit = Physics2D.Raycast (rightRayOrigin, Vector2.down);
		} else {// (Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
			leftRayOrigin = new Vector2 (this.transform.position.y + Math.Abs (this.GetComponent<SpriteRenderer> ().bounds.min.y), this.transform.position.x);
			leftsidehit = Physics2D.Raycast (leftRayOrigin, Vector2.down);
			rightRayOrigin = new Vector2 (this.transform.position.y - Math.Abs (this.GetComponent<SpriteRenderer> ().bounds.min.y), this.transform.position.x);
			rightsidehit = Physics2D.Raycast (rightRayOrigin, Vector2.up);
		}
		Physics2D.Raycast (this.transform, Vector2.down, 1, 1 << LayerMask.NameToLayer)("Walls");
		Vector2 scale = this.transform.localScale;
		Vector2 position = this.transform.position;
		BoxCollider2D waterCollider = (BoxCollider2D)this.GetComponent<BoxCollider2D> ();


		/*
		// If there is a wall only on the left side of the water body
		if (leftsidehit.distance < 5 && rightsidehit.distance > 5) {
			if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
				scale.y = scale.y * WATERRUNOFFRATE;
				scale.x = scale.x * 1 / WATERRUNOFFRATE;
				if (Input.deviceOrientation == DeviceOrientation.Portrait) {
					position.x += scale.x * WATERRUNOFFRATE;
				} else if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
					position.x -= scale.x * WATERRUNOFFRATE;
				}
			} else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
				scale.x = scale.x * WATERRUNOFFRATE;
				scale.y = scale.y * 1 / WATERRUNOFFRATE;
				if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft) {
					position.y += scale.y * WATERRUNOFFRATE;
				} else if (Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
					position.y -= scale.y * WATERRUNOFFRATE;
				}
			}
		} 

		// If there is a wall only on the right side of the water body
		else if (leftsidehit.distance > 5 && rightsidehit.distance < 5) {
			if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
				scale.y = scale.y * WATERRUNOFFRATE;
				scale.x = scale.x * 1 / WATERRUNOFFRATE;
				if (Input.deviceOrientation == DeviceOrientation.Portrait) {
					position.x -= scale.x * WATERRUNOFFRATE;
				} else if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) {
					position.x += scale.x * WATERRUNOFFRATE;
				}
			} else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
				scale.x = scale.x * WATERRUNOFFRATE;
				scale.y = scale.y * 1 / WATERRUNOFFRATE;
				if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft) {
					position.y -= scale.y * WATERRUNOFFRATE;
				} else if (Input.deviceOrientation == DeviceOrientation.LandscapeRight) {
					position.y += scale.y * WATERRUNOFFRATE;
				}
			}
		} 
		*/
		// If there are no walls on either side of the water body
		if (leftsidehit.distance > 0.000000000000000000000000000001f && rightsidehit.distance > 0.000000000000000000000000000000001f ){
		
			if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) { //if (controls.getOrientationInfo() == "portrait") {
				scale.y = scale.y * WATERRUNOFFRATE;
				scale.x = scale.x * 1 / WATERRUNOFFRATE;
			} else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight) { //else if (controls.getOrientationInfo () == "landscape") {
				scale.x = scale.x * WATERRUNOFFRATE;
				scale.y = scale.y * 1 / WATERRUNOFFRATE;
			}
			this.transform.localScale = scale;
			//this.transform.position = position;
			//waterScale = waterScale * WATERRUNOFFRATE;
			//if (waterScale <= DISAPPEARTHRESHOLD) {
			//	Destroy (this);
			//}
		}
		// If there are walls on both sides of the water body
		//else {
			//resizing = false;
			/*
			// Spawn another water source
			GameObject newParticleGenerator=(GameObject)Instantiate(Resources.Load("LiquidPhysics/ParticleGenerator"));
			if (leftsidehit.distance > 1) {
				newParticleGenerator.transform.position = leftRayOrigin;
			}
			if (rightsidehit.distance > 1) {
				newParticleGenerator.transform.position = rightRayOrigin;
			}*/
			
		//}
	}

	public void combineWater(Transform childWaterBody) {
		Vector2 scale = this.transform.localScale;
		if (Input.deviceOrientation == DeviceOrientation.Portrait || Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown) { // if (controls.getOrientationInfo () == "portrait") {
			while (this.GetComponent<SpriteRenderer>().bounds.size.x < originalLengthOfWaterBody + childWaterBody.GetComponent<SpriteRenderer>().bounds.size.x) {
				scale.x = this.GetComponent<Transform>().localScale.x + 0.1f;

			}
		} 
		else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight) { // else if (controls.getOrientationInfo () == "landscape") {
			while (this.GetComponent<SpriteRenderer>().bounds.size.y < originalLengthOfWaterBody + childWaterBody.GetComponent<SpriteRenderer>().bounds.size.y) {
				scale.y = this.GetComponent<Transform>().localScale.y + 0.1f;
			}
		}
		this.transform.localScale = scale;
	}
	

	public void OnCollision2D(Collider2D colliderEvent) {
		if (colliderEvent.tag == tag) {
		}
			//resizing = false;
		/*
		else {
			Transform childWaterBody = colliderEvent.GetComponentInChildren<Transform>();
			combineWater(childWaterBody);
			Destroy (colliderEvent.GetComponentInChildren<GameObject>());

		}*/
	}

	// Update is called once per frame
	void Update () {


		// To be deleted once controls is fixed
		if (previousOrientation != Input.deviceOrientation && frameCounter % 6 == 0) {
			resizing = true;
		}
		if (resizing == true) {
			resizeWater ();
		}
		if (Input.deviceOrientation != DeviceOrientation.Unknown && Input.deviceOrientation != DeviceOrientation.FaceDown && Input.deviceOrientation != DeviceOrientation.FaceUp)
			previousOrientation = Input.deviceOrientation;
		frameCounter++;
	}
}
