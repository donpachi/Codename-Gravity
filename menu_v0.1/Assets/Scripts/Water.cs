﻿using UnityEngine;
using System.Collections;
using System;

public class Water : MonoBehaviour {
	
	Controls controls;
	float WATERRUNOFFRATE = 0.8f;
	float waterScale = 1f;
	float DISAPPEARTHRESHOLD = 0.05f;
	bool resizing = false;
	float originalLengthOfWaterBody = 0f;
	OrientationListener.Orientation previousOrientation;
	int frameCounter = 0;
    float RAYCASTCOLLISIONDISTANCE = 0.486f;

	// Use this for initialization
	void Start () {

		controls = GameObject.FindGameObjectWithTag("Player").GetComponent<Controls>();
		AddListener (controls);
		originalLengthOfWaterBody = this.GetComponent<SpriteRenderer>().bounds.size.x;
        previousOrientation = OrientationListener.instanceOf.currentOrientation();
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


        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT)
        {
            leftRayOrigin = new Vector2(this.GetComponent<SpriteRenderer>().bounds.min.x, this.transform.position.y);          
            rightRayOrigin = new Vector2(this.GetComponent<SpriteRenderer>().bounds.max.x, this.transform.position.y);
        }
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
        {
            leftRayOrigin = new Vector2(this.GetComponent<SpriteRenderer>().bounds.max.x, this.transform.position.y);
            rightRayOrigin = new Vector2(this.GetComponent<SpriteRenderer>().bounds.min.x, this.transform.position.y);
        }
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT)
        {
            leftRayOrigin = new Vector2(this.transform.position.x, this.GetComponent<SpriteRenderer>().bounds.max.y);
            rightRayOrigin = new Vector2(this.transform.position.x, this.GetComponent<SpriteRenderer>().bounds.min.y);
            
        }
        else
        {
            leftRayOrigin = new Vector2(this.transform.position.x, this.GetComponent<SpriteRenderer>().bounds.min.y);
            rightRayOrigin = new Vector2(this.transform.position.x, this.GetComponent<SpriteRenderer>().bounds.max.y);
            
        }
        leftsidehit = Physics2D.Raycast(leftRayOrigin, OrientationListener.instanceOf.getRelativeLeftVector(), RAYCASTCOLLISIONDISTANCE, 1 << LayerMask.NameToLayer("Walls"));
        rightsidehit = Physics2D.Raycast(rightRayOrigin, OrientationListener.instanceOf.getRelativeRightVector(), RAYCASTCOLLISIONDISTANCE, 1 << LayerMask.NameToLayer("Walls"));

        
        //Physics2D.Raycast (this.transform, Vector2.down, 1, 1 << LayerMask.NameToLayer("Walls"));
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
        if (!leftsidehit && !rightsidehit){
			if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT) {
				scale.y = scale.y * WATERRUNOFFRATE;
				scale.x = scale.x * 1 / WATERRUNOFFRATE;
			} else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT) {
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
        else
        {
            resizing = false;
            /*
        	// Spawn another water source
        	GameObject newParticleGenerator=(GameObject)Instantiate(Resources.Load("LiquidPhysics/ParticleGenerator"));
        	if (leftsidehit.distance > 1) {
        		newParticleGenerator.transform.position = leftRayOrigin;
        	}
        	if (rightsidehit.distance > 1) {
        		newParticleGenerator.transform.position = rightRayOrigin;
        	}*/

        }
    }

	// Update is called once per frame
	void FixedUpdate () {


		if (previousOrientation != OrientationListener.instanceOf.currentOrientation()) {
			resizing = true;
		}
		if (resizing == true) {
			resizeWater ();
		}
	    previousOrientation = OrientationListener.instanceOf.currentOrientation();
		frameCounter++;
	}
}
