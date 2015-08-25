using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	private bool alive = true;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void addForce(Vector3 vector) {

	}

	public bool getStatus() {
		return alive;
	}

	public void setStatus(bool status) {
		alive = status;
	}

	void OnCollisionEnter(Collision obj) {
		if (obj.gameObject.tag == "Hazard") {
			alive = false;
		}
	}


}
