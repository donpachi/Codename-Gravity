using UnityEngine;
using System.Collections;

public class Boulder : MonoBehaviour {

	public Rigidbody2D boulderRB2D;

	private Rigidbody2D playerBody;
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collisionInfo) {
		if (collisionInfo.gameObject.tag == "Player") {
			playerBody = collisionInfo.rigidbody;
			if (boulderRB2D.velocity.magnitude > 7.0f) {
				print(boulderRB2D.velocity.magnitude.ToString()+ " " + playerBody.velocity.magnitude.ToString());
				player.GetComponent<Player>().TriggerDeath();
			}
		}
	}
}
