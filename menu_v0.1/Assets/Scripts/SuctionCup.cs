using UnityEngine;
using System.Collections;

public class SuctionCup : MonoBehaviour {

	public GameObject player;

	private ConstantForce2D playerConsForce;
	private Vector2 suctionVector;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collisionInfo) {
		print ("hello");
		if (collisionInfo.gameObject.tag == "Player") {
			suctionVector = Physics2D.gravity;
			collisionInfo.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
			playerConsForce = player.GetComponent<ConstantForce2D>();
			playerConsForce.relativeForce = suctionVector * 2;
		}
	}
}
