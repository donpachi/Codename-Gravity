using UnityEngine;
using System.Collections;
using System;

public class Water : MonoBehaviour {
	
	Controls controls;
	float WATERRUNOFFRATE = 0.9f;
	float waterScale = 1f;
	float DISAPPEARTHRESHOLD = 0.05f;
	bool resizing = false;
	float originalLengthOfWaterBody = 0f;
	OrientationListener.Orientation previousOrientation;
	int frameCounter = 0;
    float RAYCASTCOLLISIONDISTANCE = 0.490f;

	// Use this for initialization
	void Start () {

		controls = GameObject.FindGameObjectWithTag("Player").GetComponent<Controls>();
		originalLengthOfWaterBody = this.GetComponent<SpriteRenderer>().bounds.size.x;
        previousOrientation = OrientationListener.instanceOf.currentOrientation();
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


        Vector2 scale = this.transform.localScale;
        Vector2 position = this.transform.position;


        // If there are walls on both sides of the water body
        if (leftsidehit && rightsidehit)
        {
            Debug.Log("Walls on both sides");
            resizing = false;
        }

        // If there is a wall only on the left side of the water body
        else if (leftsidehit && rightsidehit.collider == null)
        {
            if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT)
            {
                scale.y = scale.y * WATERRUNOFFRATE;
                scale.x = scale.x * 1 / WATERRUNOFFRATE;
                if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
                {
                    position.x -= 1/WATERRUNOFFRATE;
                }
                else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT)
                {
                    position.x += 1/WATERRUNOFFRATE;
                }
            }
            else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT)
            {
                scale.x = scale.x * WATERRUNOFFRATE;
                scale.y = scale.y * 1 / WATERRUNOFFRATE;
                if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT)
                {
                    position.y -= 1/WATERRUNOFFRATE;
                }
                else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT)
                {
                    position.y += 1/WATERRUNOFFRATE;
                }
            }

            
            this.transform.position = position;
            this.transform.localScale = scale;
            //Debug.Log("Leftsidehit " + position);
        }

        // If there is a wall only on the right side of the water body
        else if (leftsidehit.collider == null && rightsidehit)
        {
            if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT)
            {
                scale.y = scale.y * WATERRUNOFFRATE;
                scale.x = scale.x * 1 / WATERRUNOFFRATE;
                if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
                {
                    position.x += 1/WATERRUNOFFRATE;
                }
                else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT)
                {
                    position.x -= 1/WATERRUNOFFRATE;
                }
            }
            else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT)
            {
                scale.x = scale.x * WATERRUNOFFRATE;
                scale.y = scale.y * 1 / WATERRUNOFFRATE;
                if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT)
                {
                    position.y += 1/WATERRUNOFFRATE;
                }
                else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT)
                {
                    position.y -= 1/WATERRUNOFFRATE;
                }
            }

            this.transform.position = position;
            this.transform.localScale = scale;
            //Debug.Log("Rightsidehit " + position);
        }


        // If there are no walls on either side of the water body
        else if (!leftsidehit && !rightsidehit){
			if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT) {
				scale.y = scale.y * WATERRUNOFFRATE;
				scale.x = scale.x * 1 / WATERRUNOFFRATE;
			} else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT) {
				scale.x = scale.x * WATERRUNOFFRATE;
				scale.y = scale.y * 1 / WATERRUNOFFRATE;
			}
			this.transform.localScale = scale;
            //if (waterScale <= DISAPPEARTHRESHOLD) {
            //	Destroy (this);
            //}

        }
        
        
    }

	// Update is called once per frame
	void FixedUpdate () {


        if (previousOrientation != OrientationListener.instanceOf.currentOrientation())
        {
            resizing = true;
        }
        if (resizing == true) {
			resizeWater ();
		}
	    previousOrientation = OrientationListener.instanceOf.currentOrientation();
	}
}
