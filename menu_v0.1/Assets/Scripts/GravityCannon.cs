using UnityEngine;
using System.Collections;

public class GravityCannon : MonoBehaviour {

	Animator anim;
	int cannonActivate = Animator.StringToHash("CannonActivate");
	private GameObject player;
	private Rigidbody2D playerBody;
	private bool activated = false;
	private float LAUNCHFORCE = 5f;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (activated) {

		}

	}

	void OnCollisionEnter2D(Collision2D collisionInfo)
	{
		playerBody = collisionInfo.rigidbody;

		if (collisionInfo.gameObject.tag == "Player")
		{
			activated = true;
			playerBody.Sleep();
			Vector3 newPosition = new Vector3 (playerBody.position.x,
				                               playerBody.position.y,
				                               1);
			playerBody.GetComponent<Transform>().position = (newPosition);
			playerBody.Sleep();
			player.GetComponent<Controls>().resetMovementFlags();
			anim.SetBool("activated", true);

		}
	}
}
