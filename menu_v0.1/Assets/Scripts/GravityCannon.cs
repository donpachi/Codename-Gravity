using UnityEngine;
using System.Collections;

public class GravityCannon : MonoBehaviour {
	
	private Rigidbody2D playerBody;
	private bool activated = false;
	private float LAUNCHFORCE = 5f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (activated) {
			//playerBody.
		}

	}

	void OnCollisionEnter2D(Collision2D collisionInfo)
	{
		playerBody = collisionInfo.rigidbody;

		if (collisionInfo.gameObject.tag == "Player")
		{
			activated = true;
			Vector3 newPosition = new Vector3 (playerBody.position.x,
				                               playerBody.position.y,
				                               1);
			playerBody.GetComponent<Transform>().position = (newPosition);
			playerBody.Sleep();
		}
	}
}
