﻿using UnityEngine;
using System.Collections;

public class GravityVortex : MonoBehaviour {
	private GameObject player;

	//Adjustable constants
	public float VORTEXDISTANCE;
	public float VORTEXFORCE;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float distance = Vector2.Distance (player.GetComponent<Transform>().position, this.GetComponent<Transform> ().position);

		if (distance < VORTEXDISTANCE && !player.GetComponent<Player>().IsInTransition()) {
			Vector2 direction = (this.GetComponent<Transform>().position - player.GetComponent<Transform> ().position).normalized;
			player.GetComponent<Rigidbody2D>().AddForce(direction * VORTEXFORCE * (distance / VORTEXDISTANCE));
		}
	}

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, VORTEXDISTANCE);
    }

}
