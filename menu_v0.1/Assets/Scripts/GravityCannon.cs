using UnityEngine;
using System.Collections;

public class GravityCannon : MonoBehaviour {

	private GameObject player;
	private Rigidbody playerBody;
	private bool activated = false;
	private float ENTRYDISTANCE = 1.5f;
	private float LAUNCHFORCE = 5f;
	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		//playerBody = player.GetComponent<Rigidbody>
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (player.GetComponent<Transform> ().position, this.GetComponent<Transform> ().position);

		if (distance < ENTRYDISTANCE)
			activated = true;

		if (activated) {
			//playerBody.
		}

	}
}
