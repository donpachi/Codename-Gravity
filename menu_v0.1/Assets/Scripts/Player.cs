using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Use this for initialization
	public event Player OnPlayerDeath;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void addForce(Vector3 vector) {

	}


	void OnCollisionEnter(Collision obj) {
		if (obj.gameObject.tag == "Hazard") {
			OnPlayerDeath();
		}
	}


}
