using UnityEngine;
using System.Collections;
using System;

public class Water : MonoBehaviour {
	
    float WATERRUNOFFRATE = 0.05f;
    //float waterScale = 1f;
    bool resizing = false;
    OrientationListener.Orientation previousOrientation;
    float RAYCASTCOLLISIONDISTANCE = 5f;//0.490f;
    float heightAnchor;

    //// Use this for initialization
    void Start()
    {
        this.tag = "Water";
        //originalLengthOfWaterBody = this.GetComponent<SpriteRenderer>().bounds.size.x;
        previousOrientation = OrientationListener.instanceOf.currentOrientation();
    }

    public void checkToSpawn() {

        //RaycastHit2D leftsidedownhit;
        //RaycastHit2D rightsidedownhit;
        RaycastHit2D leftsidehit;
        RaycastHit2D rightsidehit;

        Vector2 leftRayOrigin;
        Vector2 rightRayOrigin;

        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT)
        {
            leftRayOrigin = new Vector2(this.GetComponent<SpriteRenderer>().bounds.min.x, this.transform.position.y);
            rightRayOrigin = new Vector2(this.GetComponent<SpriteRenderer>().bounds.max.x, this.transform.position.y);
            heightAnchor = this.GetComponent<SpriteRenderer>().bounds.min.y;
        }
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
        {
            leftRayOrigin = new Vector2(this.GetComponent<SpriteRenderer>().bounds.max.x, this.transform.position.y);
            rightRayOrigin = new Vector2(this.GetComponent<SpriteRenderer>().bounds.min.x, this.transform.position.y);
            heightAnchor = this.GetComponent<SpriteRenderer>().bounds.max.y;
        }
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT)
        {
            leftRayOrigin = new Vector2(this.transform.position.x, this.GetComponent<SpriteRenderer>().bounds.max.y);
            rightRayOrigin = new Vector2(this.transform.position.x, this.GetComponent<SpriteRenderer>().bounds.min.y);
            heightAnchor = this.GetComponent<SpriteRenderer>().bounds.min.x;
        }
        else
        {
            leftRayOrigin = new Vector2(this.transform.position.x, this.GetComponent<SpriteRenderer>().bounds.min.y);
            rightRayOrigin = new Vector2(this.transform.position.x, this.GetComponent<SpriteRenderer>().bounds.max.y);
            heightAnchor = this.GetComponent<SpriteRenderer>().bounds.max.y;
        }
        leftsidehit = Physics2D.Raycast(leftRayOrigin, OrientationListener.instanceOf.getRelativeLeftVector(), RAYCASTCOLLISIONDISTANCE, 1 << LayerMask.NameToLayer("Walls"));
        rightsidehit = Physics2D.Raycast(rightRayOrigin, OrientationListener.instanceOf.getRelativeRightVector(), RAYCASTCOLLISIONDISTANCE, 1 << LayerMask.NameToLayer("Walls"));

        // If there are walls on both sides of the water body
        if (leftsidehit && rightsidehit)
        {
            Debug.Log("Walls on both sides");
            resizing = false;
        }

    //    // If there is a wall only on the left side of the water body
        else if (leftsidehit && rightsidehit.collider == null)
        {
            Debug.Log("No wall on right side");
            createSpawners(rightRayOrigin);
        }

    //    // If there is a wall only on the right side of the water body
        else if (leftsidehit.collider == null && rightsidehit)
        {
            Debug.Log("No wall on left side");
            //createSpawners(leftRayOrigin);
    
        }

        // If there are no walls on either side of the water body
        else if (!leftsidehit && !rightsidehit)
        {
            Debug.Log("No walls on either side");
        }

    }

    // Create more waterSpawners
    void createSpawners(Vector2 origin)
    {
        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT) 
        {
            for (int i = 0; i < 1; i++)//(float distanceFromOrigin = 0; distanceFromOrigin < (this.GetComponent<SpriteRenderer>().bounds.size.x / 2); distanceFromOrigin += 2)
            {
                Debug.Log("CREATING PARTICLE");
                GameObject spawnerAbove = (GameObject)Instantiate(Resources.Load("Prefabs/ParticleSource"));
                //GameObject spawnerBelow = (GameObject)Instantiate(Resources.Load("Prefabs/ParticleSource"));
                spawnerAbove.transform.parent = this.transform;
                //spawnerBelow.transform.parent = this.transform;
                Vector2 originOffsetAbove = new Vector2(origin.x, origin.y);//(origin.x - distanceFromOrigin, origin.y);
                //Vector2 originOffsetBelow = new Vector2(origin.x + distanceFromOrigin, origin.y);
                spawnerAbove.transform.position = originOffsetAbove;
                //spawnerBelow.transform.position = originOffsetBelow;
            }
        }
        else 
        {
            //for (float distanceFromOrigin = 0; distanceFromOrigin < (this.GetComponent<SpriteRenderer>().bounds.size.y / 2); distanceFromOrigin += 2)
            //{
            //    GameObject spawnerAbove = (GameObject)Instantiate(Resources.Load("Prefabs/ParticleSource"));               
            //    GameObject spawnerBelow = (GameObject)Instantiate(Resources.Load("Prefabs/ParticleSource"));
            //    spawnerAbove.transform.parent = this.transform;
            //    spawnerBelow.transform.parent = this.transform;
            //    Vector2 originOffsetAbove = new Vector2(origin.x, origin.y - distanceFromOrigin);
            //    Vector2 originOffsetBelow = new Vector2(origin.x, origin.y + distanceFromOrigin);
            //    spawnerAbove.transform.position = originOffsetAbove;
            //    spawnerBelow.transform.position = originOffsetBelow;
            //}
        }
    }


    void resizeWater()
    {
        Vector2 scale = this.transform.localScale;
        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT)
        {
            scale.y = scale.y - WATERRUNOFFRATE;      
            if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + WATERRUNOFFRATE);
            else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT)
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - WATERRUNOFFRATE);
        }
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT)
        {
            scale.x = scale.x - WATERRUNOFFRATE;
            if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT)
                this.transform.position = new Vector2(this.transform.position.x + WATERRUNOFFRATE, this.transform.position.y);                
            else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT)
                this.transform.position = new Vector2(this.transform.position.x - WATERRUNOFFRATE, this.transform.position.y);
        }
        this.transform.localScale = scale;
        
        if (scale.x <= 0 || scale.y <= 0)
        {
            Destroy(gameObject);
            resizing = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (previousOrientation != OrientationListener.instanceOf.currentOrientation())
        {       
            checkToSpawn();
            resizing = true;
        }
        if (resizing == true)
        {
            resizeWater();
        }
        previousOrientation = OrientationListener.instanceOf.currentOrientation();
        //checkSidesforWater();
	}
    void checkSidesforWater()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 1, 1 << LayerMask.NameToLayer("Water"));

        if (colliders.Length >= 2)
        {
            foreach (Collider2D objectCollided in colliders)
            {
                Debug.Log("WATER COLLIDED");
                if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
                    this.transform.localScale = new Vector2(this.transform.localScale.x, this.transform.localScale.y + 0.1f);
                if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT)
                    this.transform.localScale = new Vector2(this.transform.localScale.x + 0.1f, this.transform.localScale.y);
                Destroy(objectCollided.gameObject);
            }
        }
    }

}
