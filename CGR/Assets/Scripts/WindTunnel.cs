using UnityEngine;
using System.Collections;

public class WindTunnel : MonoBehaviour {

	private bool inTunnelPath;
	private GameObject player;
	private float windForce;
	private Animator anim;

	//Adjustable variables
	public float MaxWindForce;
	public float MaxWindDistance; //size of the box collider
	public Vector2 Direction; //Sets the default orientation or of the force, will rotate with game object

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		inTunnelPath = false;
		windForce = 1;
		Direction = gameObject.GetComponent<Transform> ().rotation * Direction;
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (inTunnelPath) {
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

	void OnTriggerEnter2D(Collider2D collision){
		inTunnelPath = true;
		anim.SetTrigger ("StartTurbine");
	}

	void OnTriggerExit2D(Collider2D collision){
		inTunnelPath = false;
	}
}
