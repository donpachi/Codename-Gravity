using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WindTunnel : MonoBehaviour {

	private bool inTunnelPath;
	private GameObject player;
	private float windForce;
	private Animator anim;
    private List<GameObject> objectArray;
    private GameObject closestObj;

	//Adjustable variables
	public float MaxWindForce;
	public float MaxWindDistance; //size of the box collider
	public Vector2 Direction; //Sets the default orientation or of the force, will rotate with game object
    public bool TurbineOn;


    //TODO Ray cast instead of bounding box. design the prefab so that it is a game object with wind tunnel children, the main game object has this script and bounding box and each wind tunnel just has the sprite and animation

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		inTunnelPath = false;
		windForce = 1;
		Direction = gameObject.GetComponent<Transform> ().rotation * Direction;
		anim = gameObject.GetComponent<Animator> ();
        objectArray = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (inTunnelPath && TurbineOn) {
			addWindForce();
		}
	}

	void addWindForce(){

		float distance = Vector2.Distance (player.GetComponent<Transform> ().position, gameObject.GetComponent<Transform> ().position);

		windForce = (MaxWindDistance - distance)/ MaxWindDistance * MaxWindForce;

		if (windForce < 0) {
			windForce = 0;
		}

		player.GetComponent<Rigidbody2D>().AddForce(Direction.normalized * windForce);
	}

	void OnTriggerEnter2D(Collider2D collision)
    {
        float distance = (collision.gameObject.transform.position - gameObject.transform.position).magnitude;
        if (!objectArray.Contains(collision.gameObject))
        {
            //check if this is the closest object
            if (closestObj == null)
            {
                closestObj = collision.gameObject;
            }
            else if (distance < (closestObj.transform.position - gameObject.transform.position).magnitude)
            {
                closestObj = collision.gameObject;
            }
            //add to array
            objectArray.Add(collision.gameObject);
        }
		inTunnelPath = true;
        Debug.Log(objectArray.Count);
        Debug.Log(closestObj.name);
	}

    void OnTriggerStay2D(Collider2D collision)
    {

    }

	void OnTriggerExit2D(Collider2D collision)
    {
        //remove from array
        Debug.Log(objectArray.Remove(collision.gameObject));
		inTunnelPath = false;
	}

    void plateDepressed()
    {
        TurbineOn = !TurbineOn;
        anim.SetBool("TurbineOn", TurbineOn);
    }

    void plateReleased()
    {

    }
}
