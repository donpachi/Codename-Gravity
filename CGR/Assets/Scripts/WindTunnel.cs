using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindTunnel : MonoBehaviour {

    private List<Vector3> windRayOrigins;

	//Adjustable variables
	public float MaxWindForce;
	public float MaxWindDistance; //size of the box collider
    public bool TurbineOn;
    public int RayAmount; //the number of rays cast
    public Dictionary<GameObject, float> objList;
    public LayerMask MoveableObjects;

    private Player player;
    private RaycastHit2D[] windRayHits;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Player>();
        setupRaycast();
        windRayHits = new RaycastHit2D[5];
        objList = new Dictionary<GameObject, float>();
    }
	
	// Update is called once per frame
	void Update () {
        if (TurbineOn)
        {
            castRays();
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
    void castRays()
    {
        //Dictionary<GameObject, float> pushableObjects = new Dictionary<GameObject, float>();
        objList.Clear();
        foreach (Vector3 ray in windRayOrigins)
        {
            //windRayHits = Physics2D.RaycastAll(ray, transform.up, MaxWindDistance, MoveableObjects);
            int hits = Physics2D.RaycastNonAlloc(ray, transform.up, windRayHits, MaxWindDistance, MoveableObjects);

            for (int i = 0; i < hits; i++)
            {
                if (windRayHits[i].collider != null)
                {
                    if(windRayHits[i].collider.gameObject.layer == LayerMask.NameToLayer("Walls"))
                    {
                        break;
                    }
                    if (!objList.ContainsKey(windRayHits[i].collider.gameObject))
                    {
                        if(windRayHits[i].collider.gameObject == !player || !player.suctionStatus || player.InAirState())
                        {
                            float distance = Vector2.Distance(ray, windRayHits[i].collider.gameObject.transform.position);
                            float windForce = ((MaxWindDistance - distance) / MaxWindDistance) * MaxWindForce;
                            if (windForce < 0)
                            {
                                windForce = 0;
                            }
                            objList.Add(windRayHits[i].collider.gameObject, windForce);
                        }
                    }
                }
            }
            Debug.DrawRay(ray, transform.up * MaxWindDistance, Color.green, 1);

        }

        //return pushableObjects;
    }
}

