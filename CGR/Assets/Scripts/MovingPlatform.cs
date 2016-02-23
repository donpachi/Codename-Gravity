using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatform : MonoBehaviour {

    public bool MoveRight;
    public bool MoveDown;
    public float XDistance = -1;
    public float YDistance = -1;
    public int numberOfMovements = -1;
    private float XDistRemain;
    private float YDistRemain;
    public float speed;
    public bool isActive;
    private float RAYCASTDISTANCE = 0.3f;
    private float SPEEDMULTIPLIER = 10f;
    private bool playerOnTop = false;
    Vector2 moveDifference;
    int numberOfIncrements = 0;
    

	// Use this for initialization
	void Start () {
        XDistRemain = XDistance;
        YDistRemain = YDistance;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isActive == true)
        {
            Vector2 vector = this.transform.position;
            Vector2 originalPosition = this.transform.position;

            // Left
            if (MoveRight == false && XDistance != -1 && numberOfMovements != 0)
            {
                bool originalDirection = MoveRight;
                float offset = movePlatform(ref MoveRight, ref XDistance, ref XDistRemain);
                if (originalDirection != MoveRight)
                    XDistRemain = XDistance;
                vector = new Vector2(vector.x - offset, vector.y);
                this.transform.position = vector;
            }

            // Right
            else if (MoveRight == true && XDistance != -1 && numberOfMovements != 0)
            {
                bool originalDirection = MoveRight;
                float offset = movePlatform(ref MoveRight, ref XDistance, ref XDistRemain);
                if (originalDirection != MoveRight)
                    XDistRemain = XDistance;
                vector = new Vector2(vector.x + offset, vector.y);
                this.transform.position = vector;
            }

            // Down
            if (MoveDown == true && YDistance != -1 && numberOfMovements != 0)
            {
                bool originalDirection = MoveDown;
                float offset = movePlatform(ref MoveDown, ref YDistance, ref YDistRemain);
                if (originalDirection != MoveDown)
                    YDistRemain = YDistance;
                vector = new Vector2(vector.x, vector.y - offset);
                this.transform.position = vector;
            }

            // Up
            else if (MoveDown == false && YDistance != -1 && numberOfMovements != 0)
            {
                bool originalDirection = MoveDown;
                float offset = movePlatform(ref MoveDown, ref YDistance, ref YDistRemain);
                if (originalDirection != MoveDown)
                    YDistRemain = YDistance;
                vector = new Vector2(vector.x, vector.y + offset);
                this.transform.position = vector;
            }

            moveDifference = new Vector2(originalPosition.x - vector.x, originalPosition.y - vector.y);

            // Raycast upwards to see if an object needs to be moved
            List<GameObject> objectsOnTop = raycastUp();

            if (objectsOnTop != null)
            {
                foreach (GameObject collidingObject in objectsOnTop) 
                    collidingObject.transform.position = new Vector2(collidingObject.transform.position.x - moveDifference.x, collidingObject.transform.position.y - moveDifference.y);
            }
            
        }
	}

    float movePlatform(ref bool direction, ref float originalDistance, ref float distanceRemaining)
    {
        if (distanceRemaining - speed / SPEEDMULTIPLIER < 0 || distanceRemaining - speed / SPEEDMULTIPLIER == 0)
        {
            numberOfIncrements++;
            direction = !direction;
            numberOfMovements--;
            //distanceRemaining = originalDistance;
            if (distanceRemaining - speed / SPEEDMULTIPLIER < 0)
            {
                return distanceRemaining;
            }
            //else 
                //return 0;
        }
        distanceRemaining -= speed / SPEEDMULTIPLIER;
        return speed / SPEEDMULTIPLIER;        
    }


    List<GameObject> raycastUp()
    {
        List<GameObject> collidingObjects = new List<GameObject>();
        Vector2 raycastOrigin = new Vector2(0, 0);
        float originOffset = 0f;

        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT) 
            raycastOrigin = new Vector2(this.transform.position.x, this.GetComponent<SpriteRenderer>().bounds.max.y + 0.1f);
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT) 
            raycastOrigin = new Vector2(this.transform.position.x, this.GetComponent<SpriteRenderer>().bounds.min.y - 0.1f);
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_LEFT)
            raycastOrigin = new Vector2(this.GetComponent<SpriteRenderer>().bounds.max.x + 0.1f, this.transform.position.y);
        else if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.LANDSCAPE_RIGHT)
            raycastOrigin = new Vector2(this.GetComponent<SpriteRenderer>().bounds.min.x - 0.1f, this.transform.position.y);

        
        if (OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.PORTRAIT || OrientationListener.instanceOf.currentOrientation() == OrientationListener.Orientation.INVERTED_PORTRAIT)
        {
            while (raycastOrigin.x + originOffset <= this.GetComponent<SpriteRenderer>().bounds.max.x)
            {
                RaycastHit2D upRaycastRight = Physics2D.Raycast(new Vector2(raycastOrigin.x + originOffset, raycastOrigin.y), OrientationListener.instanceOf.getRelativeUpVector(), RAYCASTDISTANCE);
                RaycastHit2D upRaycastLeft = Physics2D.Raycast(new Vector2(raycastOrigin.x - originOffset, raycastOrigin.y), OrientationListener.instanceOf.getRelativeUpVector(), RAYCASTDISTANCE);
                if (upRaycastLeft.collider != null && !collidingObjects.Contains(upRaycastLeft.collider.gameObject) && upRaycastLeft.collider.gameObject.tag == "Pushable")
                    collidingObjects.Add(upRaycastLeft.collider.gameObject);
                if (upRaycastRight.collider != null && !collidingObjects.Contains(upRaycastRight.collider.gameObject) && upRaycastRight.collider.gameObject.tag == "Pushable") 
                    collidingObjects.Add(upRaycastRight.collider.gameObject);
                originOffset += 0.5f;
            }
        }
        else
        {
            while (raycastOrigin.y + originOffset <= this.GetComponent<SpriteRenderer>().bounds.max.y)
            {
                RaycastHit2D upRaycastRight = Physics2D.Raycast(new Vector2(raycastOrigin.x, raycastOrigin.y + originOffset), OrientationListener.instanceOf.getRelativeUpVector(), RAYCASTDISTANCE);
                RaycastHit2D upRaycastLeft = Physics2D.Raycast(new Vector2(raycastOrigin.x, raycastOrigin.y - originOffset), OrientationListener.instanceOf.getRelativeUpVector(), RAYCASTDISTANCE);
                if (upRaycastLeft.collider != null && !collidingObjects.Contains(upRaycastLeft.collider.gameObject) && upRaycastLeft.collider.gameObject.tag == "Pushable")
                    collidingObjects.Add(upRaycastLeft.collider.gameObject);
                if (upRaycastRight.collider != null && !collidingObjects.Contains(upRaycastRight.collider.gameObject) && upRaycastRight.collider.gameObject.tag == "Pushable")
                    collidingObjects.Add(upRaycastRight.collider.gameObject);
                originOffset += 0.5f;
            }
        }
        
        return collidingObjects;
    }


    void plateDepressed()
    {
        isActive = true;
    }

    void plateReleased()
    {
        isActive = false;
    }
}
