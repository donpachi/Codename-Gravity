using UnityEngine;
using System.Collections;

public class WindTunnel : MonoBehaviour {

	private bool inTunnelPath;
	private GameObject player;
	private float windForce;

	//Adjustable variables
	public float MaxWindForce;
	public float MaxWindDistance; //size of the box collider
	public Vector2 Direction; //manually adjust in unity with orientation for now

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		inTunnelPath = false;
		windForce = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (inTunnelPath) {
			addWindForce();
		}
	}

	void addWindForce(){
		//need to implement scalar force

		float distance = Vector2.Distance (player.GetComponent<Transform> ().position, gameObject.GetComponent<Transform> ().position);

		windForce = (MaxWindDistance - distance)/ MaxWindDistance * MaxWindForce;

		player.GetComponent<Rigidbody2D>().AddForce(Direction.normalized * windForce);
	}

	void OnTriggerEnter2D(Collider2D collision){
		inTunnelPath = true;
	}

	void OnTriggerExit2D(Collider2D collision){
		inTunnelPath = false;
	}
}
