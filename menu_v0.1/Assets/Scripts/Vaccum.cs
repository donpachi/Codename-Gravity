using UnityEngine;
using System.Collections;

public class Vaccum : MonoBehaviour {
	
	private bool inTunnelPath;
	private GameObject player;
	private float vacForce;
	
	//Adjustable variables
	public float MaxVacForce;
	public float MaxVacDistance; //size of the box collider
	public Vector2 Direction; //Sets the default orientation or of the force, will rotate with game object
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		inTunnelPath = false;
		vacForce = 1;
		Direction = gameObject.GetComponent<Transform> ().rotation * Direction;
	}
	
	// Update is called once per frame
	void Update () {
		if (inTunnelPath) {
			addVacForce();
		}
	}
	
	void addVacForce(){
		
		float distance = Vector2.Distance (player.GetComponent<Transform> ().position, gameObject.GetComponent<Transform> ().position);

		vacForce = (MaxVacDistance -  Mathf.Abs(distance)) / MaxVacDistance * MaxVacForce;

		if (vacForce < 0) {
			vacForce = 0;
		}

		Debug.Log ("distance: " + distance + " force: " + vacForce);

		player.GetComponent<Rigidbody2D>().AddForce(Direction.normalized * vacForce);
	}
	
	void OnTriggerEnter2D(Collider2D collision){
		inTunnelPath = true;
	}
	
	void OnTriggerExit2D(Collider2D collision){
		inTunnelPath = false;
	}
}
