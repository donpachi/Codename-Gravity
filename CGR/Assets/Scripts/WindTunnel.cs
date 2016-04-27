using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindTunnel : MonoBehaviour {

	private Animator anim;
    private List<GameObject> objectArray;
    private List<float> forceValue;
    private GameObject closestObj;
    private List<Vector3> windRayOrigins;

	//Adjustable variables
	public float MaxWindForce;
	public float MaxWindDistance; //size of the box collider
	public Vector2 Direction; //Sets the default orientation or of the force, will rotate with game object
    public bool TurbineOn;
    public float RayIntervals; //the space between each ray

	// Use this for initialization
	void Start () {
		Direction = gameObject.GetComponent<Transform> ().rotation * Direction;
		anim = gameObject.GetComponent<Animator> ();
        objectArray = new List<GameObject>();

        setupRaycast();
        setAnimationState(TurbineOn);

	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (TurbineOn)
        {
            addWindForce(castRays());
        }

	}

    /// <summary>
    /// Adds wind force to all the pushable objects
    /// </summary>
    /// <param name="objList"></param>
    void addWindForce(Dictionary<GameObject, float> objList)
    {
        foreach (KeyValuePair<GameObject, float> entry in objList)
        {
            entry.Key.GetComponent<Rigidbody2D>().AddForce(Direction.normalized * entry.Value);
            Debug.Log("Added Force to: " + entry.Key.name + " With Force: " + entry.Value);
        }

    }

    /// <summary>
    /// Get the width of the wind turbines, then determine where and how many rays will be cast
    /// </summary>
    void setupRaycast()
    {
        windRayOrigins = new List<Vector3>();
        float width = gameObject.GetComponent<BoxCollider2D>().size.x;
        int rays = (int)(width / RayIntervals);
        Vector3 rayOrigin = transform.position - (Vector3)transform.GetComponent<BoxCollider2D>().offset + transform.TransformDirection(Vector3.left) * width/2;

        for (int i = 0; i < rays; i++)
        {
            rayOrigin = rayOrigin + transform.TransformDirection(Vector3.right) * RayIntervals;
            windRayOrigins.Add(rayOrigin);       
        }
        
    }

    /// <summary>
    /// Cast the wind rays to check for objects
    /// </summary>
    /// 
    Dictionary<GameObject, float> castRays()
    {
        //List<WindForceData> pushableObjects = new List<WindForceData>();  //list of objects that were found
        Dictionary<GameObject, float> pushableObjects = new Dictionary<GameObject, float>();
        
        foreach (Vector3 ray in windRayOrigins)
        {
            RaycastHit2D windRay = Physics2D.Raycast(ray, transform.TransformDirection(Vector3.up), MaxWindDistance);               //update this if we want rays to pass through certain objects

            Debug.DrawRay(ray, transform.TransformDirection(Vector3.up) * MaxWindDistance, Color.green, 5);

            if (windRay.collider != null)
            {
                if (windRay.collider.gameObject.CompareTag("Pushable") && !pushableObjects.ContainsKey(windRay.collider.gameObject))
                {
                    float distance = Vector2.Distance(ray, windRay.collider.gameObject.transform.position);
                    float windForce = ((MaxWindDistance - distance) / MaxWindDistance) * MaxWindForce;
                    if (windForce < 0)
                    {
                        windForce = 0;
                    }
                    pushableObjects.Add(windRay.collider.gameObject, windForce);
                }
            }
        }

        return pushableObjects;
    }

    /// <summary>
    /// Function looks throguh all the child objects and changes the animation state for each object
    /// </summary>
    /// 
    void setAnimationState(bool turbineState)
    {
        Animator[] animators = gameObject.GetComponentsInChildren<Animator>();
        foreach (Animator anim in animators)
        {
            anim.SetBool("TurbineOn", turbineState);
        }
    }

    void plateDepressed()
    {
        if (TurbineOn)
        {
            setAnimationState(false);
        }
    }

    void plateReleased()
    {

    }
}

class WindForceData
{
    private GameObject EffectedObject;
    private float WindForce;
    
    public WindForceData()
    {
        WindForce = 0;
    }

    public bool Equals(WindForceData other)
    {
        if (other.EffectedObject == this.EffectedObject)
            return true;
        return false;
    }

    public bool Equals(GameObject other)    //this compares gameobject with windforce
    {
        if (other == this.EffectedObject)
            return true;
        return false;
    }
}
