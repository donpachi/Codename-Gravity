using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

public class Player : MonoBehaviour {

	// Use this for initialization
	public delegate void DeathEventHandler(object sender, EventArgs e);
	public event DeathEventHandler PlayerDied;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void addForce(Vector3 vector) {

	}


	void OnCollisionEnter(Collision obj) {
		if (obj.gameObject.tag == "Hazard") {

		}
	}

	protected virtual void OnDeath(EventArgs e) {
		if (PlayerDied != null)
			PlayerDied (this, e);
	}


}
