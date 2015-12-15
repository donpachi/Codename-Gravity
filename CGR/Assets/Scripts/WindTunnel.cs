using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindTunnel : MonoBehaviour {

	private float windForce;
	private Animator anim;
    private List<GameObject> objectArray;
    private GameObject closestObj;
    private List<Vector3> windRayOrigins;

	//Adjustable variables
	public float MaxWindForce;
	public float MaxWindDistance; //size of the box collider
	public Vector2 Direction; //Sets the default orientation or of the force, will rotate with game object
    public bool TurbineOn;
    public float RayIntervals; //the space between each ray


    //TODO Ray cast instead of bounding box. design the prefab so that it is a game object with wind tunnel children, the main game object has this script and bounding box and each wind tunnel just has the sprite and animation

	// Use this for initialization
	void Start () {
		windForce = 1;
		Direction = gameObject.GetComponent<Transform> ().rotation * Direction;
		anim = gameObject.GetComponent<Animator> ();
        objectArray = new List<GameObject>();

        setupRaycast();
        setAnimationState(TurbineOn);

	}
	
	// Update is called once per frame
	void Update () {

        if (TurbineOn)
        {
            addWindForce(castRays());
        }

	}

    /// <summary>
    /// Adds wind force to all the pushable objects
    /// </summary>
    /// <param name="objList"></param>
    void addWindForce(List<GameObject> objList)
    {
        float distance;

        foreach (GameObject obj in objList)
        {
            distance = Vector2.Distance(obj.GetComponent<Transform>().position, transform.position);

            windForce = (MaxWindDistance - distance) / MaxWindDistance * MaxWindForce;

            if (windForce < 0)
            {
                windForce = 0;
            }
            obj.GetComponent<Rigidbody2D>().AddForce(Direction.normalized * MaxWindForce);
            Debug.Log("Added Force to: " + obj.name + " With Force: " + windForce);
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
    List<GameObject> castRays()
    {
        List<GameObject> pushableObjects = new List<GameObject>();  //list of objects that were found
        
        foreach (Vector3 ray in windRayOrigins)
        {
            RaycastHit2D windRay = Physics2D.Raycast(ray, transform.TransformDirection(Vector3.up), MaxWindDistance);               //update this if we want rays to pass through certain objects

            Debug.DrawRay(ray, transform.TransformDirection(Vector3.up) * MaxWindDistance, Color.green, 5);

            if (windRay.collider != null)
            {
                if (windRay.collider.gameObject.CompareTag("Pushable") && !pushableObjects.Contains(windRay.collider.gameObject))
                {
                    pushableObjects.Add(windRay.collider.gameObject);
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
        TurbineOn = !TurbineOn;
        setAnimationState(TurbineOn);
    }

    void plateReleased()
    {

    }
}
