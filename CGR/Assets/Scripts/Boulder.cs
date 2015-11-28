using UnityEngine;
using System.Collections;

public class Boulder : MonoBehaviour {

	public Rigidbody2D boulderRB2D;
	public float boulderSpeed;
    public float BoulderDeathSpeed;
	private Rigidbody2D playerBody;
	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		boulderSpeed = boulderRB2D.velocity.magnitude;
	}

	void OnCollisionEnter2D(Collision2D collisionInfo) {
		if (collisionInfo.gameObject.tag == "Player") {
			playerBody = collisionInfo.rigidbody;
            if (boulderSpeed > BoulderDeathSpeed && collisionInfo.relativeVelocity.magnitude > BoulderDeathSpeed)
            {
				player.GetComponent<Player>().TriggerDeath();
			}
		}
	}
}