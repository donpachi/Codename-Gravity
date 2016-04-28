using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindTunnel : MonoBehaviour {

	private Animator anim;
    private List<Vector3> windRayOrigins;

	//Adjustable variables
	public float MaxWindForce;
	public float MaxWindDistance; //size of the box collider
    public bool TurbineOn;
    public int RayAmount; //the number of rays cast
    public Dictionary<GameObject, float> objList;

	// Use this for initialization
	void Start () {
		anim = gameObject.GetComponent<Animator> ();

        setupRaycast();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (TurbineOn)
        {
            objList = castRays();
        }
        else if (objList != null)
            objList.Clear();
	}

    /// <summary>
    /// Adds wind force to all the pushable objects
    /// </summary>
    /// <param name="objList"></param>
    void addWindForce(Dictionary<GameObject, float> objList)
    {
        foreach (KeyValuePair<GameObject, float> entry in objList)
        {
            entry.Key.GetComponent<Rigidbody2D>().AddForce(transform.up * entry.Value);
            //Debug.Log("Added Force to: " + entry.Key.name + " With Force: " + entry.Value);
        }
        //Debug.Log("------");
    }

    /// <summary>
    /// Get the width of the wind turbines, then determine where and how many rays will be cast
    /// </summary>
    void setupRaycast()
    {
        windRayOrigins = new List<Vector3>();
        float width = gameObject.GetComponent<BoxCollider2D>().size.x;
        float rayIntervals = width / RayAmount;
        Vector3 rayOrigin = transform.position + transform.TransformDirection(Vector3.left) * width/2 + (-transform.right * rayIntervals) / 2;

        for (int i = 0; i < RayAmount; i++)
        {
            rayOrigin = rayOrigin + transform.TransformDirection(Vector3.right) * rayIntervals;
            windRayOrigins.Add(rayOrigin);       
        }
    }

    /// <summary>
    /// Cast the wind rays to check for objects
    /// </summary>
    /// 
    Dictionary<GameObject, float> castRays()
    {
        Dictionary<GameObject, float> pushableObjects = new Dictionary<GameObject, float>();
        
        foreach (Vector3 ray in windRayOrigins)
        {
            RaycastHit2D windRay = Physics2D.Raycast(ray, transform.up, MaxWindDistance);               //update this if we want rays to pass through certain objects

            Debug.DrawRay(ray, transform.up * MaxWindDistance, Color.green, 1);

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
}

