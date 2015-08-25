﻿using UnityEngine;
using System.Collections;

public class GravityVortex : MonoBehaviour {
	private GameObject player;
	private float VORTEXDISTANCE = 3;
	private float VORTEXFORCE = 5;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		float distance = Vector3.Distance (player.GetComponent<Transform> ().position, this.GetComponent<Transform> ().position);

		if (distance < VORTEXDISTANCE) {
			Vector3 direction = (this.GetComponent<Transform> ().position - player.GetComponent<Transform> ().position).normalized;
			player.GetComponent<Rigidbody>().AddForce(direction * VORTEXFORCE);
		}
	}

}
